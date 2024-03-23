using System;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

public interface IInterceptor : IDisposable
{
    void Hook();
    void Unhook();
}