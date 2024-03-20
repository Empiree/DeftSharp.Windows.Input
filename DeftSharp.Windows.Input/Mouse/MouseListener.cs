using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseListener : IDisposable
{
    private readonly IMouseListener _mouseInterceptor = new MouseListenerInterceptor();
    public bool IsListening => _mouseInterceptor.Subscriptions.Any();
    public IEnumerable<MouseSubscription> Subscriptions => _mouseInterceptor.Subscriptions;

    public Coordinates GetPosition() => _mouseInterceptor.GetPosition();

    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? intervalOfClick = null)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, intervalOfClick ?? TimeSpan.Zero);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action onAction)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, true);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    public void Unsubscribe(MouseEvent mouseEvent) =>
        _mouseInterceptor.Unsubscribe(mouseEvent);

    public void Unsubscribe(Guid id) =>
        _mouseInterceptor.Unsubscribe(id);

    public void UnsubscribeAll() => _mouseInterceptor.UnsubscribeAll();

    public void Dispose() => _mouseInterceptor.Dispose();
}