namespace DeftSharp.Windows.Keyboard.InteropServices.Keyboard;

/// <summary>
/// Represents keyboard events.
/// </summary>
public enum KeyboardEvent : short
{
    /// <summary>
    /// Indicates a key down event.
    /// </summary>
    KeyDown = 256,

    /// <summary>
    /// Indicates a key up event.
    /// </summary>
    KeyUp = 257
}