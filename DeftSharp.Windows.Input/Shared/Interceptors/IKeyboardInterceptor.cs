using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IKeyboardInterceptor: IInterceptor
{
    event KeyboardInputDelegate? KeyboardInput;
    void Press(Key key);
}