using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardManipulator : IDisposable
{
    private readonly IKeyboardManipulator _keyboardInterceptor;
    public IEnumerable<Key> LockedKeys => _keyboardInterceptor.LockedKeys;

    public event Action<Key>? KeyPrevented;
    public event Action<Key>? KeyReleased;

    public KeyboardManipulator()
    {
        _keyboardInterceptor = KeyboardManipulatorInterceptor.Instance;
        _keyboardInterceptor.KeyPrevented += OnInterceptorKeyPrevented;
        _keyboardInterceptor.KeyReleased += OnInterceptorKeyReleased;
    }

    ~KeyboardManipulator()
    {
        Dispose();
    }

    public bool IsKeyLocked(Key key) => _keyboardInterceptor.IsKeyLocked(key);

    public void Press(Key key) => _keyboardInterceptor.Press(key);

    public void Prevent(Key key) => _keyboardInterceptor.Prevent(key);

    public void Release(Key key) => _keyboardInterceptor.Release(key);

    public void ReleaseAll() => _keyboardInterceptor.ReleaseAll();
    
    public void Dispose()
    {
        _keyboardInterceptor.KeyPrevented -= OnInterceptorKeyPrevented;
        _keyboardInterceptor.KeyReleased -= OnInterceptorKeyReleased;
    }
    
    private void OnInterceptorKeyPrevented(Key key) => KeyPrevented?.Invoke(key);
    private void OnInterceptorKeyReleased(Key key) => KeyReleased?.Invoke(key);
}