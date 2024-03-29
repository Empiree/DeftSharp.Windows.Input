﻿using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

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
    /// Gets the key event that was pressed.
    /// </summary>
    public KeyboardInputEvent Event { get; }

    /// <summary>
    /// Initializes a new instance of the KeyPressedArgs class with the specified key.
    /// </summary>
    /// <param name="key">The key that was pressed.</param>
    /// <param name="keyboardEvent">The key event</param>
    public KeyPressedArgs(Key key, KeyboardInputEvent keyboardEvent)
    {
        KeyPressed = key;
        Event = keyboardEvent;
    }
}