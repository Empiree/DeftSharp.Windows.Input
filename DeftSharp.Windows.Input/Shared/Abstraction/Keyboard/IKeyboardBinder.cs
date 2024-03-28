using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardBinder
{
    IReadOnlyDictionary<Key, Key> BoundedKeys { get; }

    bool IsKeyBounded(Key key);
    void Bind(Key oldKey, Key newKey);
    void Bind(IEnumerable<Key> keys, Key newKey);
    void Unbind(Key key);
    void UnbindAll();
}