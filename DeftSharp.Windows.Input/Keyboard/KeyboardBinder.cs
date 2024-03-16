using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardBinder
{
    private readonly IKeyboardBinder _keyboardBinder = KeyboardBinderInterceptor.Instance;
    public IReadOnlyDictionary<Key, Key> BoundedKeys => _keyboardBinder.BoundedKeys;

    public bool IsKeyBounded(Key key) => _keyboardBinder.IsKeyBounded(key);
    public void Bind(Key oldKey, Key newKey) => _keyboardBinder.Bind(oldKey, newKey);

    public void BindMany(IEnumerable<Key> keys, Key newKey)
    {
        foreach (var oldKey in keys)
            Bind(oldKey, newKey);
    }
    public void Unbind(Key key) => _keyboardBinder.Unbind(key);
    public void UnbindAll() => _keyboardBinder.UnbindAll();
}