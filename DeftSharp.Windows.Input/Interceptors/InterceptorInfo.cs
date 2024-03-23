using System;

namespace DeftSharp.Windows.Input.Interceptors;

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