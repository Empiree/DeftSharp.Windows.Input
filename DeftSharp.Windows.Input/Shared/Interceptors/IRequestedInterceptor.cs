using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IRequestedInterceptor : IInterceptor
{
   event Func<bool>? UnhookRequested;
}