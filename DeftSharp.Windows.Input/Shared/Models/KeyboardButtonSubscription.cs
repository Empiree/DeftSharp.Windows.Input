using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Models;

public sealed class KeyboardButtonSubscription
{
    private readonly Action<Key> _onClick;

    public Guid Id { get; }
    public Key Key { get; }
    public KeyboardEvent Event { get; }
    public TimeSpan IntervalOfClick { get; }
    public DateTime? LastInvoked { get; private set; }
    public bool SingleUse { get; }

    public KeyboardButtonSubscription(
        Key key,
        Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown,
        bool singleUse = false)
    {
        _onClick = onClick;

        SingleUse = singleUse;
        Event = keyboardEvent;
        Key = key;
        Id = Guid.NewGuid();
    }

    public KeyboardButtonSubscription(
        Key key,
        Action<Key> onClick,
        TimeSpan interval,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
        : this(key, onClick, keyboardEvent)
    {
        IntervalOfClick = interval;
    }

    public void Invoke()
    {
        if (LastInvoked.HasValue && SingleUse)
            return;

        if (LastInvoked?.Add(IntervalOfClick) >= DateTime.Now)
            return;

        LastInvoked = DateTime.Now;
        _onClick(Key);
    }
}