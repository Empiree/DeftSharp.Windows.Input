using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Keyboard.InteropServices;

/// <summary>
/// Provides data for the event that occurs when a key is pressed.
/// </summary>
public class KeyPressedArgs : EventArgs
{
    /// <summary>
    /// Gets the key that was pressed.
    /// </summary>
    public Key KeyPressed { get; }

    /// <summary>
    /// Initializes a new instance of the KeyPressedArgs class with the specified key.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    public KeyPressedArgs(Key key)
    {
        KeyPressed = key;
    }
}