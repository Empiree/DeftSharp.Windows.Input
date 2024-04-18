using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Mouse.Interceptors;

namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Provides the ability to subscribe to global mouse events and get current state.
/// </summary>
public sealed class MouseListener : IMouseListener
{
    private readonly MouseListenerInterceptor _mouseInterceptor = new();

    /// <summary>
    /// Checks if the mouse listener is actively listening for events.
    /// </summary>
    public bool IsListening => _mouseInterceptor.Subscriptions.Any();
    
    /// <summary>
    /// Gets the current mouse position.
    /// </summary>
    public Point Position => _mouseInterceptor.GetPosition();

    /// <summary>
    /// Gets the active subscriptions for mouse events.
    /// </summary>
    public IEnumerable<MouseSubscription> Subscriptions => _mouseInterceptor.Subscriptions;

    /// <summary>
    /// Subscribes to an input mouse event with the specified event handler.
    /// </summary>
    /// <param name="mouseEvent">The input mouse event.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <returns>The created subscription.</returns>
    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action<MouseInputEvent> action,
        TimeSpan? interval = null)
    {
        var subscription = new MouseSubscription(mouseEvent, action, interval ?? TimeSpan.Zero);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to an input mouse event with the specified event handler to be performed once.
    /// </summary>
    /// <param name="mouseEvent">The input mouse event.</param>
    /// <param name="action">The work to execute.</param>
    /// <returns>The created subscription.</returns>
    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action<MouseInputEvent> action)
    {
        var subscription = new MouseSubscription(mouseEvent, action, true);
        _mouseInterceptor.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="MouseEvent"/>.
    /// </summary>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<MouseSubscription> SubscribeAll(Action<MouseInputEvent> action,
        TimeSpan? interval = null)
    {
        var events = Enum.GetValues(typeof(MouseEvent))
            .OfType<MouseEvent>()
            .Distinct()
            .ToList();

        events.Remove(MouseEvent.ButtonDown);
        events.Remove(MouseEvent.ButtonUp);

        return events.Select(mouseEvent => Subscribe(mouseEvent, action, interval)).ToList();
    }

    /// <summary>
    /// Subscribes to an input mouse event with the specified event handler.
    /// </summary>
    /// <param name="mouseEvent">The input mouse event.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <returns>The created subscription.</returns>
    public MouseSubscription Subscribe(MouseEvent mouseEvent, Action action, TimeSpan? interval = null)
        => Subscribe(mouseEvent, _ => action(), interval);

    /// <summary>
    /// Subscribes to an input mouse event with the specified event handler to be performed once.
    /// </summary>
    /// <param name="mouseEvent">The input mouse event.</param>
    /// <param name="action">The work to execute.</param>
    /// <returns>The created subscription.</returns>
    public MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action action)
        => SubscribeOnce(mouseEvent, _ => action());

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
    /// Unsubscribes all subscriptions associated with the specified identifiers.
    /// </summary>
    public void Unsubscribe(IEnumerable<Guid> ids)
    {
        foreach (var id in ids.Distinct())
            Unsubscribe(id);
    }

    /// <summary>
    /// Unsubscribes all mouse event subscriptions.
    /// </summary>
    public void Unsubscribe() => _mouseInterceptor.Unsubscribe();

    /// <summary>
    /// Checks if the specified key is currently pressed.
    /// </summary>
    public bool IsKeyPressed(MouseButton button) => _mouseInterceptor.IsKeyPressed(button);

    /// <summary>
    /// Disposes of all resources used by the mouse listener.
    /// </summary>
    public void Dispose() => _mouseInterceptor.Dispose();
}