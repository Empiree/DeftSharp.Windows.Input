using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;

internal interface IKeyboardListenerInterceptor : IRequestedInterceptor
{
    IEnumerable<KeyboardSubscription> Subscriptions { get; }

    void Subscribe(Key key, Action<Key> onClick, TimeSpan intervalOfClick, KeyboardEvent keyboardEvent);
    void Subscribe(IEnumerable<Key> keys, Action<Key> onClick, TimeSpan intervalOfClick, KeyboardEvent keyboardEvent);
    void SubscribeOnce(Key key, Action<Key> onClick, KeyboardEvent keyboardEvent);

    void Unsubscribe(Key key);
    void Unsubscribe(IEnumerable<Key> keys);
    void Unsubscribe(Guid id);
    void UnsubscribeAll();
}