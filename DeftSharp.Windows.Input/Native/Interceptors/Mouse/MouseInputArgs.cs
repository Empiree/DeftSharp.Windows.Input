using System;

namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Represents the arguments for mouse input events.
/// </summary>
public class MouseInputArgs : EventArgs
{
    /// <summary>
    /// The mouse event associated with these arguments.
    /// </summary>
    public MouseInputEvent Event { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseInputArgs"/> class.
    /// </summary>
    /// <param name="mouseEvent">The mouse event associated with these arguments.</param>
    public MouseInputArgs(MouseInputEvent mouseEvent)
    {
        Event = mouseEvent;
    }
}