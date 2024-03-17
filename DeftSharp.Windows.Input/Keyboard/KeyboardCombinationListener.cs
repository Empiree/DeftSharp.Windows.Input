using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardCombinationListener : IDisposable
{
    private readonly IKeyboardCombinationListener _listener = new KeyboardCombinationListenerInterceptor();
    public IEnumerable<KeyboardCombinationSubscription> Subscriptions => _listener.Subscriptions;

    public bool IsListening => Subscriptions.Any();

    ~KeyboardCombinationListener() => Dispose();
    
    public KeyboardCombinationSubscription Subscribe(IEnumerable<Key> combination, Action onClick,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new KeyboardCombinationSubscription(combination, onClick, intervalOfClick ?? TimeSpan.Zero);
        _listener.Subscribe(subscription);
        return subscription;
    }

    public KeyboardCombinationSubscription SubscribeOnce(IEnumerable<Key> combination, Action onClick)
    {
        var subscription = new KeyboardCombinationSubscription(combination, onClick, true);
        _listener.Subscribe(subscription);
        return subscription;
    } 
    
    public void UnsubscribeAll() => _listener.UnsubscribeAll();

    public void Unsubscribe(Guid id) => _listener.Unsubscribe(id);

    public void Dispose() => _listener.Dispose();
}