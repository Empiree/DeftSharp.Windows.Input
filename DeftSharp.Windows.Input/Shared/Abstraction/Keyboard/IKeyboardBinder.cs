using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardBinder
{
    IReadOnlyDictionary<Key, Key> BoundedKeys { get; }

    Key GetBoundKey(Key key);
    bool IsKeyBounded(Key key);
    void Bind(Key oldKey, Key newKey);
    void Bind(IEnumerable<Key> keys, Key newKey);
    void Swap(Key first, Key second);
    void Unbind(Key key);
    void Unbind(IEnumerable<Key> keys);
    void UnbindAll();
}