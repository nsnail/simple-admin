using Mapster;
using SimpleAdmin.WebApi.DataContracts.DbMaps;

namespace SimpleAdmin.WebApi.DataContracts.Dto.User;

public record GetProfileRsp : DataContract
{
    public List<MenuInfo> Menu        { get; set; }
    public List<string>   Permissions { get; set; }


    public record MenuInfo : IRegister
    {
        /// <inheritdoc />
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<TbSysMenu, MenuInfo>()
                  .Map(dest => dest.Meta, src => src.Adapt<MetaInfo>())
                  .Map(dest => dest.Name, src => src.Code);
        }

        /// <summary>
        ///     子节点
        /// </summary>
        public List<MenuInfo> Children { get; set; }

        /// <summary>
        ///     组件
        /// </summary>
        public string Component { get; set; }


        /// <summary>
        ///     元数据
        /// </summary>
        public MetaInfo Meta { get; set; }


        /// <summary>
        ///     权限编码
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        ///     菜单访问地址
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        ///     权限类型
        /// </summary>
        public Enums.PermissionTypes Type { get; set; }

        public record MetaInfo : IRegister
        {
            /// <inheritdoc />
            public void Register(TypeAdapterConfig config)
            {
                config.ForType<TbSysMenu, MetaInfo>().Map(dest => dest.Type, src => src.Type.ToString().ToLower());
            }

            public string Icon  { get; set; }
            public string Title { get; set; }
            public string Type  { get; set; }
        }
    }
}