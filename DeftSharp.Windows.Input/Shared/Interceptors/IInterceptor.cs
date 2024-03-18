using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IInterceptor : IDisposable
{
    event Func<bool>? UnhookRequested;
    void Hook();
    void Unhook();
}