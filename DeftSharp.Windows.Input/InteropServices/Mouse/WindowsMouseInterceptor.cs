using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Delegates;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

internal sealed class WindowsMouseInterceptor : WindowsInterceptor, IMouseInterceptor
{
    #region Singleton

    private static readonly Lazy<WindowsMouseInterceptor> LazyInstance = new(() => new WindowsMouseInterceptor());
    public static WindowsMouseInterceptor Instance => LazyInstance.Value;

    #endregion

    public event MousePipelineDelegate? InterceptorPipelineRequested;

    private WindowsMouseInterceptor()
        : base(InputMessages.WhMouseLl)
    {
    }

    public Coordinates GetPosition() => MouseAPI.GetPosition();
    public void SetPosition(int x, int y) => MouseAPI.SetPosition(x, y);
    public void Click(int x, int y, MouseButton button) => MouseAPI.Click(button, x, y);
    

    /// <summary>
    /// Callback method for the mouse hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected override nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0 || !InputMessages.IsMouseEvent(wParam))
            return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);

        var mouseEvent = (MouseEvent)wParam;
        var args = new MouseInputArgs(mouseEvent);

        return CanBeProcessed(args)
            ? WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam)
            : 1;
    }

    /// <summary>
    /// Checks whether the provided key press event can be processed by the registered event handlers.
    /// </summary>
    /// <param name="args">The <see cref="MouseInputArgs"/> representing the mouse press event.</param>
    /// <returns>True if the event can be processed; otherwise, false.</returns>
    private bool CanBeProcessed(MouseInputArgs args)
    {
        if (InterceptorPipelineRequested is null) 
            return true;

        var interceptors = new List<InterceptorResponse>();
        
        foreach (var nextInterceptor in InterceptorPipelineRequested.GetInvocationList())
        {
            var interceptor = ((MousePipelineDelegate)nextInterceptor).Invoke(args);
            interceptors.Add(interceptor);
        }

        var canBeProcessed = interceptors.All(i => i.IsAllowed);

        var callBacks = canBeProcessed
            ? interceptors.Select(i => i.OnPipelineSuccess).ToArray()
            : interceptors.Select(i => i.OnPipelineFailed).ToArray();
        
        foreach (var callBack in callBacks)
            callBack?.Invoke();

        return canBeProcessed;
    }
}