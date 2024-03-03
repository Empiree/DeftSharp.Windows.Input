namespace DeftSharp.Windows.Input.InteropServices.API;

// https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks

/// <summary>
/// Provides constants for Windows hook types and message codes.
/// </summary>
internal static class InputMessages
{
    /// <summary>
    /// Specifies a low-level keyboard input event.
    /// </summary>
    internal const int WhKeyboardLl = 13;

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


    internal const int WhMouseLl = 14;
    internal const int MouseMove = 0x0200;
    internal const int LeftButtonDown = 0x0201;
    internal const int LeftButtonUp = 0x0202;
    internal const int LeftButtonDoubleClick = 0x0203;
    internal const int RightButtonDown = 0x0204;
    internal const int RightButtonUp = 0x0205;

    internal static bool IsMouseEvent(nint wParam) =>
        wParam is MouseMove or LeftButtonDown or LeftButtonUp
            or LeftButtonDoubleClick or RightButtonDown or RightButtonUp;

    internal static bool IsKeyboardEvent(nint wParam) =>
        wParam is KeyDown or KeyUp;
}