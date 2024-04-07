using System;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides functionality to listen for numpad key presses and trigger corresponding actions.
/// </summary>
public sealed class NumpadListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly NumButton[] _numKeys =
    {
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="NumpadListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    public NumpadListener(KeyboardListener keyboardListener)
    {
        _keyboardListener = keyboardListener;
    }

    /// <summary>
    /// Subscribes to numpad key presses and triggers the specified action when a numpad key is pressed.
    /// </summary>
    /// <param name="onNumClick">The action to execute when a numpad key is pressed. It takes an integer argument representing the pressed number.</param>
    public void Subscribe(Action<int> onNumClick)
    {
        var keys = _numKeys.Select(n => n.Key);

        _keyboardListener.Subscribe(keys, key =>
        {
            var numKey = _numKeys.FirstOrDefault(n => n.Key == key);

            if (numKey is null)
                throw new NotImplementedException($"{key} was not implemented!");

            onNumClick(numKey.Number);
        });
    }

    /// <summary>
    /// Unsubscribes from listening to numpad key presses.
    /// </summary>
    public void Unsubscribe()
    {
        var keys = _numKeys.Select(n => n.Key);
        foreach (var key in keys)
            _keyboardListener.Unsubscribe(key);
    }
}