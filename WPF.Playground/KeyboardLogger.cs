using System.Collections.Generic;
using System.Diagnostics;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace WPF.Playground;

/// <summary>
/// Keyboard click`s logger
/// </summary>
public class KeyboardLogger : KeyboardInterceptor
{
    protected override bool IsInputAllowed(KeyboardInputArgs args) => true; // Allow any keyboard input events

    // If the keyboard input event has been processed
    protected override void OnInputSuccess(KeyboardInputArgs args)
    {
        Trace.WriteLine($"Pressed: {args.KeyPressed} ({args.Event})");
    }

    // If the keyboard input event has been failed
    protected override void OnInputFailure(KeyboardInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        var failureReason = failedInterceptors.ToNames();
        
        Trace.WriteLine($"Failed {args.Event} by: {failureReason}");
    }
}