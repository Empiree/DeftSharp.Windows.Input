using System.Diagnostics;
using DeftSharp.Windows.Keyboard.InteropServices.API;
using DeftSharp.Windows.Keyboard.Shared.Exceptions;

namespace DeftSharp.Windows.Keyboard.InteropServices;

public abstract class WindowsListener
{
    /// <summary>
    /// Delegate to the hook procedure.
    /// </summary>
    internal readonly WinAPI.WindowsProcedure WindowsProcedure;

    /// <summary>
    /// Identifier for the installed WinAPI hook.
    /// </summary>
    protected nint HookId = nint.Zero;

    public bool Handled { get; protected set; }

    /// <summary>
    /// Initializes a new instance of the WindowsListener class.
    /// </summary>
    protected WindowsListener()
    {
        WindowsProcedure = HookCallback;
    }

    /// <summary>
    /// Installs the WinAPI hook procedure.
    /// </summary>
    /// <param name="idHook">Identifier for the installed WinAPI hook.</param>
    /// <param name="procedure">A pointer to the windows hook procedure.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    internal nint SetHook(int idHook, WinAPI.WindowsProcedure procedure)
    {
        using var currentProcess = Process.GetCurrentProcess();
        using var currentModule = currentProcess.MainModule;

        if (currentModule?.ModuleName is null)
            throw new MainModuleException();

        return WinAPI.SetWindowsHookEx(idHook, procedure, WinAPI.GetModuleHandle(currentModule.ModuleName), 0);
    }

    /// <summary>
    /// Callback method for the Windows hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected abstract nint HookCallback(int nCode, nint wParam, nint lParam);
}