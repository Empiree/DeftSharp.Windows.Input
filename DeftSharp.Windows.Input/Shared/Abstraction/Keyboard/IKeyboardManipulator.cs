using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

public interface IKeyboardManipulator : IDisposable
{
    IEnumerable<Key> LockedKeys { get; }
    
    event Action<KeyPressedArgs>? KeyPrevented;

    bool IsKeyLocked(Key key);
    
    void Press(Key key);
    void PressCombination(IEnumerable<Key> combination);
    void Prevent(Key key);
    void PreventMany(IEnumerable<Key> keys);
    void Release(Key key);
    void ReleaseAll();
    
}