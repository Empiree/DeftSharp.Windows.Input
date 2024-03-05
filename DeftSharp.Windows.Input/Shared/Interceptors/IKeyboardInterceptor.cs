using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IKeyboardInterceptor : IRequestedInterceptor
{
    IEnumerable<Key> LockedKeys { get; }

    void Prevent(Key key);
    void Release(Key key);
    void ReleaseAll();

    event Action<Key>? KeyPrevented;
    event Action<Key>? KeyReleased;
    event EventHandler<KeyPressedArgs>? KeyPressed;
}