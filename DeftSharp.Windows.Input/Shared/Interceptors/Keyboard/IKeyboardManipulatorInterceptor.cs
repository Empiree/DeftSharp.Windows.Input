using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

internal interface IKeyboardManipulatorInterceptor : IRequestedInterceptor
{
    IEnumerable<Key> LockedKeys { get; }

    void Press(Key key);
    void Prevent(Key key);
    void Release(Key key);
    void ReleaseAll();
    
    event Action<Key>? KeyPrevented;
    event Action<Key>? KeyReleased;
}