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
        : base(WindowsMouseInterceptor.Instance)
    {
    }

    protected sealed override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(
            IsInputAllowed(args),
            InterceptorType.Custom,
            () => OnSuccess(args),
            failedInterceptors => OnFailure(args, failedInterceptors));

    protected sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// This method is called when the input event is triggered. The return value is responsible for whether we allow this event or not.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    /// <returns><b>True</b> - allow input . <b>False</b> - cancel input.</returns>
    protected abstract bool IsInputAllowed(MouseInputArgs args);

    /// <summary>
    /// This method is called when the IsInputAllowed() method is successfully executed on all active interceptors.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    protected abstract void OnSuccess(MouseInputArgs args);

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not resolve the mouse input.</param>
    protected abstract void OnFailure(MouseInputArgs args, IEnumerable<InterceptorType> failedInterceptors);
}