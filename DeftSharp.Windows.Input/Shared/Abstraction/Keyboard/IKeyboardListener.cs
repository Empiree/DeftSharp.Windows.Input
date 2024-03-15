using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardListener : IDisposable
{
    IEnumerable<KeyboardSubscription> Subscriptions { get; }

    KeyboardSubscription Subscribe(Key key, Action<Key> onClick, TimeSpan intervalOfClick, KeyboardEvent keyboardEvent);
    IEnumerable<KeyboardSubscription> Subscribe(IEnumerable<Key> keys, Action<Key> onClick, TimeSpan intervalOfClick, KeyboardEvent keyboardEvent);
    KeyboardSubscription SubscribeOnce(Key key, Action<Key> onClick, KeyboardEvent keyboardEvent);

    void Unsubscribe(Key key);
    void Unsubscribe(IEnumerable<Key> keys);
    void Unsubscribe(Guid id);
    void UnsubscribeAll();
}