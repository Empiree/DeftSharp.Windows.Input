using System.Collections.Generic;
using System.Diagnostics;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Keyboard;

namespace WPF.Playground;

/// <summary>
/// Keyboard click`s logger
/// </summary>
public class KeyboardLogger : CustomKeyboardInterceptor
{
    protected override bool IsInputAllowed(KeyPressedArgs args) => true; // Allow any keyboard input events

    // If the keyboard input event has been processed
    protected override void OnInputSuccess(KeyPressedArgs args)
    {
        Trace.WriteLine($"Pressed: {args.KeyPressed} ({args.Event})");
    }

    // If the keyboard input event has been failed
    protected override void OnInputFailure(KeyPressedArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {        
        Trace.WriteLine($"Failed: {args.KeyPressed} ({args.Event})");
    }
}