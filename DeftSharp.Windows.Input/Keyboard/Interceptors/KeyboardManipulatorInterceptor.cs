using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardManipulatorInterceptor : IKeyboardManipulatorInterceptor
{
    #region Singleton

    private static readonly Lazy<KeyboardManipulatorInterceptor> LazyInstance =
        new(() => new KeyboardManipulatorInterceptor());

    public static KeyboardManipulatorInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly IKeyboardInterceptor _keyboardInterceptor;
    private readonly HashSet<Key> _lockedKeys;
    private readonly object _lock = new();

    public event Func<bool>? UnhookRequested;
    public event Action<Key>? KeyPrevented;
    public event Action<Key>? KeyReleased;

    public IEnumerable<Key> LockedKeys => _lockedKeys;

    private KeyboardManipulatorInterceptor()
    {
        _lockedKeys = new HashSet<Key>();
        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.InterceptorRequest += OnInterceptorRequest;
        _keyboardInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
    }

    ~KeyboardManipulatorInterceptor()
    {
        Dispose();
    }

    public void Press(Key key) => _keyboardInterceptor.Press(key);

    public void Prevent(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.Any(k => k == key))
                return;

            Hook();
            _lockedKeys.Add(key);
            KeyPrevented?.Invoke(key);
        }
    }

    public void Release(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.All(k => k != key))
                return;

            _lockedKeys.Remove(key);
            KeyReleased?.Invoke(key);

            if (!_lockedKeys.Any())
                Unhook();
        }
    }

    public void ReleaseAll()
    {
        var keys = _lockedKeys.ToArray();

        foreach (var key in keys)
            Release(key);
    }

    public void Hook() => _keyboardInterceptor.Hook();
    public void Unhook() => _keyboardInterceptor.Unhook();
    public bool IsKeyLocked(Key key) => _lockedKeys.Any(k => k == key);

    public void Dispose()
    {
        ReleaseAll();
        _keyboardInterceptor.InterceptorRequest -= OnInterceptorRequest;
        _keyboardInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    private bool OnInterceptorUnhookRequested() => (UnhookRequested?.Invoke() ?? true) && !_lockedKeys.Any();

    private InterceptorResponse OnInterceptorRequest(KeyPressedArgs args) =>
        new(!IsKeyLocked(args.KeyPressed));
}