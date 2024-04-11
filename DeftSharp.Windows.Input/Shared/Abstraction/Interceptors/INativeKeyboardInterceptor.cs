using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Keyboard;

internal interface INativeKeyboardInterceptor : IRequestedInterceptor
{
    event KeyboardInputDelegate? KeyboardInput;
    void Press(Key key);
    void Press(IEnumerable<Key> keys);
    void Simulate(Key key, KeyboardInputEvent keyboardEvent);
    bool IsKeyActive(Key key);
    bool IsKeyPressed(Key key);
    KeyboardLayout GetLayout();
    KeyboardType GetKeyboardType();
}