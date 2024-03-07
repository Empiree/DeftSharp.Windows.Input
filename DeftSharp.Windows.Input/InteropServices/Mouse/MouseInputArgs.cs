using System;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

internal class MouseInputArgs : EventArgs
{
    public MouseEvent Event { get; }

    public MouseInputArgs(MouseEvent mouseEvent)
    {
        Event = mouseEvent;
    }
}