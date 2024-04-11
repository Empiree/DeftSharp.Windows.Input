namespace DeftSharp.Windows.Input.Native.System;

// https://learn.microsoft.com/en-us/windows/win32/winmsg/about-hooks

/// <summary>
/// Provides constants for Input structure.
/// </summary>
internal static class InputMessages
{
    /// <summary>
    /// The flag for a key down event in the INPUT structure.
    /// </summary>
    internal const int InputKeyDown = 0x0000;

    /// <summary>
    /// The flag for a key extended event in the INPUT structure.
    /// </summary>
    internal const int InputKeyExtended = 0x0001;

    /// <summary>
    /// The flag for a key up event in the INPUT structure.
    /// </summary>
    internal const int InputKeyUp = 0x0002;

    /// <summary>
    /// The type for a keyboard input event in the INPUT structure.
    /// </summary>
    internal const int InputKeyboard = 1;

    /// <summary>
    /// The type for a mouse input event in the INPUT structure.
    /// </summary>
    internal const int InputMouse = 0;

    /// <summary>
    /// Constant representing the left mouse button down event in the INPUT structure.
    /// </summary>
    internal const int InputMouseLeftDown = 0x0002;

    /// <summary>
    /// Constant representing the left mouse button up event in the INPUT structure.
    /// </summary>
    internal const int InputMouseLeftUp = 0x0004;

    /// <summary>
    /// Constant representing the right mouse button down event in the INPUT structure.
    /// </summary>
    internal const int InputMouseRightDown = 0x0008;

    /// <summary>
    /// Constant representing the right mouse button up event in the INPUT structure.
    /// </summary>
    internal const int InputMouseRightUp = 0x0010;

    /// <summary>
    /// Constant representing the mouse wheel event in the INPUT structure.
    /// </summary>
    internal const int InputMouseWheel = 0x0800;

    /// <summary>
    /// Constant representing the middle mouse button down event in the INPUT structure.
    /// </summary>
    internal const int InputMouseMiddleDown = 0x0020;

    /// <summary>
    /// Constant representing the middle mouse button up event in the INPUT structure.
    /// </summary>
    internal const int InputMouseMiddleUp = 0x0040;
}