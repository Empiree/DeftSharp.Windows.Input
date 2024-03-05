using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IRequestedInterceptor : IInterceptor
{
    event Func<bool>? UnhookRequested;
}