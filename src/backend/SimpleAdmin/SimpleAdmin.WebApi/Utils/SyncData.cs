using System.Data.Common;
using FreeSql;
using FreeSql.DataAnnotations;
using Newtonsoft.Json;
using SimpleAdmin.Infrastructure.Configuration.Options.Nodes.Connection;

namespace SimpleAdmin.WebApi.Utils;

public abstract class SyncData
{
    /// <summary>
    ///     检查实体属性是否为自增长
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private static bool CheckIdentity<T>() where T : class
    {
        var properties = typeof(T).GetProperties();

        return properties.Any(property =>
                                  property.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault()
                                      is ColumnAttribute { IsIdentity: true });
    }

    protected virtual T[] GetData<T>(bool isTenant = false, string path = "InitData")
    {
        var table    = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var fileName = $"{table!.Name}{(isTenant ? ".tenant" : "")}.json";
        var filePath = Path.Combine(AppContext.BaseDirectory, $"{path}/{fileName}");
        if (!File.Exists(filePath)) {
            var msg = $"文件{filePath}不存在";
            Console.WriteLine(msg);
            throw new Exception(msg);
        }

        var jsonData = File.ReadAllText(filePath);
        var data     = JsonConvert.DeserializeObject<T[]>(jsonData);

        return data;
    }

    /// <summary>
    ///     初始化数据表数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="db"></param>
    /// <param name="unitOfWork"></param>
    /// <param name="tran"></param>
    /// <param name="data"></param>
    /// <param name="dbConfig"></param>
    /// <returns></returns>
    protected virtual async Task InitDataAsync<T>(IFreeSql      db,
                                                  IUnitOfWork   unitOfWork,
                                                  DbTransaction tran,
                                                  T[]           data,
                                                  ServersNode   server) where T : class, new()
    {
        var table     = typeof(T).GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault() as TableAttribute;
        var tableName = table!.Name;

        try {
            if (await db.Queryable<T>().AnyAsync()) {
                Console.WriteLine($" table: {tableName} record already exists");
                return;
            }

            if (!(data?.Length > 0)) {
                Console.WriteLine($" table: {tableName} import data []");
                return;
            }

            var repo   = db.GetRepository<T>();
            var insert = db.Insert<T>();
            if (unitOfWork != null) {
                repo.UnitOfWork = unitOfWork;
                insert          = insert.WithTransaction(tran);
            }

            var isIdentity = CheckIdentity<T>();
            if (isIdentity) {
                if (server.DbType == DataType.SqlServer) {
                    var insrtSql = insert.AppendData(data).InsertIdentity().ToSql();
                    await
                        repo.Orm.Ado
                            .ExecuteNonQueryAsync($"SET IDENTITY_INSERT {tableName} ON\n {insrtSql} \nSET IDENTITY_INSERT {tableName} OFF");
                }
                else {
                    await insert.AppendData(data).InsertIdentity().ExecuteAffrowsAsync();
                }
            }
            else {
                repo.DbContextOptions.EnableCascadeSave = true;
                await repo.InsertAsync(data);
            }

            Console.WriteLine($" table: {tableName} sync data succeed");
        }
        catch (Exception ex) {
            Console.WriteLine($" table: {tableName} sync data failed.\n{ex.Message}");
            throw;
        }
    }
}