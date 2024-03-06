using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseManipulator : IDisposable
{
    private readonly IMouseInterceptor _mouseInterceptor;
    public IEnumerable<MouseEvent> LockedKeys => _mouseInterceptor.LockedKeys;

    public event Action<MouseEvent>? ClickPrevented;
    public event Action<MouseEvent>? ClickReleased;

    public MouseManipulator()
    {
        _mouseInterceptor = WindowsMouseInterceptor.Instance;
        _mouseInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
        _mouseInterceptor.ClickPrevented += OnInterceptorClickPrevented;
        _mouseInterceptor.ClickReleased += OnInterceptorClickReleased;
    }

    public void Prevent(MouseEvent mouseEvent)
    {
        _mouseInterceptor.Hook();
        _mouseInterceptor.Prevent(mouseEvent);
    }

    public void Release(MouseEvent mouseEvent)
    {
        _mouseInterceptor.Release(mouseEvent);
        if (!LockedKeys.Any())
            _mouseInterceptor.Unhook();
    }

    public void ReleaseAll()
    {
        _mouseInterceptor.ReleaseAll();
        _mouseInterceptor.Unhook();
    }

    public void SetPosition(int x, int y) => MouseAPI.SetPosition(x, y);

    public void Click(int x, int y, MouseButton button = MouseButton.Left) => MouseAPI.Click(button, x, y);

    public void Dispose()
    {
        ReleaseAll();
        _mouseInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
        _mouseInterceptor.ClickPrevented -= OnInterceptorClickPrevented;
        _mouseInterceptor.ClickReleased -= OnInterceptorClickReleased;
    }

    private bool OnInterceptorUnhookRequested() => !LockedKeys.Any();
    private void OnInterceptorClickPrevented(MouseEvent mouseEvent) => ClickPrevented?.Invoke(mouseEvent);
    private void OnInterceptorClickReleased(MouseEvent mouseEvent) => ClickReleased?.Invoke(mouseEvent);
}