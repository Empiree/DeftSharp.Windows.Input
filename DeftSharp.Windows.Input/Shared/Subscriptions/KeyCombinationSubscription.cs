using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Subscriptions.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyCombinationSubscription : InputSubscription<Action>
{
    public IEnumerable<Key> Combination { get; }
    
    public KeyCombinationSubscription(
        IEnumerable<Key> combination,
        Action onClick, 
        bool singleUse = false) 
        : base(onClick, singleUse)
    {
        Combination = combination.Distinct();
        
        if (Combination.Any(k => k is Key.None))
            throw new KeyNoneException();
    }

    public KeyCombinationSubscription(
        IEnumerable<Key> combination,
        Action onClick, 
        TimeSpan interval) 
        : base(onClick, interval)
    {
        Combination = combination.Distinct();
        
        if (Combination.Any(k => k is Key.None))
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