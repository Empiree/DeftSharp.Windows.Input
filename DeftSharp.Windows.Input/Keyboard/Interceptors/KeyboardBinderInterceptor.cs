using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal class KeyboardBinderInterceptor : IKeyboardBinderInterceptor
{
    #region Singleton

    private static readonly Lazy<KeyboardBinderInterceptor> LazyInstance =
        new(() => new KeyboardBinderInterceptor());

    public static KeyboardBinderInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly IKeyboardInterceptor _keyboardInterceptor;
    private readonly ConcurrentDictionary<Key, Key> _boundedKeys;

    public ConcurrentDictionary<Key, Key> BoundedKeys => _boundedKeys;
    
    public event Func<bool>? UnhookRequested;

    private KeyboardBinderInterceptor()
    {
        _boundedKeys = new ConcurrentDictionary<Key, Key>();
        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.InterceptorRequest += OnInterceptorRequest;
        _keyboardInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
        
    }

    ~KeyboardBinderInterceptor()
    {
        Dispose();
    }
    
    public void Bind(Key oldKey, Key newKey)
    {
        if (_boundedKeys.ContainsKey(oldKey))
            _boundedKeys[oldKey] = newKey;

        Hook();
        var isAdded = _boundedKeys.TryAdd(oldKey, newKey);
    }

    public void Unbind(Key key)
    {
        if (!_boundedKeys.ContainsKey(key))
            return;

        var boundedKey = _boundedKeys.FirstOrDefault();
        
        _boundedKeys.TryRemove(boundedKey);
        
        if(!_boundedKeys.Any())
            Unhook();
    }

    public void UnbindAll()
    {
        var boundedKeys = _boundedKeys.Keys.ToArray();
        foreach (var boundedKey in boundedKeys)
            Unbind(boundedKey);
    }

    public void Dispose()
    {
        UnbindAll();
        _keyboardInterceptor.InterceptorRequest -= OnInterceptorRequest;
        _keyboardInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    public void Hook() => _keyboardInterceptor.Hook();

    public void Unhook() => _keyboardInterceptor.Unhook();
    
    private bool OnInterceptorUnhookRequested() => (UnhookRequested?.Invoke() ?? true) && !_boundedKeys.Any();

    private InterceptorResponse OnInterceptorRequest(KeyPressedArgs args)
    {
        return new InterceptorResponse(
            !IsKeyBounded(args.KeyPressed),
            null, () =>
            {
                if (args.Event == KeyboardEvent.KeyUp)
                    return;
                
                var key = _boundedKeys[args.KeyPressed];
                KeyboardAPI.PressButton(key);
            });
    }

    public bool IsKeyBounded(Key key) => _boundedKeys.ContainsKey(key);
}