using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseListenerInterceptor : MouseInterceptor, IMouseListener
{
    private readonly ObservableCollection<MouseSubscription> _subscriptions;

    public IEnumerable<MouseSubscription> Subscriptions => _subscriptions;

    public MouseListenerInterceptor()
        : base(WindowsMouseInterceptor.Instance)
    {
        _subscriptions = new ObservableCollection<MouseSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    ~MouseListenerInterceptor() => Dispose();

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

    public void UnsubscribeAll()
    {
        if (_subscriptions.Any())
            _subscriptions.Clear();
    }

    public Coordinates GetPosition() => Mouse.GetPosition();

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !Subscriptions.Any();

    protected override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(true, InterceptorType.Listener, () => HandleMouseInput(args));

    private void HandleMouseInput(MouseInputArgs args)
    {
        var mouseSubscriptions = _subscriptions
            .Where(s => s.Event.Equals(args.Event))
            .ToList();

        var genericSubscriptions = GetGenericSubscriptions(args.Event);
        
        mouseSubscriptions.AddRange(genericSubscriptions);

        foreach (var subscription in mouseSubscriptions)
        {
            if (subscription.SingleUse)
                Unsubscribe(subscription.Id);

            subscription.Invoke();
        }
    }

    private IEnumerable<MouseSubscription> GetGenericSubscriptions(MouseEvent mouseEvent)
    {
        if (IsMouseDownEvent(mouseEvent))
            return _subscriptions.Where(s => s.Event.Equals(MouseEvent.ButtonDown));

        if (IsMouseUpEvent(mouseEvent))
            return _subscriptions.Where(s => s.Event.Equals(MouseEvent.ButtonUp));
        
        return Enumerable.Empty<MouseSubscription>();
    }

    private bool IsMouseDownEvent(MouseEvent mouseEvent) =>
        mouseEvent is MouseEvent.LeftButtonDown or
            MouseEvent.RightButtonDown or MouseEvent.MiddleButtonDown;

    private bool IsMouseUpEvent(MouseEvent mouseEvent) =>
        mouseEvent is MouseEvent.LeftButtonUp or
            MouseEvent.RightButtonUp or MouseEvent.MiddleButtonUp;

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }
}