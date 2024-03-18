using System;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal abstract class KeyboardInterceptor : IInterceptor
{
    protected readonly IKeyboardInterceptor Keyboard;

    protected KeyboardInterceptor(IKeyboardInterceptor keyboardInterceptor)
    {
        Keyboard = keyboardInterceptor;
        Keyboard.KeyboardInput += OnKeyboardInput;
        Keyboard.UnhookRequested += OnInterceptorUnhookRequested;
    }

    public virtual void Dispose()
    {
        Keyboard.KeyboardInput -= OnKeyboardInput;
        Keyboard.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    protected abstract InterceptorResponse OnKeyboardInput(KeyPressedArgs args);
    protected abstract bool OnInterceptorUnhookRequested();

    public void Hook() => Keyboard.Hook();
    public void Unhook() => Keyboard.Unhook();

    public event Func<bool>? UnhookRequested;
}