using System;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Interfaces;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

public class WindowsMouseListener : WindowsListener, IMouseAPI, IDisposable
{
    public event EventHandler<MouseInputArgs>? MouseInput;

    public void Hook()
    {
        if (Handled)
            return;

        HookId = SetHook(InputMessages.WhMouseLl, WindowsProcedure);
        Handled = true;
    }

    public void Unhook()
    {
        if (!Handled)
            return;

        WinAPI.UnhookWindowsHookEx(HookId);
        Handled = false;
    }

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

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() => Unhook();
}