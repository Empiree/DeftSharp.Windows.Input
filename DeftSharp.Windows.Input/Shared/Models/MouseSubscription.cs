using System;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Models;

public sealed class MouseSubscription
{
    private readonly Action _onClick;

    public Guid Id { get; }
    public MouseEvent Event { get; }
    public TimeSpan IntervalOfClick { get; }
    public DateTime? LastInvoked { get; private set; }
    public bool SingleUse { get; }

    public MouseSubscription(
        MouseEvent mouseEvent,
        Action onClick,
        bool singleUse = false)
    {
        _onClick = onClick;

        SingleUse = singleUse;
        IntervalOfClick = TimeSpan.Zero;
        Event = mouseEvent;
        Id = Guid.NewGuid();
    }

    public MouseSubscription(
        MouseEvent mouseEvent,
        Action onClick,
        TimeSpan interval)
        : this(mouseEvent, onClick)
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
        _onClick();
    }
}