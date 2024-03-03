using System;

namespace DeftSharp.Windows.Keyboard.InteropServices.Mouse;

public class MouseInputArgs: EventArgs
{
    public MouseEvent Event { get; }

    public MouseInputArgs(MouseEvent mouseEvent)
    {
        Event = mouseEvent;
    }
}