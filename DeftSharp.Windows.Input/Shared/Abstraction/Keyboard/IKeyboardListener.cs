using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardListener : IDisposable
{
    IEnumerable<KeyboardSubscription> Subscriptions { get; }
    
    void Subscribe(KeyboardSubscription subscription);

    void Unsubscribe(Key key);
    void Unsubscribe(IEnumerable<Key> keys);
    void Unsubscribe(Guid id);
    void UnsubscribeAll();
}