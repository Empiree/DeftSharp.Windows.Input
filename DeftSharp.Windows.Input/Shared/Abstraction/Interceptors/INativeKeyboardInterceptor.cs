using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

internal interface INativeKeyboardInterceptor : IRequestedInterceptor
{
    event KeyboardInputDelegate? KeyboardInput;
    void Press(Key key);
    void PressCombination(IEnumerable<Key> combination);
    bool IsKeyActive(Key key);
    bool IsKeyPressed(Key key);
}