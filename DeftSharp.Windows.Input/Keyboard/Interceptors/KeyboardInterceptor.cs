using System.Collections.Generic;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Keyboard;
using DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

/// <summary>
/// Parent class for all keyboard interceptors
/// </summary>
public abstract class KeyboardInterceptor : IInterceptor
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

    internal readonly IKeyboardInterceptor Keyboard;

    /// <summary>
    /// The property indicates whether the hook is captured or not.
    /// </summary>
    public bool IsHandled { get; private set; }

    protected KeyboardInterceptor()
    {
        Keyboard = WindowsKeyboardInterceptor.Instance;
        _interceptorInfo = new InterceptorInfo(GetType().Name, InterceptorType.Custom);
    }

    internal KeyboardInterceptor(InterceptorType interceptorType)
    {
        Keyboard = WindowsKeyboardInterceptor.Instance;
        _interceptorInfo = new InterceptorInfo(GetType().Name, interceptorType);
    }

    public virtual void Dispose() => Unhook();

    private InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(
            IsInputAllowed(args),
            _interceptorInfo,
            () => OnInputSuccess(args),
            failedInterceptors => OnInputFailure(args, failedInterceptors));

    /// <summary>
    /// The method is called if any interceptor has called the Unhook method. If we are still working with key handling,
    /// we return false. If we don't need the system hook anymore, return true. This is intended for optimization. 
    /// </summary>
    /// <returns>If we need hook - false, otherwise true</returns>
    internal virtual bool OnPipelineUnhookRequested() => !IsHandled;

    /// <summary>
    /// Subscribe to system events about presses.
    /// </summary>
    public void Hook()
    {
        if (IsHandled)
            return;

        IsHandled = true;
        Keyboard.KeyboardInput += OnKeyboardInput;
        Keyboard.UnhookRequested += OnPipelineUnhookRequested;
        Keyboard.Hook();
    }

    /// <summary>
    /// Unsubscribe from all presses. This method will call the UnhookRequested event.
    /// </summary>
    public void Unhook()
    {
        IsHandled = false;
        Keyboard.Unhook();
        Keyboard.KeyboardInput -= OnKeyboardInput;
        Keyboard.UnhookRequested -= OnPipelineUnhookRequested;
    }

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
    protected virtual void OnInputSuccess(KeyPressedArgs args)
    {
    }

    /// <summary>
    /// This method is called if at least one active interceptor returned false in the IsInputAllowed() method.
    /// </summary>
    /// <param name="args">Key pressed args</param>
    /// <param name="failedInterceptors">A list of interceptors that did not allowed the key press.</param>
    protected virtual void OnInputFailure(KeyPressedArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
    }
}