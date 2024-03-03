using System;
using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Keyboard.InteropServices.API;

namespace DeftSharp.Windows.Keyboard.InteropServices.Keyboard;

/// <summary>
/// Listens for system keyboard events and raises an event when a key is pressed.
/// </summary>
public abstract class WindowsKeyboardListener : WindowsListener, IDisposable
{
    /// <summary>
    /// Occurs when a key is pressed.
    /// </summary>
    public event EventHandler<KeyPressedArgs>? KeyPressed;

    /// <summary>
    /// Installs the keyboard hook.
    /// </summary>
    protected void HookKeyboard()
    {
        if (Handled)
            return;

        HookId = SetHook(InputMessages.WhKeyboardLl, WindowsProcedure);
        Handled = true;
    }

    /// <summary>
    /// Uninstalls the keyboard hook.
    /// </summary>
    protected void UnHookKeyboard()
    {
        if (!Handled)
            return;

        WinAPI.UnhookWindowsHookEx(HookId);
        Handled = false;
    }

    /// <summary>
    /// Callback method for the keyboard hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected override nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if ((nCode < 0 || !InputMessages.IsKeyboardEvent(wParam)) && wParam != InputMessages.WmSystemKeyDown)
            return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);

        var virtualKeyCode = Marshal.ReadInt32(lParam);

        var key = KeyInterop.KeyFromVirtualKey(virtualKeyCode);
        var keyEvent = (KeyboardEvent)wParam;

        var keyPressedArgs = new KeyPressedArgs(key, keyEvent);

        KeyPressed?.Invoke(this, keyPressedArgs);

        return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() => UnHookKeyboard();
}