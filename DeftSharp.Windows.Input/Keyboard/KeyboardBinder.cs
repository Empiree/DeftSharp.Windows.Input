using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the option to change the bind of the specified button.
/// </summary>
public sealed class KeyboardBinder : IKeyboardBinder
{
    private readonly KeyboardBinderInterceptor _keyboardBinder = KeyboardBinderInterceptor.Instance;

    /// <summary>
    /// Gets the dictionary of keys bound to other keys.
    /// </summary>
    public IReadOnlyDictionary<Key, Key> BoundedKeys => _keyboardBinder.BoundedKeys;

    /// <summary>
    /// Gets the key bound to the specified key.
    /// </summary>
    public Key GetBoundKey(Key key) => _keyboardBinder.GetBoundKey(key);

    /// <summary>
    /// Checks if the specified key is bound to another key.
    /// </summary>
    public bool IsKeyBounded(Key key) => _keyboardBinder.IsKeyBounded(key);

    /// <summary>
    /// Binds the specified key to the new key.
    /// </summary>
    public void Bind(Key oldKey, Key newKey) => _keyboardBinder.Bind(oldKey, newKey);

    /// <summary>
    /// Binds multiple keys to the new key.
    /// </summary>
    public void Bind(IEnumerable<Key> keys, Key newKey)
    {
        foreach (var oldKey in keys.Distinct())
            Bind(oldKey, newKey);
    }

    /// <summary>
    /// Swaps the bindings of two keys.
    /// </summary>
    public void Swap(Key first, Key second)
    {
        Bind(first, second);
        Bind(second, first);
    }

    /// <summary>
    /// Unbinds the specified key.
    /// </summary>
    public void Unbind(Key key) => _keyboardBinder.Unbind(key);

    /// <summary>
    /// Unbinds multiple keys.
    /// </summary>
    public void Unbind(IEnumerable<Key> keys)
    {
        foreach (var key in keys.Distinct())
            Unbind(key);
    }

    /// <summary>
    /// Unbinds all keys.
    /// </summary>
    public void Unbind() => _keyboardBinder.Unbind();
}