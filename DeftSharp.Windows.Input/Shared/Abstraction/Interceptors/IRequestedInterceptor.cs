using System;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

public interface IRequestedInterceptor : IInterceptor
{
    event Func<bool>? UnhookRequested;
}