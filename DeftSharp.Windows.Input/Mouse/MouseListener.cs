using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseListener: IDisposable
{
    private readonly IMouseListener _mouseInterceptor;
    public bool IsListening => _mouseInterceptor.Subscriptions.Any();
    public IEnumerable<MouseSubscription> Subscriptions => _mouseInterceptor.Subscriptions;

    public MouseListener()
    {
        _mouseInterceptor = new MouseListenerInterceptor();
    }

    ~MouseListener()
    {
        Dispose();
    }
    
    public Coordinates GetPosition() => _mouseInterceptor.GetPosition();

    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? intervalOfClick = null) =>
        _mouseInterceptor.Subscribe(mouseEvent, onAction, intervalOfClick ?? TimeSpan.Zero);

    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action onAction) =>
        _mouseInterceptor.SubscribeOnce(mouseEvent, onAction);

    public void Unsubscribe(MouseEvent mouseEvent) =>
        _mouseInterceptor.Unsubscribe(mouseEvent);

    public void Unsubscribe(Guid id) =>
        _mouseInterceptor.Unsubscribe(id);

    public void UnsubscribeAll() => _mouseInterceptor.UnsubscribeAll();
    
    public void Dispose() => _mouseInterceptor.Dispose();
}