using System;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Pipeline;

public sealed class InterceptorInfo
{
    public Guid Id { get;}
    public InterceptorType Type { get;}
    public string Name { get; }

    public InterceptorInfo(string name, InterceptorType type)
    {
        Id = Guid.NewGuid();
        Name = name;
        Type = type;
    }
}