using System;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Native.System;
using static DeftSharp.Windows.Input.Native.System.InputMessages;
using static DeftSharp.Windows.Input.Native.User32;

namespace DeftSharp.Windows.Input.Native;

/// <summary>
/// Provides methods for simulating mouse input using Windows API.
/// </summary>
internal static class MouseAPI
{
    /// <summary>
    /// Retrieves the current position of the cursor on the screen.
    /// </summary>
    /// <returns>The current position of the cursor as a <see cref="Point"/> structure.</returns>
    internal static Point GetPosition()
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
    /// Simulates a mouse click at the specified position.
    /// </summary>
    /// <param name="x">The x-coordinate of the click position.</param>
    /// <param name="y">The y-coordinate of the click position.</param>
    /// <param name="button">The mouse button to click.</param>
    internal static void Click(int x, int y, MouseButton button)
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
    /// Retrieves the current mouse speed level, which ranges from 1 to 20.
    /// </summary>
    /// <returns>
    /// The current mouse speed level. Returns <c>-1</c> if the speed level could not be retrieved.
    /// </returns>
    internal static int GetMouseSpeed()
    {
        const int getMouseSpeedInfo = 0x0070;

        var result = SystemParametersInfo(getMouseSpeedInfo, 0, out var sensitivity, 0);

        return result ? sensitivity : -1;
    }

    /// <summary>
    /// Sets the mouse speed.
    /// </summary>
    /// <param name="speed">The desired mouse speed. Must be a value between 1 and 20.</param>
    internal static void SetMouseSpeed(int speed)
    {
        const int setMouseSpeedInfo = 0x0071;
        const int updateIniFile = 0x01;
        const int sendChangeInfo = 0x02;

        var mouseSpeed = speed switch
        {
            > 20 => 20,
            < 1 => 1,
            _ => speed
        };

        SystemParametersInfo(setMouseSpeedInfo, 0, mouseSpeed, updateIniFile | sendChangeInfo);
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
    private static System.Input CreateInput(uint dwFlags)
    {
        var input = new System.Input(InputMouse);
        input.u.mi = new MouseInput();
        input.u.mi.dwFlags = dwFlags;
        return input;
    }
}