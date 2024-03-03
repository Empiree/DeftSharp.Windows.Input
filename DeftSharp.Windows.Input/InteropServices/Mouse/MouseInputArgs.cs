using System;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

public class MouseInputArgs : EventArgs
{
    public MouseEvent Event { get; }

    public MouseInputArgs(MouseEvent mouseEvent)
    {
        Event = mouseEvent;
    }
}