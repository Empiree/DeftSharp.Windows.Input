using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal abstract class MouseInterceptor : IRequestedInterceptor
{
    protected readonly IMouseInterceptor Mouse;

    protected MouseInterceptor(IMouseInterceptor mouseInterceptor)
    {
        Mouse = mouseInterceptor;
        Mouse.MouseInputMiddleware += OnMouseInput;
        Mouse.UnhookRequested += OnInterceptorUnhookRequested;
    }

    public virtual void Dispose()
    {
        Mouse.MouseInputMiddleware -= OnMouseInput;
        Mouse.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    protected abstract InterceptorResponse OnMouseInput(MouseInputArgs args);
    protected abstract bool OnInterceptorUnhookRequested();

    public void Hook() => Mouse.Hook();
    public void Unhook() => Mouse.Unhook();

    public event Func<bool>? UnhookRequested;
}