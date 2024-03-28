using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardManipulator : IDisposable
{
    IEnumerable<Key> LockedKeys { get; }

    event Action<KeyPressedArgs>? KeyPrevented;

    bool IsKeyLocked(Key key);

    void Press(Key key);
    void PressCombination(IEnumerable<Key> combination);
    void Prevent(Key key, Func<bool>? predicate = null);
    void Prevent(IEnumerable<Key> keys, Func<bool>? predicate = null);
    void SetInterval(Key key, TimeSpan interval);
    void SetInterval(IEnumerable<Key> keys, TimeSpan interval);
    void Release(Key key);
    void ReleaseAll();
}