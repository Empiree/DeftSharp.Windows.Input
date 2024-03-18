using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

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
    }

    public KeyCombinationSubscription(
        IEnumerable<Key> combination,
        Action onClick, 
        TimeSpan intervalOfClick) 
        : base(onClick, intervalOfClick)
    {
        Combination = combination.Distinct();
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