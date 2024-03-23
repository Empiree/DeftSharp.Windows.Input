using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse.Interceptors;

namespace WPF.Playground;

public class ScrollDisabler : CustomMouseInterceptor
{
    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Scroll)
            return false; // disable mouse wheel input
        
        return true; // all other events can be processed
    }
}