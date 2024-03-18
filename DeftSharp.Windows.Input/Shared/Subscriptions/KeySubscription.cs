using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

public sealed class KeySubscription : InputSubscription<Action<Key>>
{
    public Key Key { get; }
    public KeyboardEvent Event { get; }

    internal KeySubscription(
        Key key,
        Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown,
        bool singleUse = false)
    : base(onClick, singleUse)
    {
        Event = keyboardEvent;
        Key = key;
    }

    internal KeySubscription(
        Key key,
        Action<Key> onClick,
        TimeSpan interval,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
        : base(onClick, interval)
    {
        Event = keyboardEvent;
        Key = key;
    }

    internal void Invoke()
    {
        if (LastInvoked.HasValue && SingleUse)
            return;

        if (LastInvoked?.Add(IntervalOfClick) >= DateTime.Now)
            return;

        LastInvoked = DateTime.Now;
        OnClick(Key);
    }
}