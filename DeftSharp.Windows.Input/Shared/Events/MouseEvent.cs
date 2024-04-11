namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies types of mouse subscription events.
/// </summary>
public enum MouseEvent
{
    /// <summary>
    /// Generic type for all button down events.
    /// </summary>
    ButtonDown,
    
    /// <summary>
    /// Generic type for all button up events.
    /// </summary>
    ButtonUp,
    
    /// <summary>
    /// The mouse has been moved.
    /// </summary>
    Move,

    /// <summary>
    /// The left mouse button is pressed down.
    /// </summary>
    LeftButtonDown,

    /// <summary>
    /// The left mouse button is released.
    /// </summary>
    LeftButtonUp,

    /// <summary>
    /// The right mouse button is pressed down.
    /// </summary>
    RightButtonDown,

    /// <summary>
    /// The right mouse button is released.
    /// </summary>
    RightButtonUp,
    
    /// <summary>
    /// The middle mouse button is pressed down.
    /// </summary>
    MiddleButtonDown,
    
    /// <summary>
    /// The middle mouse button is released.
    /// </summary>
    MiddleButtonUp,
    
    /// <summary>
    /// The mouse wheel was scrolled.
    /// </summary>
    Scroll
}