using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardBinder
{
    IReadOnlyDictionary<Key, Key> BoundedKeys { get; }

    bool IsKeyBounded(Key key);
    void Bind(Key oldKey, Key newKey);
    void BindMany(IEnumerable<Key> keys, Key newKey);
    void Unbind(Key key);
    void UnbindAll();
}