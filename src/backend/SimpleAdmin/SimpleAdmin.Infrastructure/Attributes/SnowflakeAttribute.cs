// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace SimpleAdmin.Infrastructure.Attributes;

/// <summary>
/// 标记一个属性是否启用雪花id生成
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class SnowflakeAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}