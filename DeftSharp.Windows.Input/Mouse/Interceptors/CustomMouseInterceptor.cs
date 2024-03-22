using System.Collections.Generic;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

public abstract class CustomMouseInterceptor : MouseInterceptor
{
    protected CustomMouseInterceptor()
        : base(WindowsMouseInterceptor.Instance) {
    }

    protected sealed override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
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
    protected abstract bool CanBeProcessed(MouseInputArgs args);
    
    /// <summary>
    /// This method is called when the CanBeProcessed() method is successfully executed on all active interceptors.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract void OnPipelineSuccess(MouseInputArgs args);
    
    /// <summary>
    /// This method is called if at least one active interceptor returned false in the CanBeProcessed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not resolve the key press.</param>
    protected abstract void OnPipelineFailed(MouseInputArgs args, IEnumerable<InterceptorType> failedInterceptors);
}