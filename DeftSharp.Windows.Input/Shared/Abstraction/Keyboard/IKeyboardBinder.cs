using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardBinder : IDisposable
{
    IReadOnlyDictionary<Key, Key> BoundedKeys { get; }

    void Bind(Key oldKey, Key newKey);
    void Unbind(Key key);
    void UnbindAll();
}