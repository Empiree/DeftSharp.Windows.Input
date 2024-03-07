using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IInterceptor : IDisposable
{
    void Hook();
    void Unhook();
}