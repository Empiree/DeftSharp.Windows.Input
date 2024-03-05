using System;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

public sealed class WindowsMouseInterceptor : WindowsInterceptor, IMouseInterceptor, IDisposable
{
    #region Singleton

    private static readonly Lazy<WindowsMouseInterceptor> LazyInstance = new(() => new WindowsMouseInterceptor());
    public static WindowsMouseInterceptor Instance => LazyInstance.Value;

    #endregion

    public event EventHandler<MouseInputArgs>? MouseInput;

    private WindowsMouseInterceptor()
        : base(InputMessages.WhMouseLl)
    {
    }

    public Coordinates GetPosition()
    {
        WinAPI.GetCursorPos(out var position);
        return position;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() => Unhook();

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

        var args = new MouseInputArgs((MouseEvent)wParam);

        MouseInput?.Invoke(this, args);

        return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);
    }
}