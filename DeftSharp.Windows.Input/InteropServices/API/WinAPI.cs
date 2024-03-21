using System.Runtime.InteropServices;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.InteropServices.API;

// https://learn.microsoft.com/en-us/windows/win32/api/

/// <summary>
/// Provides a set of methods to interact with Windows API functions.
/// </summary>
internal static class WinAPI
{
    /// <summary>
    /// Defines a delegate for processing windows events.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>A handle to the next hook procedure in the chain or <c>0</c> if there's no next procedure.</returns>
    internal delegate nint WindowsProcedure(int nCode, nint wParam, nint lParam);

    /// <summary>
    /// Installs an application-defined hook procedure into a hook chain.
    /// </summary>
    /// <param name="idHook">Specifies the type of hook procedure to be installed.</param>
    /// <param name="lpfn">A pointer to the hook procedure.</param>
    /// <param name="hMod">A handle to the DLL containing the hook procedure pointed to by lpfn.</param>
    /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated.</param>
    /// <returns>A handle to the hook procedure if successful; otherwise, <c>0</c>.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern nint SetWindowsHookEx(int idHook, WindowsProcedure lpfn, nint hMod, uint dwThreadId);

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

    /// <summary>
    /// Retrieves the position of the cursor in screen coordinates.
    /// </summary>
    /// <param name="lpPoint">A reference to a <see cref="Coordinates"/> structure that receives the screen coordinates of the cursor.</param>
    /// <returns>true if successful; otherwise, false. To get extended error information, call GetLastError.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool GetCursorPos(out Coordinates lpPoint);

    /// <summary>
    /// Moves the cursor to the specified screen coordinates.
    /// </summary>
    /// <param name="x">The new x-coordinate of the cursor, in screen coordinates.</param>
    /// <param name="y">The new y-coordinate of the cursor, in screen coordinates.</param>
    /// <returns>true if successful; otherwise, false. To get extended error information, call GetLastError.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool SetCursorPos(int x, int y);

    /// <summary>
    /// Synthesizes mouse motion and button clicks.
    /// </summary>
    /// <param name="dwFlags">Controls various aspects of mouse motion and button clicking. This parameter can be a combination of the following values:
    /// <list type="bullet">
    /// <item><description>MOUSEEVENTF_ABSOLUTE: Specifies absolute coordinates.</description></item>
    /// <item><description>MOUSEEVENTF_LEFTDOWN: Specifies that the left button is down.</description></item>
    /// <item><description>MOUSEEVENTF_LEFTUP: Specifies that the left button is up.</description></item>
    /// <item><description>MOUSEEVENTF_MIDDLEDOWN: Specifies that the middle button is down.</description></item>
    /// <item><description>MOUSEEVENTF_MIDDLEUP: Specifies that the middle button is up.</description></item>
    /// <item><description>MOUSEEVENTF_MOVE: Specifies that the mouse moves.</description></item>
    /// <item><description>MOUSEEVENTF_RIGHTDOWN: Specifies that the right button is down.</description></item>
    /// <item><description>MOUSEEVENTF_RIGHTUP: Specifies that the right button is up.</description></item>
    /// <item><description>MOUSEEVENTF_WHEEL: Specifies that the wheel has been moved.</description></item>
    /// </list>
    /// </param>
    /// <param name="dx">The mouse's absolute position along the x-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE.</param>
    /// <param name="dy">The mouse's absolute position along the y-axis or its amount of motion since the last mouse event was generated, depending on the setting of MOUSEEVENTF_ABSOLUTE.</param>
    /// <param name="cButtons">The number of times the mouse buttons were pressed and released.</param>
    /// <param name="dwExtraInfo">An additional value associated with the mouse event.</param>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);
    
    /// <summary>
    /// Synthesizes a keystroke by generating a KEYBDINPUT structure specifying the event.
    /// </summary>
    /// <param name="bVk">The virtual-key code of the key to be pressed.</param>
    /// <param name="bScan">The hardware scan code of the key to be pressed.</param>
    /// <param name="dwFlags">Flags that specify various aspects of function operation.</param>
    /// <param name="dwExtraInfo">An additional value associated with the keystroke.</param>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, nuint dwExtraInfo);

    /// <summary>
    /// Synthesizes input events such as keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    /// <param name="nInputs">The number of structures in the array pointed to by pInputs.</param>
    /// <param name="pInputs">An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
    /// <param name="cbSize">The size, in bytes, of an INPUT structure.</param>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern uint SendInput(uint nInputs, Structures.Input[] pInputs, int cbSize);
    
    /// <summary>
    /// Retrieves the state of the specified virtual key.
    /// </summary>
    /// <param name="nVirtKey">The virtual-key code.</param>
    /// <returns>The return value specifies the status of the specified virtual key. If the high-order bit is set, the key is down.</returns>
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int nVirtKey);
}