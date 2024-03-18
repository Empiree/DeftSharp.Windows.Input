using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

public sealed class KeySequenceSubscription: InputSubscription<Action>
{
    public IEnumerable<Key> Sequence { get; }
    
    internal KeySequenceSubscription(
        IEnumerable<Key> sequence,
        Action onClick,
        bool singleUse = false)
        : base(onClick, singleUse)
    {
        Sequence = sequence;
    }

    internal KeySequenceSubscription(
        IEnumerable<Key> sequence,
        Action onClick,
        TimeSpan interval)
        : base(onClick, interval)
    {
        Sequence = sequence;
    }
    
    internal void Invoke()
    {
        if (!CanBeInvoked())
            return;

        LastInvoked = DateTime.Now;
        OnClick();
    }
}