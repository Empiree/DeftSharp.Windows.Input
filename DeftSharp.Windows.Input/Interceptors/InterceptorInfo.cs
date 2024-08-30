using System;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Represents information about an interceptor.
/// </summary>
public sealed class InterceptorInfo(string name, InterceptorType type)
{
    /// <summary>
    /// Gets the unique identifier of the interceptor.
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets the type of the interceptor.
    /// </summary>
    public InterceptorType Type { get; } = type;

    /// <summary>
    /// Gets the name of the interceptor.
    /// </summary>
    public string Name { get; } = name;
}