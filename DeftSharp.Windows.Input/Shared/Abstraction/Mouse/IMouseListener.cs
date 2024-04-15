using System;
using System.Collections.Generic;

namespace DeftSharp.Windows.Input.Mouse;

public interface IMouseListener : IDisposable
{
    IEnumerable<MouseSubscription> Subscriptions { get; }

    bool IsListening { get; }
    Point Position { get; }

    MouseSubscription Subscribe(MouseEvent mouseEvent, Action<MouseInputEvent> onAction,
        TimeSpan? interval = null);

    MouseSubscription Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? interval = null);
    MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action<MouseInputEvent> onAction);
    MouseSubscription SubscribeOnce(MouseEvent mouseEvent, Action onAction);
    IEnumerable<MouseSubscription> SubscribeAll(Action<MouseInputEvent> onAction, TimeSpan? interval = null);

    void Unsubscribe(MouseEvent mouseEvent);
    void Unsubscribe(Guid id);
    void Unsubscribe(IEnumerable<Guid> ids);
    void Unsubscribe();

    bool IsKeyPressed(MouseButton button);
}