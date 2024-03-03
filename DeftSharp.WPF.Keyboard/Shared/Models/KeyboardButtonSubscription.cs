using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Keyboard.Shared.Models;

public sealed class KeyboardButtonSubscription
{
    private readonly Action<Key> _onClick;

    public Guid Id { get; }
    public Key Key { get; }
    public TimeSpan IntervalOfClick { get; }
    public DateTime? LastInvoked { get; private set; }
    public bool SingleUse { get; }

    public KeyboardButtonSubscription(Key key, Action<Key> onClick, TimeSpan? interval = null, bool singleUse = false)
    {
        _onClick = onClick;

        Key = key;
        SingleUse = singleUse;
        IntervalOfClick = interval ?? TimeSpan.Zero;

        Id = Guid.NewGuid();
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