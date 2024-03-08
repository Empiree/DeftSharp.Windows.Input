using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IMouseInterceptor : IRequestedInterceptor
{
    event Func<MouseInputArgs, InterceptorResponse>? InterceptorPipelineRequested;

    Coordinates GetPosition();
    void SetPosition(int x, int y);
    void Click(int x, int y, MouseButton button);
}