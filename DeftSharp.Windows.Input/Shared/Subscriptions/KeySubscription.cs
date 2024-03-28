using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Subscriptions.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeySubscription : InputSubscription<Action<Key, KeyboardInputEvent>>
{
    public Key Key { get; }
    public KeyboardEvent Event { get; }

    internal KeySubscription(
        Key key,
        Action<Key, KeyboardInputEvent> onClick,
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
        Action<Key, KeyboardInputEvent> onClick,
        TimeSpan interval,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
        : base(onClick, interval)
    {
        if (key is Key.None)
            throw new KeyNoneException();
        
        Event = keyboardEvent;
        Key = key;
    }

    internal void Invoke(KeyboardInputEvent inputEvent)
    {
        if (!CanBeInvoked())
            return;

        LastInvoked = DateTime.Now;
        OnClick(Key, inputEvent);
    }
}