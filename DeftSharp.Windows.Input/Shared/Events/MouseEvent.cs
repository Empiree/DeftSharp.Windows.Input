namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies types of mouse events.
/// </summary>
public enum MouseEvent : ushort
{
    /// <summary>
    /// The mouse has been moved.
    /// </summary>
    Move = 512,

    /// <summary>
    /// The left mouse button is pressed down.
    /// </summary>
    LeftButtonDown = 513,

    /// <summary>
    /// The left mouse button is released.
    /// </summary>
    LeftButtonUp = 514,

    /// <summary>
    /// The right mouse button is pressed down.
    /// </summary>
    RightButtonDown = 516,

    /// <summary>
    /// The right mouse button is released.
    /// </summary>
    RightButtonUp = 517,
    
    /// <summary>
    /// The middle mouse button is pressed down.
    /// </summary>
    MiddleButtonDown = 519,
    
    /// <summary>
    /// The middle mouse button is released.
    /// </summary>
    MiddleButtonUp = 520,
    
    /// <summary>
    /// The mouse wheel was scrolled.
    /// </summary>
    Wheel = 522
}