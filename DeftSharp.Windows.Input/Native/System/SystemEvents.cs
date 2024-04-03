namespace DeftSharp.Windows.Input.Native;

/// <summary>
/// Provides constants for Windows events
/// </summary>
internal static class SystemEvents
{
    /// <summary>
    /// Flag indicating that the key is pressed.
    /// </summary>
    internal const int KeyPressedFlag = 0x8000;

    /// <summary>
    /// Flag indicating the activity of the key.
    /// </summary>
    internal const int KeyActiveFlag = 0x0001;
    
    /// <summary>
    /// Defines a keystroke message sent to the active window when a key is pressed.
    /// </summary>
    internal const int KeyDown = 0x0100;

    /// <summary>
    /// Defines a keystroke message sent to the active window when a key is pressed.
    /// </summary>
    internal const int KeyUp = 0x0101;

    /// <summary>
    /// Defines a system keystroke message sent to the active window when a system key is pressed.
    /// </summary>
    internal const int WmSystemKeyDown = 0x0104;

    /// <summary>
    /// The message for a mouse move event.
    /// </summary>
    internal const int MouseMove = 0x0200;

    /// <summary>
    /// The message for a left mouse button down event.
    /// </summary>
    internal const int MouseLeftButtonDown = 0x0201;

    /// <summary>
    /// The message for a left mouse button up event.
    /// </summary>
    internal const int MouseLeftButtonUp = 0x0202;

    /// <summary>
    /// The message for a left mouse button double-click event.
    /// </summary>
    internal const int MouseLeftButtonDoubleClick = 0x0203;

    /// <summary>
    /// The message for a right mouse button down event.
    /// </summary>
    internal const int MouseRightButtonDown = 0x0204;

    /// <summary>
    /// The message for a right mouse button up event.
    /// </summary>
    internal const int MouseRightButtonUp = 0x0205;

    /// <summary>
    /// The message for a middle mouse button down event.
    /// </summary>
    internal const int MouseMiddleButtonDown = 0x0207;

    /// <summary>
    /// The message for a middle mouse button up event.
    /// </summary>
    internal const int MouseMiddleButtonUp = 0x0208;

    /// <summary>
    /// The message for a mouse wheel event.
    /// </summary>
    internal const int MouseWheel = 0x020A;

    /// <summary>
    /// Determines whether the specified Windows message is a mouse event.
    /// </summary>
    /// <param name="wParam">The wParam value of the Windows message.</param>
    /// <returns>True if the message is a mouse event; otherwise, false.</returns>
    internal static bool IsMouseEvent(nint wParam) =>
        wParam is MouseMove or MouseLeftButtonDown or MouseLeftButtonUp
            or MouseLeftButtonDoubleClick or MouseRightButtonDown or MouseRightButtonUp
            or MouseMiddleButtonDown or MouseMiddleButtonUp or MouseWheel;

    /// <summary>
    /// Determines whether the specified Windows message is a keyboard event.
    /// </summary>
    /// <param name="wParam">The wParam value of the Windows message.</param>
    /// <returns>True if the message is a keyboard event; otherwise, false.</returns>
    internal static bool IsKeyboardEvent(nint wParam) =>
        wParam is KeyDown or KeyUp;
}