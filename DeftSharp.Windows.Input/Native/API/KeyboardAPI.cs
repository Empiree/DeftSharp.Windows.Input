using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Exceptions;
using static DeftSharp.Windows.Input.Native.User32;
using static DeftSharp.Windows.Input.Native.Kernel32;
using static DeftSharp.Windows.Input.Native.System.InputMessages;
using static DeftSharp.Windows.Input.Native.SystemEvents;

namespace DeftSharp.Windows.Input.Native;

/// <summary>
/// Provides methods for simulating keyboard input using Windows API.
/// </summary>
internal static class KeyboardAPI
{
    /// <summary>
    /// Retrieves the current keyboard layout.
    /// </summary>
    /// <returns>
    /// The <see cref="KeyboardLayout"/> object representing the current keyboard layout.
    /// </returns>
    internal static KeyboardLayout GetLayout()
    {
        var layoutHandle = GetLayoutHandle();
        
        var lcid = layoutHandle.ToInt32() & 0xFFFF;
        
        var culture = new CultureInfo(lcid);

        return new KeyboardLayout(culture.KeyboardLayoutId, lcid, culture.Name, culture.DisplayName);
    }

    /// <summary>
    /// Determines whether the specified key is currently active.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key is currently active; otherwise, false.</returns>
    internal static bool IsKeyActive(Key key)
    {
        var keyCode = (byte)KeyInterop.VirtualKeyFromKey(key);
        var keyState = GetKeyState(keyCode);
        return (keyState & KeyActiveFlag) != 0;
    }
    
    /// <summary>
    /// Determines whether the specified key is currently pressed.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>True if the specified key is currently pressed; otherwise, false.</returns>
    internal static bool IsKeyPressed(Key key)
    {
        var keyCode = (byte)KeyInterop.VirtualKeyFromKey(key);
        var keyState = GetKeyState(keyCode);
        return (keyState & KeyPressedFlag) != 0;
    }
    
    /// <summary>
    /// Presses the specified key.
    /// </summary>
    /// <param name="key">The key to press.</param>
    /// <exception cref="KeyboardPressException">Thrown if the key press simulation fails.</exception>
    internal static void Press(Key key)
    {
        var keyCode = (byte)KeyInterop.VirtualKeyFromKey(key);
        var result = SimulateKeyPress(keyCode);
        if (!result)
            throw new KeyboardPressException(key);
    }

    /// <summary>
    /// Presses the specified keys synchronously.
    /// </summary>
    /// <param name="keys">The keys to press.</param>
    internal static void PressSynchronously(IEnumerable<Key> keys)
    {
        var inputs = keys
            .Distinct()
            .Select(k => (byte)KeyInterop.VirtualKeyFromKey(k))
            .Select(code => CreateInput(code))
            .ToArray();
        
        SendInput(inputs);

        for (var i = 0; i < inputs.Length; i++)
            inputs[i].u.ki.dwFlags = InputKeyUp;
        
        SendInput(inputs);
    }
    
    /// <summary>
    /// Retrieves the handle to the current keyboard layout.
    /// </summary>
    internal static IntPtr GetLayoutHandle() 
        => GetKeyboardLayout(GetCurrentThreadId());

    /// <summary>
    /// Simulates pressing a key with the specified virtual key code.
    /// </summary>
    /// <param name="keyCode">The virtual key code of the key to simulate pressing.</param>
    /// <returns>True if the key press simulation was successful; otherwise, false.</returns>
    private static bool SimulateKeyPress(ushort keyCode)
    {
        var input = CreateInput(keyCode);
        var result = SendInput(input);

        if (!result)
            return false;

        input.u.ki.dwFlags = InputKeyUp;
        result = SendInput(input);
        return result;
    }

    /// <summary>
    /// Creates an INPUT structure representing a keyboard input event with the specified virtual key code.
    /// </summary>
    /// <param name="keyCode">The virtual key code of the key.</param>
    /// <param name="dwFlags">Flags that specify various aspects of function operation.</param>
    /// <returns>The created INPUT structure.</returns>
    private static System.Input CreateInput(ushort keyCode, uint dwFlags = InputKeyDown)
    {
        var input = new System.Input(InputKeyboard);
        input.u.ki.wVk = keyCode;
        input.u.ki.dwFlags = dwFlags;
        return input;
    }
}