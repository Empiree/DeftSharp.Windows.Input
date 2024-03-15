using System;
using System.Collections.Generic;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;

public interface IKeyboardSequenceListener : IDisposable
{
    IEnumerable<KeyboardSequenceSubscription> Subscriptions { get; }
    
    KeyboardSequenceSubscription Subscribe(IEnumerable<Key> sequence, Action onClick, TimeSpan interval);
    KeyboardSequenceSubscription SubscribeOnce(IEnumerable<Key> sequence, Action onClick);
    void UnsubscribeAll();
    void Unsubscribe(Guid id);
}