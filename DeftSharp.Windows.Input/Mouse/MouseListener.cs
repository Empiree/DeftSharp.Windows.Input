using System;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Listeners;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseListener : InputListener<MouseSubscription>, IDisposable
{
    private readonly IMouseInterceptor _mouseInterceptor;

    public MouseListener()
    {
        _mouseInterceptor = WindowsMouseInterceptor.Instance;
        _mouseInterceptor.MouseInput += OnMouseInput;
        _mouseInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
    }

    private bool OnInterceptorUnhookRequested() => !InputSubscriptions.Any();

    public Coordinates GetPosition() => _mouseInterceptor.GetPosition();

    public void Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? intervalOfClick = null)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, intervalOfClick ?? TimeSpan.Zero);
        InputSubscriptions.Add(subscription);
    }

    public void SubscribeOnce(MouseEvent mouseEvent, Action onAction)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, true);
        InputSubscriptions.Add(subscription);
    }

    public void Unsubscribe(MouseEvent mouseEvent)
    {
        var subscriptions = InputSubscriptions.Where(e => e.Event.Equals(mouseEvent)).ToArray();

        foreach (var mouseSubscription in subscriptions)
            InputSubscriptions.Remove(mouseSubscription);
    }

    public void Unsubscribe(Guid id)
    {
        var mouseEvent = InputSubscriptions.FirstOrDefault(s => s.Id == id);

        if (mouseEvent is null)
            return;

        InputSubscriptions.Remove(mouseEvent);
    }

    public void Dispose()
    {
        Unregister();
        _mouseInterceptor.MouseInput -= OnMouseInput;
        _mouseInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    protected override void Register()
    {
        if (IsListening)
            return;

        _mouseInterceptor.Hook();
        base.Register();
    }

    protected override void Unregister()
    {
        if (!IsListening)
            return;

        UnsubscribeAll();
        _mouseInterceptor.Unhook();
        base.Unregister();
    }

    private void OnMouseInput(object? sender, MouseInputArgs e)
    {
        var mouseEvents =
            InputSubscriptions.Where(s => s.Event.Equals(e.Event)).ToArray();

        foreach (var mouseEvent in mouseEvents)
        {
            if (mouseEvent.SingleUse)
                Unsubscribe(mouseEvent.Id);

            mouseEvent.Invoke();
        }
    }
}