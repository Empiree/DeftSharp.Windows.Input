namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies options for simulating mouse button events.
/// </summary>
public enum MouseSimulateOption : ushort
{
    /// <summary>
    /// Simulates a left mouse button down event.
    /// </summary>
    LeftButtonDown = 0x0002,

    /// <summary>
    /// Simulates a left mouse button up event.
    /// </summary>
    LeftButtonUp  = 0x0004,

    /// <summary>
    /// Simulates a right mouse button down event.
    /// </summary>
    RightButtonDown = 0x0008,

    /// <summary>
    /// Simulates a right mouse button up event.
    /// </summary>
    RightButtonUp = 0x0010,

    /// <summary>
    /// Simulates a middle mouse button down event.
    /// </summary>
    MiddleButtonDown = 0x0020,

    /// <summary>
    /// Simulates a middle mouse button up event.
    /// </summary>
    MiddleButtonUp = 0x0040
}