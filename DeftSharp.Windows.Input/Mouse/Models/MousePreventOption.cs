namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies options for preventing mouse events.
/// </summary>
public enum MousePreventOption : ushort
{
    /// <summary>
    /// Prevents mouse move events.
    /// </summary>
    Move,

    /// <summary>
    /// Prevents left mouse button events (down, up, double click).
    /// </summary>
    LeftButton,

    /// <summary>
    /// Prevents right mouse button events (down, up).
    /// </summary>
    RightButton,
    
    /// <summary>
    /// Prevents middle mouse button events (down, up).
    /// </summary>
    MiddleButton,
    
    /// <summary>
    /// Prevents mouse scroll events.
    /// </summary>
    Scroll
}