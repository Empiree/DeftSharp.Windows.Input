using System;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

/// <summary>
/// Represents the arguments for mouse input events.
/// </summary>
internal class MouseInputArgs : EventArgs
{
    /// <summary>
    /// The mouse event associated with these arguments.
    /// </summary>
    public MouseEvent Event { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MouseInputArgs"/> class.
    /// </summary>
    /// <param name="mouseEvent">The mouse event associated with these arguments.</param>
    public MouseInputArgs(MouseEvent mouseEvent)
    {
        Event = mouseEvent;
    }
}