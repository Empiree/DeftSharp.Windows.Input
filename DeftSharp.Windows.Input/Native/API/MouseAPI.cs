using System;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Native.API.Structures;
using static DeftSharp.Windows.Input.Native.API.WinAPI;
using static DeftSharp.Windows.Input.Native.API.InputMessages;

namespace DeftSharp.Windows.Input.Native.API;

/// <summary>
/// Provides methods for simulating mouse input using Windows API.
/// </summary>
internal static class MouseAPI
{
    /// <summary>
    /// Retrieves the current position of the cursor on the screen.
    /// </summary>
    /// <returns>The current position of the cursor as a <see cref="Coordinates"/> structure.</returns>
    internal static Coordinates GetPosition()
    {
        GetCursorPos(out var position);
        return position;
    }

    /// <summary>
    /// Scrolls the mouse wheel.
    /// </summary>
    /// <param name="scrollAmount">The amount to scroll. 
    /// Positive value scrolls the wheel up, negative scrolls the wheel down.</param>
    internal static void Scroll(int scrollAmount)
    {
        var input = CreateInput(InputMouseWheel);
        input.u.mi.mouseData = (uint)scrollAmount;

        SendInput(input);
    }
    
    /// <summary>
    /// Sets the position of the mouse cursor.
    /// </summary>
    /// <param name="x">The new x-coordinate of the mouse cursor.</param>
    /// <param name="y">The new y-coordinate of the mouse cursor.</param>
    internal static void SetPosition(int x, int y) => SetCursorPos(x, y);
    
    /// <summary>
    /// Simulates a mouse click at the current position.
    /// </summary>
    /// <param name="button">The mouse button to click.</param>
    internal static void Click(MouseButton button)
    {
        var currentPosition = GetPosition();
        Click(button, currentPosition.X, currentPosition.Y);
    }

    /// <summary>
    /// Simulates a mouse click at the specified position.
    /// </summary>
    /// <param name="button">The mouse button to click.</param>
    /// <param name="x">The x-coordinate of the click position.</param>
    /// <param name="y">The y-coordinate of the click position.</param>
    internal static void Click(MouseButton button, int x, int y)
    {
        SetPosition(x, y);

        switch (button)
        {
            case MouseButton.Left:
                ClickLeft(x, y);
                break;
            case MouseButton.Right:
                ClickRight(x, y);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(button), button, null);
        }
    }

    /// <summary>
    /// Simulates a right mouse click at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the click position.</param>
    /// <param name="y">The y-coordinate of the click position.</param>
    private static void ClickRight(int x, int y)
    {
        mouse_event(InputMouseRightDown, x, y, 0, 0);
        mouse_event(InputMouseRightUp, x, y, 0, 0);
    }

    /// <summary>
    /// Simulates a left mouse click at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the click position.</param>
    /// <param name="y">The y-coordinate of the click position.</param>
    private static void ClickLeft(int x, int y)
    {
        mouse_event(InputMouseLeftDown, x, y, 0, 0);
        mouse_event(InputMouseLeftUp, x, y, 0, 0);
    }
    
    /// <summary>
    /// Creates an INPUT structure representing a mouse input event with the specified move flag.
    /// </summary>
    /// <param name="dwFlags">Flags that specify various aspects of function operation.</param>
    /// <returns>The created INPUT structure.</returns>
    private static Structures.Input CreateInput(uint dwFlags)
    {
        var input = new Structures.Input(InputMouse);
        input.u.mi = new MouseInput();
        input.u.mi.dwFlags = dwFlags;
        return input;
    }
}