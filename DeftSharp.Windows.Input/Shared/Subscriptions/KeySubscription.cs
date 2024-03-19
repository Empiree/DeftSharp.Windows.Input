using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Exceptions;

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
        if (key is Key.None)
            throw new KeyNoneException();
        
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
        if (key is Key.None)
            throw new KeyNoneException();
        
        Event = keyboardEvent;
        Key = key;
    }

    internal void Invoke()
    {
        if (!CanBeInvoked())
            return;

        LastInvoked = DateTime.Now;
        OnClick(Key);
    }
}