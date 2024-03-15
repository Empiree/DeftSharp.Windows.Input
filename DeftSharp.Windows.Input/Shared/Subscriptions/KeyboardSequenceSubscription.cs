using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

public sealed class KeyboardSequenceSubscription: InputSubscription<Action>
{
    public IEnumerable<Key> Sequence { get; }
    
    internal KeyboardSequenceSubscription(
        IEnumerable<Key> sequence,
        Action onClick,
        bool singleUse = false)
        : base(onClick, singleUse)
    {
        Sequence = sequence;
    }

    internal KeyboardSequenceSubscription(
        IEnumerable<Key> sequence,
        Action onClick,
        TimeSpan interval)
        : base(onClick, interval)
    {
        Sequence = sequence;
    }
    
    internal void Invoke()
    {
        if (LastInvoked.HasValue && SingleUse)
            return;

        if (LastInvoked?.Add(IntervalOfClick) >= DateTime.Now)
            return;

        LastInvoked = DateTime.Now;
        OnClick();
    }
}