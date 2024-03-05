using System;
using System.Diagnostics;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.InteropServices;

public abstract class WindowsInterceptor : IRequestedInterceptor
{
    private readonly int _interceptorHook;
    private bool _handled;

    /// <summary>
    /// Delegate to the hook procedure.
    /// </summary>
    private readonly WinAPI.WindowsProcedure _windowsProcedure;

    public event Func<bool>? UnhookRequested;

    /// <summary>
    /// Identifier for the installed WinAPI hook.
    /// </summary>
    protected nint HookId = nint.Zero;

    /// <summary>
    /// Initializes a new instance of the WindowsListener class.
    /// </summary>
    protected WindowsInterceptor(int interceptorHook)
    {
        _interceptorHook = interceptorHook;
        _windowsProcedure = HookCallback;
    }

    /// <summary>
    /// Installs the WinAPI hook procedure.
    /// </summary>
    /// <param name="idHook">Identifier for the installed WinAPI hook.</param>
    /// <param name="procedure">A pointer to the windows hook procedure.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    private nint SetHook(int idHook, WinAPI.WindowsProcedure procedure)
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

    public void Hook()
    {
        if (_handled)
            return;

        HookId = SetHook(_interceptorHook, _windowsProcedure);
        _handled = true;
    }

    public void Unhook()
    {
        if (!_handled)
            return;

        if (!CanBeUnhooked())
            return;

        WinAPI.UnhookWindowsHookEx(HookId);
        HookId = nint.Zero;
        _handled = false;
    }

    private bool CanBeUnhooked()
    {
        if (UnhookRequested is null) 
            return true;
        
        foreach (var handler in UnhookRequested.GetInvocationList())
        {
            var canBeUnhooked = (Func<bool>)handler;
            if (!canBeUnhooked())
                return false;
        }

        return true;
    }
}