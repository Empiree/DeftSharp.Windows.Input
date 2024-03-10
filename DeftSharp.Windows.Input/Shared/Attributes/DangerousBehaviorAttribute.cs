using System;

namespace DeftSharp.Windows.Input.Shared.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class DangerousBehaviorAttribute : Attribute
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public string? Description { get; }

    public DangerousBehaviorAttribute() { }
    public DangerousBehaviorAttribute(string description) => Description = description;
}
