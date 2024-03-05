using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IMouseInterceptor : IRequestedInterceptor
{
    Coordinates GetPosition();

    event EventHandler<MouseInputArgs>? MouseInput;
}