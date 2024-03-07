using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Mouse;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseManipulator : IDisposable
{
    private readonly IMouseManipulatorInterceptor _mouseInterceptor;
    public IEnumerable<MouseEvent> LockedKeys => _mouseInterceptor.LockedKeys;

    public event Action<MouseEvent>? ClickPrevented;
    public event Action<MouseEvent>? ClickReleased;

    public MouseManipulator()
    {
        _mouseInterceptor = MouseManipulatorInterceptor.Instance;
        _mouseInterceptor.ClickPrevented += OnInterceptorClickPrevented;
        _mouseInterceptor.ClickReleased += OnInterceptorClickReleased;
    }

    public void Prevent(MouseEvent mouseEvent) => _mouseInterceptor.Prevent(mouseEvent);
    public void Release(MouseEvent mouseEvent) => _mouseInterceptor.Release(mouseEvent);
    public void ReleaseAll() => _mouseInterceptor.ReleaseAll();
    public void SetPosition(int x, int y) => _mouseInterceptor.SetPosition(x, y);
    public void Click(int x, int y, MouseButton button = MouseButton.Left) => _mouseInterceptor.Click(x, y, button);

    public void Dispose()
    {
        _mouseInterceptor.ClickPrevented -= OnInterceptorClickPrevented;
        _mouseInterceptor.ClickReleased -= OnInterceptorClickReleased;
    }
    
    private void OnInterceptorClickPrevented(MouseEvent mouseEvent) => ClickPrevented?.Invoke(mouseEvent);
    private void OnInterceptorClickReleased(MouseEvent mouseEvent) => ClickReleased?.Invoke(mouseEvent);
}