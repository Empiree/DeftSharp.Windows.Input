using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardBinder : IDisposable
{
    private readonly IKeyboardBinderInterceptor _keyboardBinderInterceptor;

    public IReadOnlyDictionary<Key, Key> BoundedKeys => _keyboardBinderInterceptor.BoundedKeys;
    
    public KeyboardBinder()
    {
        _keyboardBinderInterceptor = KeyboardBinderInterceptor.Instance;
    }

    ~KeyboardBinder()
    {
        Dispose();
    }

    public void Bind(Key oldKey, Key newKey) => _keyboardBinderInterceptor.Bind(oldKey, newKey);
    public void Unbind(Key key) => _keyboardBinderInterceptor.Unbind(key);
    public void UnbindAll() => _keyboardBinderInterceptor.UnbindAll();
    
    public void Dispose()
    {
      
    }
}