namespace DeftSharp.Windows.Keyboard.InteropServices;

/// <summary>
/// Represents keyboard events.
/// </summary>
public enum KeyEvent : short
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