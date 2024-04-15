using System;

namespace DeftSharp.Windows.Input.Shared.Subscriptions.Input;

public abstract class InputSubscription<TAction>
{
    protected readonly TAction OnClick;
    public Guid Id { get; }
    public TimeSpan Interval { get; }
    public DateTime? LastInvoked { get; protected set; }
    public bool SingleUse { get; }

    protected InputSubscription(TAction onClick, bool singleUse = false)
    {
        OnClick = onClick;
        SingleUse = singleUse;
        
        Id = Guid.NewGuid();
    }

    protected InputSubscription(TAction onClick, TimeSpan interval)
    : this(onClick)
    {
        Interval = interval;
    }

    internal virtual bool CanBeInvoked()
    {
        if (LastInvoked.HasValue && SingleUse)
            return false;

        if (LastInvoked?.Add(Interval) >= DateTime.Now)
            return false;

        return true;
    }
}