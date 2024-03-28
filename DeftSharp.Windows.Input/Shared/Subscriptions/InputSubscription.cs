using System;

namespace DeftSharp.Windows.Input.Shared.Subscriptions.Input;

public abstract class InputSubscription<TAction>
{
    protected readonly TAction OnClick;
    public Guid Id { get; }
    public TimeSpan IntervalOfClick { get; }
    public DateTime? LastInvoked { get; protected set; }
    public bool SingleUse { get; }

    protected InputSubscription(TAction onClick, bool singleUse = false)
    {
        OnClick = onClick;
        SingleUse = singleUse;
        
        Id = Guid.NewGuid();
    }

    protected InputSubscription(TAction onClick, TimeSpan intervalOfClick)
    : this(onClick)
    {
        IntervalOfClick = intervalOfClick;
    }

    internal virtual bool CanBeInvoked()
    {
        if (LastInvoked.HasValue && SingleUse)
            return false;

        if (LastInvoked?.Add(IntervalOfClick) >= DateTime.Now)
            return false;

        return true;
    }
}