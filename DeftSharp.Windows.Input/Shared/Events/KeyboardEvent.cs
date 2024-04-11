namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Specifies types of keyboard subscription events.
/// </summary>
public enum KeyboardEvent
{
    /// <summary>
    /// Generic type for all events.
    /// </summary>
    All,
    
    /// <summary>
    /// Indicates a key down event.
    /// </summary>
    KeyDown,

    /// <summary>
    /// Indicates a key up event.
    /// </summary>
    KeyUp,
}