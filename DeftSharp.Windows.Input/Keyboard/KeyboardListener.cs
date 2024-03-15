using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardListener : IDisposable
{
    private readonly IKeyboardListener _keyboardInterceptor = new KeyboardListenerInterceptor();
    public bool IsListening => _keyboardInterceptor.Subscriptions.Any();
    public IEnumerable<KeyboardSubscription> Subscriptions => _keyboardInterceptor.Subscriptions;
    
    ~KeyboardListener() => Dispose();

    public KeyboardSubscription Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) 
    {
        var subscription = new KeyboardSubscription(key, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);
        _keyboardInterceptor.Subscribe(subscription);
        return subscription;
    }

    public IEnumerable<KeyboardSubscription> Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => Subscribe(k, onClick, intervalOfClick, keyboardEvent)).ToList();

    public KeyboardSubscription SubscribeOnce(Key key, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeyboardSubscription(key, onClick, keyboardEvent, true);
        _keyboardInterceptor.Subscribe(subscription);
        return subscription;
    }

    public void Unsubscribe(Key key) => _keyboardInterceptor.Unsubscribe(key);

    public void Unsubscribe(IEnumerable<Key> keys) => _keyboardInterceptor.Unsubscribe(keys);

    public void Unsubscribe(Guid id) => _keyboardInterceptor.Unsubscribe(id);

    public void UnsubscribeAll() => _keyboardInterceptor.UnsubscribeAll();

    public void Dispose() => _keyboardInterceptor.Dispose();
}