using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardBinder
{
    private readonly IKeyboardBinder _keyboardBinderInterceptor;

    public IReadOnlyDictionary<Key, Key> BoundedKeys => _keyboardBinderInterceptor.BoundedKeys;

    public KeyboardBinder()
    {
        _keyboardBinderInterceptor = KeyboardBinderInterceptor.Instance;
    }
    
    public bool IsKeyBounded(Key key) => _keyboardBinderInterceptor.IsKeyBounded(key);
    public void Bind(Key oldKey, Key newKey) => _keyboardBinderInterceptor.Bind(oldKey, newKey);
    public void Unbind(Key key) => _keyboardBinderInterceptor.Unbind(key);
    public void UnbindAll() => _keyboardBinderInterceptor.UnbindAll();
}