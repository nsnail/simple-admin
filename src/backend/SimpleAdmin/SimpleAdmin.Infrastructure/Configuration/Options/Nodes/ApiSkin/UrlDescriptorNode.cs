namespace SimpleAdmin.Infrastructure.Configuration.Options.Nodes.ApiSkin;

/// <summary>
///     url描述符 配置节点
/// </summary>
public record UrlDescriptorNode
{
    /// <summary>
    ///     名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     url
    /// </summary>
    public string Url { get; set; }
}



