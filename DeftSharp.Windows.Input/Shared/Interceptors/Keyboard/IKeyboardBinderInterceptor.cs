using System.Collections.Concurrent;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

internal interface IKeyboardBinderInterceptor : IRequestedInterceptor
{
    ConcurrentDictionary<Key, Key> BoundedKeys { get; }

    void Bind(Key oldKey, Key newKey);
    void Unbind(Key key);
    void UnbindAll();
}