using System;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal abstract class KeyboardInterceptor : IRequestedInterceptor
{
    protected readonly IKeyboardInterceptor Keyboard;

    protected KeyboardInterceptor(IKeyboardInterceptor keyboardInterceptor)
    {
        Keyboard = keyboardInterceptor;
        Keyboard.InterceptorPipelineRequested += OnInterceptorPipelineRequested;
        Keyboard.UnhookRequested += OnInterceptorUnhookRequested;
    }

    public virtual void Dispose()
    {
        Keyboard.InterceptorPipelineRequested -= OnInterceptorPipelineRequested;
        Keyboard.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    protected abstract InterceptorResponse OnInterceptorPipelineRequested(KeyPressedArgs args);
    protected abstract bool OnInterceptorUnhookRequested();

    public void Hook() => Keyboard.Hook();
    public void Unhook() => Keyboard.Unhook();

    public event Func<bool>? UnhookRequested;
}