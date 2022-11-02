// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

using System;

namespace SimpleAdmin.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SnowflakeAttribute : Attribute
{
    public bool Enable { get; set; } = true;
}