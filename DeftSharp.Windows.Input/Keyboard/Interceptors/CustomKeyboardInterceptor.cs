using System.Collections.Generic;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Native.Keyboard;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// The class allows you to create your own custom interceptors
/// </summary>
public abstract class CustomKeyboardInterceptor : KeyboardInterceptor
{
    protected CustomKeyboardInterceptor()
        : base(WindowsKeyboardInterceptor.Instance) { }

    internal sealed override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(
            IsInputAllowed(args),
            new InterceptorInfo(Name, InterceptorType.Custom),
            () => OnInputSuccess(args),
            failedInterceptors => OnInputFailure(args, failedInterceptors));

    internal sealed override bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// This method is called when the input event is triggered. The return value is responsible for whether we allow this event or not.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <returns><b>True</b> - allow input. <b>False</b> - cancel input.</returns>
    protected abstract bool IsInputAllowed(KeyPressedArgs args);

    /// <summary>
    /// This method is called when the IsInputAllowed() method is successfully executed on all active interceptors.
    /// And the input can be successfully processed.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    protected abstract void OnInputSuccess(KeyPressedArgs args);

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not allowed the key press.</param>
    protected abstract void OnInputFailure(KeyPressedArgs args, IEnumerable<InterceptorInfo> failedInterceptors);
}