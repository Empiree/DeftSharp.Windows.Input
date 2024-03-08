using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

internal interface IKeyboardInterceptor: IRequestedInterceptor
{
    event Func<KeyPressedArgs, InterceptorResponse>? InterceptorRequest;
    void Press(Key key);
}