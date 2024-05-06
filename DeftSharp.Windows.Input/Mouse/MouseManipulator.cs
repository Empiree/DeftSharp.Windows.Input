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
    /// Checks if the mouse event is currently locked.
    /// </summary>
    public bool IsKeyLocked(MousePreventOption preventOption) => _mouseInterceptor.IsKeyLocked(preventOption);

    /// <summary>
    /// Prevents the specified mouse event.
    /// </summary>
    [DangerousBehavior("Be careful with the use of this method. You can completely lock your mouse.")]
    public void Prevent(MousePreventOption preventOption, Func<bool>? predicate = null)
    {
        predicate ??= () => true;

        _mouseInterceptor.Prevent(preventOption, predicate);
    }

    /// <summary>
    /// Releases the prevention of the specified mouse event.
    /// </summary>
    public void Release(MousePreventOption preventOption) => _mouseInterceptor.Release(preventOption);

    /// <summary>
    /// Releases the prevention of all mouse events.
    /// </summary>
    public void Release() => _mouseInterceptor.Release();

    /// <summary>
    /// Sets the position of the mouse cursor.
    /// </summary>
    public void SetPosition(int x, int y) => _mouseInterceptor.SetPosition(x, y);

    /// <summary>
    /// Simulates a mouse event at the current coordinates.
    /// </summary>
    public void Simulate(MouseSimulateOption simulateOption) => _mouseInterceptor.Simulate(simulateOption);

    /// <summary>
    /// Simulates a mouse event at the specified coordinates.
    /// </summary>
    public void Simulate(int x, int y, MouseSimulateOption simulateOption) =>
        _mouseInterceptor.Simulate(x, y, simulateOption);

    /// <summary>
    /// Simulates a mouse click at the specified coordinates.
    /// </summary>
    public void Click(int x, int y, MouseButton button = MouseButton.Left) => _mouseInterceptor.Click(x, y, button);

    /// <summary>
    /// Simulates a mouse click at the current coordinates.
    /// </summary>
    public void Click(MouseButton button = MouseButton.Left) => _mouseInterceptor.Click(button);

    /// <summary>
    /// Simulates a mouse double-click at the specified coordinates.
    /// </summary>
    public void DoubleClick(int x, int y)
    {
        Click(x, y);
        Click(x, y);
    }

    /// <summary>
    /// Simulates a mouse double-click at the current coordinates.
    /// </summary>
    public void DoubleClick()
    {
        Click();
        Click();
    }

    /// <summary>
    /// Scrolls the mouse wheel.
    /// </summary>
    /// <param name="rotation">The rotation of the mouse wheel. 
    /// Positive value scrolls the wheel up, negative scrolls the wheel down.</param>
    public void Scroll(int rotation) => _mouseInterceptor.Scroll(rotation);

    /// <summary>
    /// Sets the system mouse speed.
    /// </summary>
    /// <param name="speed">The desired mouse speed. Must be a value between 1 and 20.</param>
    [SystemChanges("This function changes settings in your system.")]
    public void SetMouseSpeed(int speed) => _mouseInterceptor.SetMouseSpeed(speed);

    /// <summary>
    /// Disposes of all resources used by the mouse manipulator.
    /// </summary>
    public void Dispose() => _mouseInterceptor.InputPrevented -= OnInterceptorInputPrevented;

    private void OnInterceptorInputPrevented(MouseInputEvent mouseEvent) => InputPrevented?.Invoke(mouseEvent);
}