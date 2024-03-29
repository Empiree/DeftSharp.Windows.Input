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
    void Press(IEnumerable<Key> keys);
    void Prevent(Key key, Func<bool>? predicate = null);
    void Prevent(IEnumerable<Key> keys, Func<bool>? predicate = null);
    void SetInterval(Key key, TimeSpan interval);
    void SetInterval(IEnumerable<Key> keys, TimeSpan interval);
    void ResetIntervals();
    void ResetInterval(Key key);
    void ResetInterval(IEnumerable<Key> keys);
    void Release(Key key);
    void Release(IEnumerable<Key> keys);
    void ReleaseAll();
}