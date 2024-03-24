using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Mouse.Interceptors;

namespace WPF.Playground;

public class ScrollDisabler : MouseInterceptor
{
    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Scroll)
            return false; // disallow mouse scroll input
        
        return true; // all other input events can be processed
    }
}