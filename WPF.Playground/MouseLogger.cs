using System.Collections.Generic;
using System.Diagnostics;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Pipeline;

namespace WPF.Playground;

public class MouseLogger : CustomMouseInterceptor
{
    // Always allow input because it's a logger.
    protected override bool IsInputAllowed(MouseInputArgs args) => true;

    // If the input event was successfully processed
    protected override void OnInputSuccess(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Move) // Don't log a move event
            return;
        
        Trace.WriteLine($"Proceed {args.Event}");
    }

    // If the input event has been prohibited
    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        var failureReason = failedInterceptors.ToNames(); //string.Join(", ", failedInterceptors.Select(i => i.Name));
        
        Trace.WriteLine($"Failed {args.Event} by: {failureReason}");
    }
}