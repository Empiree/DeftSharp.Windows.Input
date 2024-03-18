using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

public interface IKeyboardSequenceListener : IDisposable
{
    IEnumerable<KeySequenceSubscription> Subscriptions { get; }

    void Subscribe(KeySequenceSubscription subscription);
    void UnsubscribeAll();
    void Unsubscribe(Guid id);
}