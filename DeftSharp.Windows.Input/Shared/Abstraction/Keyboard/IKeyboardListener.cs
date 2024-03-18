using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

internal interface IKeyboardListener : IDisposable
{
    IEnumerable<KeySubscription> Subscriptions { get; }
    
    void Subscribe(KeySubscription subscription);

    void Unsubscribe(Key key);
    void Unsubscribe(Guid id);
    void UnsubscribeAll();
}