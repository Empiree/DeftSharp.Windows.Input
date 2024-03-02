using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Keyboard.InteropServices;

// https://learn.microsoft.com/en-us/windows/win32/api/

/// <summary>
/// Provides a set of methods to interact with Windows API functions.
/// </summary>
internal static class WinAPI
{
    /// <summary>
    /// Defines a delegate for processing keyboard events.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>A handle to the next hook procedure in the chain or <c>0</c> if there's no next procedure.</returns>
    public delegate nint WindowsKeyboardProcedure(int nCode, nint wParam, nint lParam);
    
    /// <summary>
    /// Installs an application-defined hook procedure into a hook chain.
    /// </summary>
    /// <param name="idHook">Specifies the type of hook procedure to be installed.</param>
    /// <param name="lpfn">A pointer to the hook procedure.</param>
    /// <param name="hMod">A handle to the DLL containing the hook procedure pointed to by lpfn.</param>
    /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern nint SetWindowsHookEx(int idHook, WindowsKeyboardProcedure lpfn, nint hMod, uint dwThreadId);

    /// <summary>
    /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function.
    /// </summary>
    /// <param name="hhk">A handle to the hook to be removed.</param>
    /// <returns><c>true</c> if successful; otherwise, <c>false</c>.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool UnhookWindowsHookEx(nint hhk);

    /// <summary>
    /// Passes the hook information to the next hook procedure in the current hook chain.
    /// </summary>
    /// <param name="hhk">A handle to the current hook.</param>
    /// <param name="nCode">Specifies the hook code passed to the current hook procedure.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    /// <summary>
    /// Retrieves a module handle for the specified module.
    /// </summary>
    /// <param name="lpModuleName">The name of the loaded module (either a .dll or .exe file).</param>
    /// <returns>A handle to the specified module, or <c>0</c> if the module could not be found.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern nint GetModuleHandle(string lpModuleName);
}