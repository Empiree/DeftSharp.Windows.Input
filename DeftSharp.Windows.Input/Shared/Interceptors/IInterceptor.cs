using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IInterceptor : IDisposable
{
    event Func<bool>? UnhookRequested;
    void Hook();
    void Unhook();
}