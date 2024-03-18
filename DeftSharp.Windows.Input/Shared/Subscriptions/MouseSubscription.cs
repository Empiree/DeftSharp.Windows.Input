using System;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Subscriptions;

public sealed class MouseSubscription : InputSubscription<Action>
{
    public MouseEvent Event { get; }

    internal MouseSubscription(
        MouseEvent mouseEvent,
        Action onClick,
        bool singleUse = false)
        : base(onClick, singleUse)
    {
        Event = mouseEvent;
    }

    internal MouseSubscription(
        MouseEvent mouseEvent,
        Action onClick,
        TimeSpan interval)
        : base(onClick, interval)
    {
        Event = mouseEvent;
    }

    internal void Invoke()
    {
        if (!CanBeInvoked())
            return;
        
        LastInvoked = DateTime.Now;
        OnClick();
    }
}