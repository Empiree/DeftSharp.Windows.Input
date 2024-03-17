using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

public interface IKeyboardCombinationListener : IDisposable
{
    IEnumerable<KeyboardCombinationSubscription> Subscriptions { get; }

    void Subscribe(KeyboardCombinationSubscription subscription);
    void UnsubscribeAll();
    void Unsubscribe(Guid id);
}