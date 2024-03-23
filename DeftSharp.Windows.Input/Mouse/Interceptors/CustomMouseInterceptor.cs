using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Native.Mouse;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// The class allows you to create your own custom interceptors
/// </summary>
public abstract class CustomMouseInterceptor : MouseInterceptor
{
    protected CustomMouseInterceptor()
        : base(WindowsMouseInterceptor.Instance) { }

    internal sealed override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(
            IsInputAllowed(args),
            new InterceptorInfo(Name, InterceptorType.Custom),
            () => OnInputSuccess(args),
            failedInterceptors => OnInputFailure(args, failedInterceptors));

    internal sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// This method is called when the input event is triggered. The return value is responsible for whether we allow this event or not.
    /// <para />
    /// <b> WARNING: Be careful with the result of this method. You can completely lock your mouse. </b>
    /// </summary>
    /// <param name="args">Mouse input args</param>
    /// <returns><b>True</b> - allow input. <b>False</b> - cancel input.</returns>
    protected abstract bool IsInputAllowed(MouseInputArgs args);

    /// <summary>
    /// This method is called when the IsInputAllowed() method is successfully executed on all active interceptors.
    /// And the input can be successfully processed.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    protected virtual void OnInputSuccess(MouseInputArgs args) {}

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not allowed the mouse input.</param>
    protected virtual void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors){}
}