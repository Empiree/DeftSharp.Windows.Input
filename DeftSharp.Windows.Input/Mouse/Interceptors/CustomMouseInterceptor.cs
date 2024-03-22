using System.Collections.Generic;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

/// <summary>
/// The class allows you to create your own custom interceptors
/// </summary>
public abstract class CustomMouseInterceptor : MouseInterceptor
{
    protected CustomMouseInterceptor()
        : base(WindowsMouseInterceptor.Instance) { }

    protected sealed override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(
            IsInputAllowed(args),
            InterceptorType.Custom,
            () => OnSuccess(args),
            failedInterceptors => OnFailure(args, failedInterceptors));

    protected sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// Can we allow the button to be pressed
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract bool IsInputAllowed(MouseInputArgs args);
    
    /// <summary>
    /// This method is called when the CanBeProcessed() method is successfully executed on all active interceptors.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract void OnSuccess(MouseInputArgs args);
    
    /// <summary>
    /// This method is called if at least one active interceptor returned false in the CanBeProcessed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not resolve the key press.</param>
    protected abstract void OnFailure(MouseInputArgs args, IEnumerable<InterceptorType> failedInterceptors);
}