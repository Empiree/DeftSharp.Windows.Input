using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardManipulator : IDisposable
{
    private readonly IKeyboardInterceptor _keyboardInterceptor;
    public IEnumerable<Key> LockedKeys => _keyboardInterceptor.LockedKeys;

    public event Action<Key>? KeyPrevented;
    public event Action<Key>? KeyReleased;

    public KeyboardManipulator()
    {
        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
        _keyboardInterceptor.KeyPrevented += OnInterceptorKeyPrevented;
        _keyboardInterceptor.KeyReleased += OnInterceptorKeyReleased;
    }

    public void Press(Key key) => KeyboardAPI.PressButton(key);

    public void Prevent(Key key)
    {
        _keyboardInterceptor.Hook();
        _keyboardInterceptor.Prevent(key);
    }

    public void Release(Key key)
    {
        _keyboardInterceptor.Release(key);
        if (!LockedKeys.Any())
            _keyboardInterceptor.Unhook();
    }

    public void ReleaseAll()
    {
        _keyboardInterceptor.ReleaseAll();
        _keyboardInterceptor.Unhook();
    }

    public void Dispose()
    {
        ReleaseAll();
        _keyboardInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
        _keyboardInterceptor.KeyPrevented -= OnInterceptorKeyPrevented;
        _keyboardInterceptor.KeyReleased -= OnInterceptorKeyReleased;
    }

    private bool OnInterceptorUnhookRequested() => !LockedKeys.Any();
    private void OnInterceptorKeyPrevented(Key key) => KeyPrevented?.Invoke(key);
    private void OnInterceptorKeyReleased(Key key) => KeyReleased?.Invoke(key);
}