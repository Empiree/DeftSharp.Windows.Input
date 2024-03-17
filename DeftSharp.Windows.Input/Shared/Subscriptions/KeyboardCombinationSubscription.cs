using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

public sealed class KeyboardCombinationSubscription : InputSubscription<Action>
{
    public IEnumerable<Key> Combination { get; }
    
    public KeyboardCombinationSubscription(
        IEnumerable<Key> combination,
        Action onClick, 
        bool singleUse = false) 
        : base(onClick, singleUse)
    {
        Combination = combination.Distinct();
    }

    public KeyboardCombinationSubscription(
        IEnumerable<Key> combination,
        Action onClick, 
        TimeSpan intervalOfClick) 
        : base(onClick, intervalOfClick)
    {
        Combination = combination.Distinct();
    }
}