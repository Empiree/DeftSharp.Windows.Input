using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseListenerInterceptor : MouseInterceptor
{
    private readonly ObservableCollection<MouseSubscription> _subscriptions;

    public IEnumerable<MouseSubscription> Subscriptions => _subscriptions;

    public MouseListenerInterceptor()
        : base(InterceptorType.Observable)
    {
        _subscriptions = new ObservableCollection<MouseSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    public void Subscribe(MouseSubscription subscription)
    {
        if (Subscriptions.Any(sub => sub.Id.Equals(subscription.Id)))
            return;

        _subscriptions.Add(subscription);
    }

    public void Unsubscribe(MouseEvent mouseEvent)
    {
        var subscriptions = _subscriptions.Where(e => e.Event.Equals(mouseEvent)).ToArray();

        foreach (var mouseSubscription in subscriptions)
            _subscriptions.Remove(mouseSubscription);
    }

    public void Unsubscribe(Guid id)
    {
        var mouseEvent =
            _subscriptions.FirstOrDefault(sub => sub.Id.Equals(id));

        if (mouseEvent is null)
            return;

        _subscriptions.Remove(mouseEvent);
    }

    public void Unsubscribe()
    {
        if (_subscriptions.Any())
            _subscriptions.Clear();
    }

    public Point GetPosition() => Mouse.GetPosition();

    public bool IsKeyPressed(MouseButton button) => Mouse.IsKeyPressed(button);

    public override void Dispose()
    {
        Unsubscribe();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !Subscriptions.Any();
    protected override bool IsInputAllowed(MouseInputArgs args) => true;

    protected override void OnInputSuccess(MouseInputArgs args)
    {
        var events = args.Event.ToMouseEvents();

        var mouseSubscriptions = _subscriptions
            .Where(s => events.Contains(s.Event))
            .ToList();

        foreach (var subscription in mouseSubscriptions)
        {
            if (subscription.SingleUse)
                Unsubscribe(subscription.Id);

            subscription.Invoke(args.Event);
        }
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }
}