namespace DeftSharp.Windows.Keyboard.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks

/// <summary>
/// Provides constants for Windows hook types and message codes.
/// </summary>
internal static class Hooks
{
    /// <summary>
    /// Specifies a low-level keyboard input event.
    /// </summary>
    internal const int WhKeyboardLl = 13;

    /// <summary>
    /// Defines a keystroke message sent to the active window when a key is pressed.
    /// </summary>
    internal const int WmKeydown = 0x0100;

    /// <summary>
    /// Defines a system keystroke message sent to the active window when a system key is pressed.
    /// </summary>
    internal const int WmSystemKeyDown = 0x0104;
}