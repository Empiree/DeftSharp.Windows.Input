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
        : base(WindowsKeyboardInterceptor.Instance)
    {
    }

    protected sealed override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(
            IsInputAllowed(args),
            InterceptorType.Custom,
            () => OnSuccess(args),
            failedInterceptors => OnFailure(args, failedInterceptors));

    protected sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// This method is called when the input event is triggered. The return value is responsible for whether we allow this event or not.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <returns><b>True</b> - allow input . <b>False</b> - cancel input.</returns>
    protected abstract bool IsInputAllowed(KeyPressedArgs args);

    /// <summary>
    /// This method is called when the IsInputAllowed() method is successfully executed on all active interceptors.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract void OnSuccess(KeyPressedArgs args);

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not resolve the key press.</param>
    protected abstract void OnFailure(KeyPressedArgs args, IEnumerable<InterceptorType> failedInterceptors);
}