using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IKeyboardInterceptor: IRequestedInterceptor
{
    event KeyboardInputDelegate? KeyboardInputMiddleware;
    void Press(Key key);
}