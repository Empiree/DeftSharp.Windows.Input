using System;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

/// <summary>
/// Parent class for all keyboard interceptors
/// </summary>
public abstract class KeyboardInterceptor : IInterceptor
{
    /// <summary>
    /// Interceptor name
    /// </summary>
    protected readonly string Name;
    protected readonly IKeyboardInterceptor Keyboard;

    /// <summary>
    /// The property indicates whether the hook is captured or not.
    /// </summary>
    public bool IsHandled { get; private set; }

    internal KeyboardInterceptor(IKeyboardInterceptor keyboardInterceptor)
    {
        Keyboard = keyboardInterceptor;
        Name = GetType().Name;
    }

    public virtual void Dispose() => Unhook();

    protected abstract InterceptorResponse OnKeyboardInput(KeyPressedArgs args);
    
    /// <summary>
    /// The method is called if any interceptor has called the Unhook method. If we are still working with key handling,
    /// we return false. If we don't need the system hook anymore, return true. This is intended for optimization. 
    /// </summary>
    /// <returns>If we need hook - false, otherwise true</returns>
    protected abstract bool OnPipelineUnhookRequested();

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

    public event Func<bool>? UnhookRequested;
}