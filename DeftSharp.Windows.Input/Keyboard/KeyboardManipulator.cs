using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardManipulator : IKeyboardManipulator
{
    private readonly KeyboardManipulatorInterceptor _manipulator;
    private readonly KeyboardIntervalInterceptor _intervalInterceptor;
    public IEnumerable<Key> LockedKeys => _manipulator.LockedKeys;

    public event Action<KeyPressedArgs>? KeyPrevented;

    public KeyboardManipulator()
    {
        _manipulator = KeyboardManipulatorInterceptor.Instance;
        _intervalInterceptor = KeyboardIntervalInterceptor.Instance;
        _manipulator.KeyPrevented += OnInterceptorKeyPrevented;
    }

    public bool IsKeyLocked(Key key) => _manipulator.IsKeyLocked(key);

    public void Press(Key key) => _manipulator.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => _manipulator.PressCombination(combination);

    public void SetInterval(Key key, TimeSpan interval) => _intervalInterceptor.SetInterval(key, interval);

    public void SetInterval(IEnumerable<Key> keys, TimeSpan interval)
    {
        foreach (var key in keys)
            SetInterval(key, interval);
    }

    public void Prevent(Key key, Func<bool>? predicate = null)
    {
        predicate ??= () => true;

        _manipulator.Prevent(key, predicate);
    }

    public void Prevent(IEnumerable<Key> keys, Func<bool>? predicate = null)
    {
        foreach (var key in keys)
            Prevent(key, predicate);
    }

    public void Release(Key key) => _manipulator.Release(key);

    public void ReleaseAll() => _manipulator.ReleaseAll();

    public void Dispose() => _manipulator.KeyPrevented -= OnInterceptorKeyPrevented;

    private void OnInterceptorKeyPrevented(KeyPressedArgs args) => KeyPrevented?.Invoke(args);
}