using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

public interface IKeyboardSequenceListener : IDisposable
{
    IEnumerable<KeyboardSequenceSubscription> Subscriptions { get; }

    void Subscribe(KeyboardSequenceSubscription subscription);
    void UnsubscribeAll();
    void Unsubscribe(Guid id);
}