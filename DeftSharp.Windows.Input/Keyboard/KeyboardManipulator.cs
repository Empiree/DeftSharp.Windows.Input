using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to control the keyboard.
/// </summary>
public sealed class KeyboardManipulator : IKeyboardManipulator
{
    private readonly KeyboardManipulatorInterceptor _manipulator;
    private readonly KeyboardIntervalInterceptor _intervalInterceptor;

    /// <summary>
    /// Gets the keys that are currently locked.
    /// </summary>
    public IEnumerable<Key> LockedKeys => _manipulator.LockedKeys;

    /// <summary>
    /// Event triggered when a key press event is prevented.
    /// </summary>
    public event Action<KeyPressedArgs>? KeyPrevented;

    public KeyboardManipulator()
    {
        _manipulator = KeyboardManipulatorInterceptor.Instance;
        _intervalInterceptor = KeyboardIntervalInterceptor.Instance;
        _manipulator.KeyPrevented += OnInterceptorKeyPrevented;
    }

    /// <summary>
    /// Checks if the key is currently locked.
    /// </summary>
    public bool IsKeyLocked(Key key) => _manipulator.IsKeyLocked(key);

    /// <summary>
    /// Presses the key.
    /// </summary>
    public void Press(Key key) => _manipulator.Press(key);

    /// <summary>
    /// Presses the combination of keys.
    /// </summary>
    public void Press(params Key[] keys) => _manipulator.Press(keys);

    /// <summary>
    /// Simulates a keyboard event for the specified key.
    /// </summary>
    public void Simulate(Key key, KeyboardSimulateOption option) => _manipulator.Simulate(key, option);

    /// <summary>
    /// Simulates a keyboard event for the specified keys.
    /// </summary>
    public void Simulate(IEnumerable<Key> keys, KeyboardSimulateOption option)
    {
        foreach (var key in keys.Distinct())
            Simulate(key, option);
    }

    /// <summary>
    /// Resets the press interval for the key.
    /// </summary>
    public void ResetInterval(Key key) => SetInterval(key, TimeSpan.Zero);

    /// <summary>
    /// Resets the press interval for the keys.
    /// </summary>
    public void ResetInterval(IEnumerable<Key> keys) => SetInterval(keys, TimeSpan.Zero);

    /// <summary>
    /// Resets all press intervals.
    /// </summary>
    public void ResetInterval() => _intervalInterceptor.ResetInterval();

    /// <summary>
    /// Sets the press interval for the key.
    /// </summary>
    public void SetInterval(Key key, TimeSpan interval) => _intervalInterceptor.SetInterval(key, interval);

    /// <summary>
    /// Sets the press interval for the keys.
    /// </summary>
    public void SetInterval(IEnumerable<Key> keys, TimeSpan interval)
    {
        foreach (var key in keys.Distinct())
            SetInterval(key, interval);
    }

    /// <summary>
    /// Prevents the specified key from being pressed.
    /// </summary>
    public void Prevent(Key key, Func<bool>? predicate = null)
    {
        predicate ??= () => true;

        _manipulator.Prevent(key, predicate);
    }

    /// <summary>
    /// Prevents the specified keys from being pressed.
    /// </summary>
    public void Prevent(IEnumerable<Key> keys, Func<bool>? predicate = null)
    {
        foreach (var key in keys.Distinct())
            Prevent(key, predicate);
    }

    /// <summary>
    /// Releases the specified key.
    /// </summary>
    public void Release(Key key) => _manipulator.Release(key);

    /// <summary>
    /// Releases the specified keys.
    /// </summary>
    public void Release(IEnumerable<Key> keys)
    {
        foreach (var key in keys.Distinct())
            Release(key);
    }

    /// <summary>
    /// Releases all currently pressed keys.
    /// </summary>
    public void Release() => _manipulator.Release();

    /// <summary>
    /// Disposes of all resources used by the keyboard manipulator.
    /// </summary>
    public void Dispose() => _manipulator.KeyPrevented -= OnInterceptorKeyPrevented;

    private void OnInterceptorKeyPrevented(KeyPressedArgs args) => KeyPrevented?.Invoke(args);
}