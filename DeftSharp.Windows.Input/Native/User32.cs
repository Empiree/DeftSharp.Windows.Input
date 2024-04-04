using System;
using System.Runtime.InteropServices;
using System.Text;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Native.System;

namespace DeftSharp.Windows.Input.Native;

/// <summary>
/// Provides methods for interacting with the User32.dll library.
/// </summary>
internal static class User32
{
    /// <summary>
    /// Retrieves the handle to the current keyboard layout for the specified thread.
    /// </summary>
    /// <param name="idThread">The identifier of the thread to query.</param>
    /// <returns>
    /// The handle to the keyboard layout for the specified thread. 
    /// </returns>
    [DllImport("user32.dll")]
    internal static extern IntPtr GetKeyboardLayout(uint idThread);

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
    /// Retrieves the position of the cursor in screen coordinates.
    /// </summary>
    /// <param name="lpPoint">A reference to a <see cref="Point"/> structure that receives the screen coordinates of the cursor.</param>
    /// <returns>true if successful; otherwise, false. To get extended error information, call GetLastError.</returns>
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern bool GetCursorPos(out Point lpPoint);

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
    private static extern uint SendInput(uint nInputs, System.Input[] pInputs, int cbSize);

    /// <summary>
    /// Retrieves the state of the specified virtual key.
    /// </summary>
    /// <param name="nVirtKey">The virtual-key code.</param>
    /// <returns>The return value specifies the status of the specified virtual key. If the high-order bit is set, the key is down.</returns>
    [DllImport("user32.dll")]
    public static extern short GetKeyState(int nVirtKey);

    /// <summary>
    /// Sends an array of input events to the system input queue.
    /// </summary>
    /// <param name="inputs">An array of INPUT structures representing the input events to send.</param>
    /// <returns>True if the input events were successfully sent; otherwise, false.</returns>
    internal static bool SendInput(System.Input[] inputs) =>
        SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(System.Input))) != 0;

    /// <summary>
    /// Sends an input event to the system input queue.
    /// </summary>
    /// <param name="input">The INPUT structure representing the input event to send.</param>
    /// <returns>True if the input event was successfully sent; otherwise, false.</returns>
    internal static bool SendInput(System.Input input) =>
        SendInput(new[] { input });

    /// <summary>
    /// Translates the specified virtual-key code and keyboard state to the corresponding Unicode character or characters.
    /// </summary>
    /// <param name="wVirtKey">The virtual-key code to be translated.</param>
    /// <param name="wScanCode">The hardware scan code of the key to be translated.</param>
    /// <param name="lpKeyState">A pointer to a 256-byte array that contains the current keyboard state.</param>
    /// <param name="pwszBuff">A pointer to the buffer that receives the translated Unicode character or characters.</param>
    /// <param name="cchBuff">The size, in characters, of the buffer pointed to by the pwszBuff parameter.</param>
    /// <param name="wFlags">Specifies the behavior of the function.</param>
    /// <param name="dwhkl">The input locale identifier.</param>
    /// <returns>
    /// The return value is one of the following:
    /// - -1 if the specified virtual key is a dead-key character (accent or diacritic).
    /// - 0 if the specified virtual key has no translation for the current state of the keyboard.
    /// - 1 if one character was copied to the buffer.
    /// - 2 or greater if two or more characters were copied to the buffer.
    /// </returns>
    [DllImport("user32.dll")]
    internal static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
        [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
        StringBuilder pwszBuff, int cchBuff, uint wFlags,
        IntPtr dwhkl);

    /// <summary>
    /// Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual-key code.
    /// </summary>
    /// <param name="uCode">The virtual-key code or scan code for a key.</param>
    /// <param name="uMapType">The translation to be performed.</param>
    /// <returns>
    /// The return value is either a scan code, a virtual-key code, or a character value, depending on the value of uCode and uMapType.
    /// </returns>
    [DllImport("user32.dll")]
    internal static extern uint MapVirtualKey(uint uCode, uint uMapType);

    /// <summary>
    /// Retrieves or sets system parameters.
    /// </summary>
    /// <param name="uiAction">The system parameter to query or set.</param>
    /// <param name="uiParam">A parameter whose usage and meaning depend on the system parameter being queried or set.</param>
    /// <param name="pvParam">A pointer to the variable that receives the requested information.</param>
    /// <param name="fWinIni">Determines whether the user profile is to be updated.</param>
    /// <returns>
    /// <c>true</c> if the function succeeds, <c>false</c> otherwise. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool SystemParametersInfo(int uiAction, int uiParam, out int pvParam, int fWinIni);

    /// <summary>
    /// Retrieves or sets system parameters.
    /// </summary>
    /// <param name="uiAction">The system parameter to query or set.</param>
    /// <param name="uiParam">A parameter whose usage and meaning depend on the system parameter being queried or set.</param>
    /// <param name="pvParam">A pointer to the variable that receives the requested information.</param>
    /// <param name="fWinIni">Determines whether the user profile is to be updated.</param>
    /// <returns>
    /// <c>true</c> if the function succeeds, <c>false</c> otherwise. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern bool SystemParametersInfo(int uiAction, int uiParam, IntPtr pvParam, int fWinIni);

    /// <summary>
    /// Retrieves the type of keyboard hardware.
    /// </summary>
    /// <param name="nTypeFlag">The type of keyboard hardware to retrieve.</param>
    /// <returns>
    /// If the function succeeds, the return value specifies the type of keyboard hardware. 
    /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
    /// </returns>
    [DllImport("user32.dll", SetLastError = true)]
    internal static extern int GetKeyboardType(int nTypeFlag);
}