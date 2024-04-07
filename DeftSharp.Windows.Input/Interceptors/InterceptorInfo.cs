using System;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Represents information about an interceptor.
/// </summary>
public sealed class InterceptorInfo
{
    /// <summary>
    /// Gets the unique identifier of the interceptor.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the type of the interceptor.
    /// </summary>
    public InterceptorType Type { get; }

    /// <summary>
    /// Gets the name of the interceptor.
    /// </summary>
    public string Name { get; }

    public InterceptorInfo(string name, InterceptorType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }
}