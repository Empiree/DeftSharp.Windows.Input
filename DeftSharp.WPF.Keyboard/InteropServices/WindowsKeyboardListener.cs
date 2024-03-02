using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Keyboard.Shared.Exceptions;

namespace DeftSharp.Windows.Keyboard.InteropServices;

/// <summary>
/// Listens for system keyboard events and raises an event when a key is pressed.
/// </summary>
public abstract class WindowsKeyboardListener: IDisposable
{
    /// <summary>
    /// Occurs when a key is pressed.
    /// </summary>
    public event EventHandler<KeyPressedArgs>? KeyPressed;

    /// <summary>
    /// Delegate to the keyboard hook procedure.
    /// </summary>
    private readonly WinAPI.WindowsKeyboardProcedure _procedure;

    /// <summary>
    /// Identifier for the installed keyboard hook.
    /// </summary>
    private nint _hookId = nint.Zero;

    /// <summary>
    /// Initializes a new instance of the WindowsKeyboardListener class.
    /// </summary>
    protected WindowsKeyboardListener()
    {
        _procedure = HookCallback;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() => UnHookKeyboard();

    /// <summary>
    /// Installs the keyboard hook.
    /// </summary>
    protected void HookKeyboard()
    {
        _hookId = SetHook(_procedure);
    }

    /// <summary>
    /// Uninstalls the keyboard hook.
    /// </summary>
    protected void UnHookKeyboard()
    {
        WinAPI.UnhookWindowsHookEx(_hookId);
    }

    /// <summary>
    /// Installs the keyboard hook procedure.
    /// </summary>
    /// <param name="procedure">A pointer to the keyboard hook procedure.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    private nint SetHook(WinAPI.WindowsKeyboardProcedure procedure)
    {
        using var currentProcess = Process.GetCurrentProcess();
        using var currentModule = currentProcess.MainModule;

        if (currentModule?.ModuleName is null)
            throw new MainModuleException();

        return WinAPI.SetWindowsHookEx(Hooks.WhKeyboardLl, procedure, WinAPI.GetModuleHandle(currentModule.ModuleName),
            0);
    }

    /// <summary>
    /// Callback method for the keyboard hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    private nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if ((nCode < 0 || wParam != Hooks.WmKeydown) && wParam != Hooks.WmSystemKeyDown)
            return WinAPI.CallNextHookEx(_hookId, nCode, wParam, lParam);

        var virtualKeyCode = Marshal.ReadInt32(lParam);
        var keyPressedArgs = new KeyPressedArgs(KeyInterop.KeyFromVirtualKey(virtualKeyCode));

        KeyPressed?.Invoke(this, keyPressedArgs);

        return WinAPI.CallNextHookEx(_hookId, nCode, wParam, lParam);
    }
}