using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Subscriptions.Input;

namespace DeftSharp.Windows.Input.Keyboard;

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
        
        if (Sequence.Any(k => k is Key.None))
            throw new KeyNoneException();
    }

    internal KeySequenceSubscription(
        IEnumerable<Key> sequence,
        Action onClick,
        TimeSpan interval)
        : base(onClick, interval)
    {
        Sequence = sequence;
        
        if (Sequence.Any(k => k is Key.None))
            throw new KeyNoneException();
    }
    
    internal void Invoke()
    {
        if (!CanBeInvoked())
            return;

        LastInvoked = DateTime.Now;
        OnClick();
    }
}