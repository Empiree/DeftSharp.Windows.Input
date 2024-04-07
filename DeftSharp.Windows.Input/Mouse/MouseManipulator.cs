using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Shared.Attributes;

namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Provides the ability to control the mouse.
/// </summary>
public sealed class MouseManipulator : IMouseManipulator
{
    private readonly MouseManipulatorInterceptor _mouseInterceptor;

    /// <summary>
    /// Gets the mouse events that are currently locked.
    /// </summary>
    public IEnumerable<MouseInputEvent> LockedKeys => _mouseInterceptor.LockedKeys;

    /// <summary>
    /// Event triggered when a mouse input event is prevented.
    /// </summary>
    public event Action<MouseInputEvent>? InputPrevented;

    public MouseManipulator()
    {
        _mouseInterceptor = MouseManipulatorInterceptor.Instance;
        _mouseInterceptor.InputPrevented += OnInterceptorInputPrevented;
    }

    /// <summary>
    /// Checks if the specified mouse event is currently locked.
    /// </summary>
    public bool IsKeyLocked(MouseInputEvent mouseEvent) => _mouseInterceptor.IsKeyLocked(mouseEvent);

    /// <summary>
    /// Prevents the specified mouse event.
    /// </summary>
    [DangerousBehavior("Be careful with the use of this method. You can completely lock your mouse")]
    public void Prevent(PreventMouseEvent mouseEvent, Func<bool>? predicate = null)
    {
        predicate ??= () => true;

        _mouseInterceptor.Prevent(mouseEvent, predicate);
    }

    /// <summary>
    /// Releases the prevention of the specified mouse event.
    /// </summary>
    public void Release(PreventMouseEvent mouseEvent) => _mouseInterceptor.Release(mouseEvent);

    /// <summary>
    /// Releases the prevention of all mouse events.
    /// </summary>
    public void ReleaseAll() => _mouseInterceptor.ReleaseAll();

    /// <summary>
    /// Sets the position of the mouse cursor.
    /// </summary>
    public void SetPosition(int x, int y) => _mouseInterceptor.SetPosition(x, y);

    /// <summary>
    /// Performs a mouse click at the specified coordinates with the specified button.
    /// </summary>
    public void Click(int x, int y, MouseButton button = MouseButton.Left) => _mouseInterceptor.Click(button, x, y);

    /// <summary>
    /// Performs a mouse click with the specified button.
    /// </summary>
    public void Click(MouseButton button = MouseButton.Left) => _mouseInterceptor.Click(button);

    /// <summary>
    /// Performs a double-click at the specified coordinates.
    /// </summary>
    public void DoubleClick(int x, int y)
    {
        Click(x, y);
        Click(x, y);
    }

    /// <summary>
    /// Performs a double-click.
    /// </summary>
    public void DoubleClick()
    {
        Click();
        Click();
    }

    /// <summary>
    /// Scrolls the mouse wheel.
    /// </summary>
    /// <param name="scrollAmount">The amount to scroll. 
    /// Positive value scrolls the wheel up, negative scrolls the wheel down.</param>
    public void Scroll(int scrollAmount) => _mouseInterceptor.Scroll(scrollAmount);

    /// <summary>
    /// Sets the mouse speed.
    /// </summary>
    /// <param name="speed">The desired mouse speed. Must be a value between 1 and 20.</param>
    public void SetMouseSpeed(int speed) => _mouseInterceptor.SetMouseSpeed(speed);

    /// <summary>
    /// Disposes of all resources used by the mouse manipulator.
    /// </summary>
    public void Dispose() => _mouseInterceptor.InputPrevented -= OnInterceptorInputPrevented;

    private void OnInterceptorInputPrevented(MouseInputEvent mouseEvent) => InputPrevented?.Invoke(mouseEvent);
}