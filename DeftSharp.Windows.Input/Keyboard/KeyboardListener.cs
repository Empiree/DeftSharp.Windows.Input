using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardListener : IDisposable
{
    private readonly IKeyboardListenerInterceptor _keyboardInterceptor;
    public bool IsListening => _keyboardInterceptor.Subscriptions.Any();
    public IEnumerable<KeyboardSubscription> Subscriptions => _keyboardInterceptor.Subscriptions;

    public KeyboardListener()
    {
        _keyboardInterceptor = new KeyboardListenerInterceptor();
    }

    ~KeyboardListener()
    {
        Dispose();
    }

    public void Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        _keyboardInterceptor.Subscribe(key, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);

    public void Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        _keyboardInterceptor.Subscribe(keys, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);

    public void SubscribeOnce(Key key, Action<Key> onClick, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        _keyboardInterceptor.SubscribeOnce(key, onClick, keyboardEvent);

    public void Unsubscribe(Key key) => _keyboardInterceptor.Unsubscribe(key);

    public void Unsubscribe(IEnumerable<Key> keys) => _keyboardInterceptor.Unsubscribe(keys);

    public void Unsubscribe(Guid id) => _keyboardInterceptor.Unsubscribe(id);

    public void UnsubscribeAll() => _keyboardInterceptor.UnsubscribeAll();

    public void Dispose() => _keyboardInterceptor.Dispose();
}