using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

public class KeyboardManipulator : IDisposable
{
    private readonly IKeyboardInterceptor _keyboardInterceptor;
    public IEnumerable<Key> LockedKeys => _keyboardInterceptor.LockedKeys;

    public KeyboardManipulator()
    {
        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
    }

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
    }

    private bool OnInterceptorUnhookRequested() => !LockedKeys.Any();
}