using System;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.InteropServices.Keyboard;

/// <summary>
/// Provides data for the event that occurs when a key is pressed.
/// </summary>
internal class KeyPressedArgs : EventArgs
{
    /// <summary>
    /// Gets the key that was pressed.
    /// </summary>
    public Key KeyPressed { get; }

    /// <summary>
    /// Gets the key event that was pressed.
    /// </summary>
    public KeyboardEvent Event { get; }

    /// <summary>
    /// Initializes a new instance of the KeyPressedArgs class with the specified key.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    /// <param name="keyboardEvent">The key event</param>
    public KeyPressedArgs(Key key, KeyboardEvent keyboardEvent)
    {
        KeyPressed = key;
        Event = keyboardEvent;
    }
}