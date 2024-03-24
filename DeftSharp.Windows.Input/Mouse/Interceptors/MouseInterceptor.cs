using System.Collections.Generic;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Mouse;
using DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

/// <summary>
/// Parent class for all mouse interceptors
/// </summary>
public abstract class MouseInterceptor : IInterceptor
{
    private readonly InterceptorInfo _interceptorInfo;

    /// <summary>
    /// Interceptor name
    /// </summary>
    protected string Name => _interceptorInfo.Name;

    /// <summary>
    /// Interceptor type
    /// </summary>
    protected InterceptorType Type => _interceptorInfo.Type;

    internal readonly IMouseInterceptor Mouse;

    /// <summary>
    /// The property indicates whether the hook is captured or not.
    /// </summary>
    public bool IsHandled { get; private set; }

    protected MouseInterceptor()
    {
        Mouse = WindowsMouseInterceptor.Instance;
        _interceptorInfo = new InterceptorInfo(GetType().Name, InterceptorType.Custom);
    }

    internal MouseInterceptor(InterceptorType type)
    {
        Mouse = WindowsMouseInterceptor.Instance;
        _interceptorInfo = new InterceptorInfo(GetType().Name, type);
    }

    private InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(
            IsInputAllowed(args),
            _interceptorInfo,
            () => OnInputSuccess(args),
            failedInterceptors => OnInputFailure(args, failedInterceptors));

    public virtual void Dispose() => Unhook();

    /// <summary>
    /// The method is called if any interceptor has called the Unhook method. If we are still working with mouse handling,
    /// we return false. If we don't need the system hook anymore, return true. This is intended for optimization. 
    /// </summary>
    /// <returns>If we need hook - false, otherwise true</returns>
    internal virtual bool OnPipelineUnhookRequested() => !IsHandled;

    public void Hook()
    {
        if (IsHandled)
            return;

        IsHandled = true;
        Mouse.MouseInput += OnMouseInput;
        Mouse.UnhookRequested += OnPipelineUnhookRequested;
        Mouse.Hook();
    }

    public void Unhook()
    {
        IsHandled = false;
        Mouse.Unhook();
        Mouse.MouseInput -= OnMouseInput;
        Mouse.UnhookRequested -= OnPipelineUnhookRequested;
    }

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
    protected virtual void OnInputSuccess(MouseInputArgs args)
    {
    }

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Mouse input args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not allowed the mouse input.</param>
    protected virtual void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
    }
}