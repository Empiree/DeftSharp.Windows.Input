using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the arguments for keyboard input events.
/// </summary>
public class KeyboardInputArgs : EventArgs
{
    /// <summary>
    /// Gets the key that was pressed.
    /// </summary>
    public Key KeyPressed { get; }

    /// <summary>
    /// Gets the key event that was pressed.
    /// </summary>
    public KeyboardInputEvent Event { get; }

    /// <summary>
    /// Initializes a new instance of the KeyboardInputArgs class with the specified key.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    /// <param name="keyboardEvent">The key event</param>
    public KeyboardInputArgs(Key key, KeyboardInputEvent keyboardEvent)
    {
        KeyPressed = key;
        Event = keyboardEvent;
    }
}