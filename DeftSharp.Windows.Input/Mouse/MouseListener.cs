using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Mouse.Interceptors;

namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Provides the ability to subscribe to various mouse events.
/// </summary>
public sealed class MouseListener : IMouseListener
{
    private readonly MouseListenerInterceptor _mouseInterceptor = new();

    /// <summary>
    /// Checks if the mouse listener is actively listening for events.
    /// </summary>
    public bool IsListening => _mouseInterceptor.Subscriptions.Any();

    /// <summary>
    /// Gets the current mouse speed level.
    /// </summary>
    public int Speed => _mouseInterceptor.GetSpeed();
    
    /// <summary>
    /// Gets the current mouse position.
    /// </summary>
    public Point Position => _mouseInterceptor.GetPosition();

    /// <summary>
    /// Gets the active subscriptions for mouse events.
    /// </summary>
    public IEnumerable<MouseSubscription> Subscriptions => _mouseInterceptor.Subscriptions;

    /// <summary>
    /// Subscribes to a specific mouse event with the specified action to be performed.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action<MouseInputEvent> onAction,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, intervalOfClick ?? TimeSpan.Zero);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a specific mouse event with the specified action to be performed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action<MouseInputEvent> onAction)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, true);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="MouseEvent"/>.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<MouseSubscription> SubscribeAll(Action<MouseInputEvent> onAction,
        TimeSpan? intervalOfClick = null)
    {
        var events = Enum.GetValues(typeof(MouseEvent))
            .OfType<MouseEvent>()
            .Distinct()
            .ToList();

        events.Remove(MouseEvent.ButtonDown);
        events.Remove(MouseEvent.ButtonUp);

        return events.Select(mouseEvent => Subscribe(mouseEvent, onAction, intervalOfClick)).ToList();
    }

    /// <summary>
    /// Subscribes to a specific mouse event with the specified action to be performed.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? intervalOfClick = null)
        => Subscribe(mouseEvent, _ => onAction(), intervalOfClick);

    /// <summary>
    /// Subscribes to a specific mouse event with the specified action to be performed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action onAction)
        => SubscribeOnce(mouseEvent, _ => onAction());

    /// <summary>
    /// Unsubscribes from the specified mouse event.
    /// </summary>
    public void Unsubscribe(MouseEvent mouseEvent) =>
        _mouseInterceptor.Unsubscribe(mouseEvent);

    /// <summary>
    /// Unsubscribes all subscriptions associated with the specified identifier.
    /// </summary>
    public void Unsubscribe(Guid id) =>
        _mouseInterceptor.Unsubscribe(id);

    /// <summary>
    /// Unsubscribes all mouse event subscriptions.
    /// </summary>
    public void UnsubscribeAll() => _mouseInterceptor.UnsubscribeAll();

    /// <summary>
    /// Disposes of all resources used by the mouse listener.
    /// </summary>
    public void Dispose() => _mouseInterceptor.Dispose();
}