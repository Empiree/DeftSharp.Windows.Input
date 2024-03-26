using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardManipulator : IKeyboardManipulator
{
    private readonly KeyboardManipulatorInterceptor _keyboardInterceptor;
    public IReadOnlyDictionary<Key, Func<bool>> LockedKeys => _keyboardInterceptor.LockedKeys;

    public event Action<KeyPressedArgs>? KeyPrevented;

    public KeyboardManipulator()
    {
        _keyboardInterceptor = KeyboardManipulatorInterceptor.Instance;
        _keyboardInterceptor.KeyPrevented += OnInterceptorKeyPrevented;
    }

    public bool IsKeyLocked(Key key) => _keyboardInterceptor.IsKeyLocked(key);

    public void Press(Key key) => _keyboardInterceptor.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => _keyboardInterceptor.PressCombination(combination);

    public void Prevent(Key key, Func<bool>? predicate = null)
    {
        predicate ??= () => true;
        
        _keyboardInterceptor.Prevent(key, predicate);
    }

    public void PreventMany(IEnumerable<Key> keys, Func<bool>? predicate = null)
    {
        foreach (var key in keys)
            Prevent(key, predicate);
    }

    public void Release(Key key) => _keyboardInterceptor.Release(key);

    public void ReleaseAll() => _keyboardInterceptor.ReleaseAll();
    
    public void Dispose() => _keyboardInterceptor.KeyPrevented -= OnInterceptorKeyPrevented;

    private void OnInterceptorKeyPrevented(KeyPressedArgs args) => KeyPrevented?.Invoke(args);
}