using System;
using System.Diagnostics;
using DeftSharp.Windows.Input.Native;
using DeftSharp.Windows.Input.Native.System;
using DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;
using DeftSharp.Windows.Input.Shared.Exceptions;

namespace DeftSharp.Windows.Input.Interceptors;

internal abstract class WindowsInterceptor : IRequestedInterceptor
{
    private readonly int _interceptorHook;
    private bool _isHandled;

    /// <summary>
    /// Delegate to the hook procedure.
    /// </summary>
    private readonly WindowsProcedure _windowsProcedure;

    /// <summary>
    /// Event that is raised when a request to unhook the windows hook is received.
    /// Handlers of this event can perform additional checks to determine if unhooking should proceed.
    /// </summary>
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
    /// Sets up the windows hook by installing the hook procedure.
    /// </summary>
    public void Hook()
    {
        if (_isHandled)
            return;

        HookId = SetHook(_interceptorHook, _windowsProcedure);
        _isHandled = true;
    }

    /// <summary>
    /// Removes the windows hook by uninstalling the hook procedure.
    /// </summary>
    public void Unhook()
    {
        if (!_isHandled)
            return;

        if (!CanBeUnhooked())
            return;

        User32.UnhookWindowsHookEx(HookId);
        HookId = nint.Zero;
        _isHandled = false;
    }

    /// <summary>
    /// Callback method for the Windows hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected abstract nint HookCallback(int nCode, nint wParam, nint lParam);

    /// <summary>
    /// Checks whether the keyboard hook can be unhooked based on the registered unhook request handlers.
    /// </summary>
    /// <returns>True if the keyboard hook can be unhooked; otherwise, false.</returns>
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

    /// <summary>
    /// Installs the WinAPI hook procedure.
    /// </summary>
    /// <param name="idHook">Identifier for the installed WinAPI hook.</param>
    /// <param name="procedure">A pointer to the windows hook procedure.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    private nint SetHook(int idHook, WindowsProcedure procedure)
    {
        using var currentProcess = Process.GetCurrentProcess();
        using var currentModule = currentProcess.MainModule;

        if (currentModule?.ModuleName is null)
            throw new MainModuleException();

        var moduleHandle = Kernel32.GetModuleHandle(currentModule.ModuleName);

        return User32.SetWindowsHookEx(idHook, procedure, moduleHandle, 0);
    }

    /// <summary>
    /// Disposes of the keyboard listener by unhooking the keyboard hook.
    /// </summary>
    public void Dispose() => Unhook();
}