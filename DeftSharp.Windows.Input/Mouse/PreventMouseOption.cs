namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies options for preventing mouse events.
/// </summary>
public enum PreventMouseOption : ushort
{
    /// <summary>
    /// Prevents mouse move events.
    /// </summary>
    Move = 512,

    /// <summary>
    /// Prevents left mouse button events (down, up, double click).
    /// </summary>
    LeftButton = 513,

    /// <summary>
    /// Prevents right mouse button events (down, up).
    /// </summary>
    RightButton = 516
}