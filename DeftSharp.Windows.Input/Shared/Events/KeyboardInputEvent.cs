namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Specifies types of keyboard events.
/// </summary>
public enum KeyboardInputEvent : short
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