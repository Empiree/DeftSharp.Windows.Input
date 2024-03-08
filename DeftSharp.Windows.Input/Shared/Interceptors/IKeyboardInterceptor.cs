using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IKeyboardInterceptor: IRequestedInterceptor
{
    event Func<KeyPressedArgs, InterceptorResponse>? InterceptorPipelineRequested;
    void Press(Key key);
}