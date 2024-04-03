using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

internal interface INativeKeyboardInterceptor : IRequestedInterceptor
{
    event KeyboardInputDelegate? KeyboardInput;
    void Press(Key key);
    void Press(IEnumerable<Key> keys);
    bool IsKeyActive(Key key);
    bool IsKeyPressed(Key key);
    KeyboardLayout GetLayout();
    KeyboardType GetKeyboardType();
}