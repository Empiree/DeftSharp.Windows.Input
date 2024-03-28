using System;
using DeftSharp.Windows.Input.Shared.Subscriptions.Input;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseSubscription : InputSubscription<Action<MouseInputEvent>>
{
    public MouseEvent Event { get; }

    internal MouseSubscription(
        MouseEvent mouseEvent,
        Action<MouseInputEvent> onClick,
        bool singleUse = false)
        : base(onClick, singleUse) =>
        Event = mouseEvent;

    internal MouseSubscription(
        MouseEvent mouseEvent,
        Action<MouseInputEvent> onClick,
        TimeSpan interval)
        : base(onClick, interval) =>
        Event = mouseEvent;

    internal void Invoke(MouseInputEvent inputEvent)
    {
        if (!CanBeInvoked())
            return;
        
        LastInvoked = DateTime.Now;
        OnClick(inputEvent);
    }
}