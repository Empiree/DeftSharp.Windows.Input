using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Mouse;

namespace WPF.Playground;

public class ScrollDisabler : CustomMouseInterceptor
{
    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Scroll)
            return false; // disallow mouse scroll input
        
        return true; // all other input events can be processed
    }
}