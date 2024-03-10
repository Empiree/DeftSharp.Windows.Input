using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardManipulator : IDisposable
{
    IEnumerable<Key> LockedKeys { get; }

    bool IsKeyLocked(Key key);
    
    void Press(Key key);
    void Prevent(Key key);
    void Release(Key key);
    void ReleaseAll();
    
    event Action<KeyPressedArgs>? KeyPrevented;
}