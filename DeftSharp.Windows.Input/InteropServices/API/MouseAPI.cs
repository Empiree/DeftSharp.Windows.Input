using System;
using DeftSharp.Windows.Input.Mouse;
using static DeftSharp.Windows.Input.InteropServices.API.WinAPI;

namespace DeftSharp.Windows.Input.InteropServices.API;

/// <summary>
/// Provides methods for simulating mouse input using Windows API.
/// </summary>
internal static class MouseAPI
{
    internal static Coordinates GetPosition()
    {
        GetCursorPos(out var position);
        return position;
    }
    
    /// <summary>
    /// Sets the position of the mouse cursor.
    /// </summary>
    /// <param name="x">The new x-coordinate of the mouse cursor.</param>
    /// <param name="y">The new y-coordinate of the mouse cursor.</param>
    internal static void SetPosition(int x, int y) => SetCursorPos(x, y);

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
        mouse_event(InputMessages.InputMouseRightDown, x, y, 0, 0);
        mouse_event(InputMessages.InputMouseRightUp, x, y, 0, 0);
    }

    /// <summary>
    /// Simulates a left mouse click at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the click position.</param>
    /// <param name="y">The y-coordinate of the click position.</param>
    private static void ClickLeft(int x, int y)
    {
        mouse_event(InputMessages.InputMouseLeftDown, x, y, 0, 0);
        mouse_event(InputMessages.InputMouseLeftUp, x, y, 0, 0);
    }
}