using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Mouse;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal class MouseListenerInterceptor : IMouseListenerInterceptor
{
    public event Func<bool>? UnhookRequested;

    private readonly ObservableCollection<MouseSubscription> _subscriptions;
    private readonly IMouseInterceptor _mouseInterceptor;

    public IEnumerable<MouseSubscription> Subscriptions => _subscriptions;

    public MouseListenerInterceptor()
    {
        _subscriptions = new ObservableCollection<MouseSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;

        _mouseInterceptor = WindowsMouseInterceptor.Instance;
        _mouseInterceptor.MouseProcessing += OnKeyProcessing;
        _mouseInterceptor.UnhookRequested += OnUnhookRequested;
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

    public void Hook() => _mouseInterceptor.Hook();
    public void Unhook() => _mouseInterceptor.Unhook();
    public Coordinates GetPosition() => _mouseInterceptor.GetPosition();

    public void Dispose()
    {
        UnsubscribeAll();
        _mouseInterceptor.MouseProcessing -= OnKeyProcessing;
        _mouseInterceptor.UnhookRequested -= OnUnhookRequested;
    }

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

    private bool OnUnhookRequested() => (UnhookRequested?.Invoke() ?? true) && !Subscriptions.Any();
    private PipelineInterceptorOperation OnKeyProcessing(MouseInputArgs args) =>
        new(true, () => HandleMouseInput(this, args));

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }
}