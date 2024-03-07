using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Mouse;

internal interface IMouseInterceptor : IRequestedInterceptor
{
    event Func<MouseInputArgs, PipelineInterceptorOperation>? MouseProcessing;

    Coordinates GetPosition();
    void SetPosition(int x, int y);
    void Click(int x, int y, MouseButton button);
}