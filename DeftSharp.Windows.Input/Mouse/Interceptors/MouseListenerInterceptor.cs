using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;
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

    ~MouseListenerInterceptor()
    {
        Dispose();
    }

    public void Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan intervalOfClick)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, intervalOfClick);
        _subscriptions.Add(subscription);
    }

    public void SubscribeOnce(MouseEvent mouseEvent, Action onAction)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, true);
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
        var mouseEvent = _subscriptions.FirstOrDefault(s => s.Id == id);

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

    protected override InterceptorResponse OnInterceptorPipelineRequested(MouseInputArgs args) =>
        new(true, PipelineInterceptor.Listener,() => HandleMouseInput(this, args));

    private void HandleMouseInput(object? sender, MouseInputArgs e)
    {
        var mouseEvents = _subscriptions.Where(s => s.Event.Equals(e.Event)).ToArray();

        foreach (var mouseEvent in mouseEvents)
        {
            if (mouseEvent.SingleUse)
                Unsubscribe(mouseEvent.Id);

            mouseEvent.Invoke();
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