using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;
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
    /// Marks the input that the key binder sends, so it can be told apart from real key presses.
    /// </summary>
    internal const nint BinderInputExtraInfo = 0x44534249;

    /// <summary>
    /// Retrieves the type of keyboard hardware.
    /// </summary>
    /// <returns>
    /// The type of keyboard hardware as a <see cref="KeyboardType"/> object.
    /// </returns>
    internal static KeyboardType GetKeyboardType()
    {
        const int keyboardTypeFlag = 0;

        var keyboardTypeValue = User32.GetKeyboardType(keyboardTypeFlag);

        var keyboardName = keyboardTypeValue switch
        {
            1 => "IBM PC/XT or compatible (83-key) keyboard",
            2 => "Olivetti \"ICO\" (102-key) keyboard ",
            3 => "IBM PC/AT (84-key) or similar keyboard",
            4 => "IBM enhanced (101- or 102-key) keyboard",
            5 => "Nokia 1050 and similar keyboards",
            6 => "Nokia 9140 and similar keyboards",
            7 => "Japanese keyboard",
            _ => "Unknown"
        };

        return new KeyboardType(keyboardTypeValue, keyboardName);
    }

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
        var keyState = GetAsyncKeyState(keyCode);
        return (keyState & KeyPressedFlag) != 0;
    }

    /// <summary>
    /// Presses the specified key.
    /// </summary>
    /// <param name="key">The key to press.</param>
    internal static void Press(Key key)
    {
        Simulate(key, KeyboardSimulateOption.KeyDown);
        Simulate(key, KeyboardSimulateOption.KeyUp);
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
    /// Simulates a keyboard event based on the specified key and option.
    /// </summary>
    /// <param name="key">The key to simulate.</param>
    /// <param name="option">The option specifying the type of keyboard event to simulate.</param>
    /// <param name="extraInfo">An additional value that the keyboard hook receives with the event.</param>
    internal static void Simulate(Key key, KeyboardSimulateOption option, nint extraInfo = 0)
    {
        var keyCode = (byte)KeyInterop.VirtualKeyFromKey(key);
        var input = CreateInput(keyCode, (uint)option, extraInfo);
        SendInput(input);
    }

    /// <summary>
    /// Retrieves the handle to the current keyboard layout.
    /// </summary>
    internal static IntPtr GetLayoutHandle()
        => GetKeyboardLayout(GetCurrentThreadId());

    /// <summary>
    /// Retrieves the handle to the keyboard layout of the active window, the layout the user types with.
    /// </summary>
    internal static IntPtr GetForegroundLayoutHandle()
    {
        var threadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
        return GetKeyboardLayout(threadId);
    }

    /// <summary>
    /// Gets the letter that the key types in the keyboard layout of the active window.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <returns>The letter in upper case, or <c>null</c> if the key does not type a letter.</returns>
    internal static string? GetLetter(Key key) => GetLetter(key, GetForegroundLayoutHandle());

    /// <summary>
    /// Gets the letter that the key types in the specified keyboard layout.
    /// </summary>
    /// <param name="key">The key to check.</param>
    /// <param name="layout">The handle to the keyboard layout.</param>
    /// <returns>The letter in upper case, or <c>null</c> if the key does not type a letter.</returns>
    internal static string? GetLetter(Key key, IntPtr layout)
    {
        var virtualKeyCode = (uint)KeyInterop.VirtualKeyFromKey(key);
        var scanCode = MapVirtualKeyEx(virtualKeyCode, 0, layout);
        var buffer = new StringBuilder(8);

        // Flag 0x4 keeps the keyboard state as is, so a dead key the user has just pressed still works
        var length = ToUnicodeEx(virtualKeyCode, scanCode, new byte[256], buffer, buffer.Capacity, 0x4, layout);

        return length == 1 ? ToLetter(buffer[0], GetLayoutCulture(layout)) : null;
    }

    /// <summary>
    /// Converts a typed character to an upper case letter.
    /// </summary>
    /// <param name="character">The typed character.</param>
    /// <param name="culture">The culture of the keyboard layout, it decides the upper case form.</param>
    /// <returns>The letter in upper case, or <c>null</c> if the character is not a letter.</returns>
    internal static string? ToLetter(char character, CultureInfo culture) =>
        char.IsLetter(character) ? char.ToUpper(character, culture).ToString() : null;

    /// <summary>
    /// Gets the culture of the language that the keyboard layout belongs to.
    /// </summary>
    private static CultureInfo GetLayoutCulture(IntPtr layout)
    {
        try
        {
            return CultureInfo.GetCultureInfo((int)(layout.ToInt64() & 0xFFFF));
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.InvariantCulture;
        }
    }

    /// <summary>
    /// Creates an INPUT structure representing a keyboard input event with the specified virtual key code.
    /// </summary>
    /// <param name="keyCode">The virtual key code of the key.</param>
    /// <param name="dwFlags">Flags that specify various aspects of function operation.</param>
    /// <param name="extraInfo">An additional value that the keyboard hook receives with the event.</param>
    /// <returns>The created INPUT structure.</returns>
    private static System.Input CreateInput(ushort keyCode, uint dwFlags = InputKeyDown, nint extraInfo = 0)
    {
        var input = new System.Input(InputKeyboard);
        input.u.ki.wVk = keyCode;
        input.u.ki.dwFlags = dwFlags;
        input.u.ki.dwExtraInfo = extraInfo;
        return input;
    }
}