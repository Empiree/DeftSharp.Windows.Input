using System;
using System.Collections.Generic;
using System.Linq;
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
    public void Press(IEnumerable<Key> keys) => _manipulator.PressCombination(keys);

    public void ResetInterval(Key key) => SetInterval(key, TimeSpan.Zero);
    public void ResetInterval(IEnumerable<Key> keys) => SetInterval(keys, TimeSpan.Zero);

    public void ResetIntervals() => _intervalInterceptor.ResetIntervals();

    public void SetInterval(Key key, TimeSpan interval) => _intervalInterceptor.SetInterval(key, interval);

    public void SetInterval(IEnumerable<Key> keys, TimeSpan interval)
    {
        foreach (var key in keys.Distinct())
            SetInterval(key, interval);
    }

    public void Prevent(Key key, Func<bool>? predicate = null)
    {
        predicate ??= () => true;

        _manipulator.Prevent(key, predicate);
    }

    public void Prevent(IEnumerable<Key> keys, Func<bool>? predicate = null)
    {
        foreach (var key in keys.Distinct())
            Prevent(key, predicate);
    }

    public void Release(Key key) => _manipulator.Release(key);

    public void Release(IEnumerable<Key> keys)
    {
        foreach (var key in keys.Distinct())
            Release(key);
    }

    public void ReleaseAll() => _manipulator.ReleaseAll();

    public void Dispose() => _manipulator.KeyPrevented -= OnInterceptorKeyPrevented;

    private void OnInterceptorKeyPrevented(KeyPressedArgs args) => KeyPrevented?.Invoke(args);
}