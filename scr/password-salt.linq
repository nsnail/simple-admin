<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Globalization</Namespace>
</Query>

public static class c
{
	/// <summary>
	///     对一个字符串进行md5hash运算
	/// </summary>
	/// <param name="me">字符串</param>
	/// <param name="e">字符串使用的编码</param>
	/// <returns>hash摘要的16进制文本形式（无连字符小写）</returns>
	public static string Md5(this string me, Encoding e)
	{
		using var md5 = MD5.Create();
		return BitConverter.ToString(md5.ComputeHash(e.GetBytes(me)))
						   .Replace("-", string.Empty)
						   .ToLower(CultureInfo.CurrentCulture);
	}

	/// <summary>
	///     将字符串转为guid
	/// </summary>
	/// <param name="me">字符串</param>
	/// <returns></returns>
	public static Guid Guid(this string me)
	{
		return System.Guid.Parse(me);
	}

	/// <summary>
	///     字符串转密码
	/// </summary>
	/// <param name="me"></param>
	/// <param name="saltCode">加盐</param>
	/// <returns></returns>
	public static Guid Password(this string me, string saltCode)
	{
		return (me.Md5(Encoding.UTF8) + saltCode).Md5(Encoding.UTF8).Guid();
	}


}


void Main()
{
	
	"admin".Password("b950830a-ce50-4df0-a1ad-09f101f0ae2e").Dump();
}

// You can define other methods, fields, classes and namespaces here