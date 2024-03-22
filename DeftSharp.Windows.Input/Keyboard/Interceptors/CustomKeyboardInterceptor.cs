using System.Collections.Generic;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

/// <summary>
/// The class allows you to create your own custom interceptors
/// </summary>
public abstract class CustomKeyboardInterceptor : KeyboardInterceptor
{
    protected CustomKeyboardInterceptor()
        : base(WindowsKeyboardInterceptor.Instance) {
    }

    protected sealed override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(
            CanBeProcessed(args),
            InterceptorType.Custom,
            () => OnPipelineSuccess(args),
            failedInterceptors => OnPipelineFailed(args, failedInterceptors));

    protected sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// Can we allow the button to be pressed
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract bool CanBeProcessed(KeyPressedArgs args);
    
    /// <summary>
    /// This method is called when the CanBeProcessed() method is successfully executed on all active interceptors.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract void OnPipelineSuccess(KeyPressedArgs args);
    
    /// <summary>
    /// This method is called if at least one active interceptor returned false in the CanBeProcessed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not resolve the key press.</param>
    protected abstract void OnPipelineFailed(KeyPressedArgs args, IEnumerable<InterceptorType> failedInterceptors);
}