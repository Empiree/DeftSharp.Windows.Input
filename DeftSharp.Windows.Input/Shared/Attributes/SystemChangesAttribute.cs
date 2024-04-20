using System;

namespace DeftSharp.Windows.Input.Shared.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class SystemChangesAttribute : Attribute
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Description { get; }

    public SystemChangesAttribute() { }
    public SystemChangesAttribute(string description) => Description = description;
}
