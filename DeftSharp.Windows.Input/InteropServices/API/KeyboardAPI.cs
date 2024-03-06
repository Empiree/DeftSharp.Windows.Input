using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Exceptions;
using static DeftSharp.Windows.Input.InteropServices.API.InputMessages;

namespace DeftSharp.Windows.Input.InteropServices.API;

/// <summary>
/// Provides methods for simulating keyboard input using Windows API.
/// </summary>
internal static class KeyboardAPI
{
    /// <summary>
    /// Presses the specified key.
    /// </summary>
    /// <param name="key">The key to press.</param>
    /// <exception cref="KeyboardPressException">Thrown if the key press simulation fails.</exception>
    internal static void PressButton(Key key)
    {
        var keyCode = (byte)KeyInterop.VirtualKeyFromKey(key);
        var result = SimulateKeyPress(keyCode);
        if (!result)
            throw new KeyboardPressException(key);
    }
    
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

        input.u.ki.dwFlags = InputKeyup;
        result = SendInput(input);
        return result;
    }

    /// <summary>
    /// Sends an array of input events to the system input queue.
    /// </summary>
    /// <param name="inputs">An array of INPUT structures representing the input events to send.</param>
    /// <returns>True if the input events were successfully sent; otherwise, false.</returns>
    private static bool SendInput(Structures.Input[] inputs) =>
        WinAPI.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(inputs[0])) != 0;

    /// <summary>
    /// Sends an input event to the system input queue.
    /// </summary>
    /// <param name="input">The INPUT structure representing the input event to send.</param>
    /// <returns>True if the input event was successfully sent; otherwise, false.</returns>
    private static bool SendInput(Structures.Input input) =>
        SendInput(new[] { input });

    /// <summary>
    /// Creates an INPUT structure representing a keyboard input event with the specified virtual key code.
    /// </summary>
    /// <param name="keyCode">The virtual key code of the key.</param>
    /// <returns>The created INPUT structure.</returns>
    private static Structures.Input CreateInput(ushort keyCode)
    {
        var input = new Structures.Input(InputKeyboard);
        input.u.ki.wVk = keyCode;
        input.u.ki.dwFlags = InputKeydown;
        return input;
    }
}