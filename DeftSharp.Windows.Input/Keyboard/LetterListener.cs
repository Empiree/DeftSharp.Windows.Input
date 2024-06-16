using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to listen for letter key presses and trigger corresponding actions.
/// </summary>
public sealed class LetterListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly HashSet<Guid> _subscriptionIds;

    private readonly LetterButton[] _letKeys =
    {
        // QWERTY Keyboard (English)
        new LetterButton(Key.A, "A"),
        new LetterButton(Key.B, "B"),
        new LetterButton(Key.C, "C"),
        new LetterButton(Key.D, "D"),
        new LetterButton(Key.E, "E"),
        new LetterButton(Key.F, "F"),
        new LetterButton(Key.G, "G"),
        new LetterButton(Key.H, "H"),
        new LetterButton(Key.I, "I"),
        new LetterButton(Key.J, "J"),
        new LetterButton(Key.K, "K"),
        new LetterButton(Key.L, "L"),
        new LetterButton(Key.M, "M"),
        new LetterButton(Key.N, "N"),
        new LetterButton(Key.O, "O"),
        new LetterButton(Key.P, "P"),
        new LetterButton(Key.Q, "Q"),
        new LetterButton(Key.R, "R"),
        new LetterButton(Key.S, "S"),
        new LetterButton(Key.T, "T"),
        new LetterButton(Key.U, "U"),
        new LetterButton(Key.V, "V"),
        new LetterButton(Key.W, "W"),
        new LetterButton(Key.X, "X"),
        new LetterButton(Key.Y, "Y"),
        new LetterButton(Key.Z, "Z"),

        //// QWERTY Keyboard (Spanish)
        new LetterButton((Key)192, "Ñ"), // Adding letter Ñ

    };


    /// <summary>
    /// Checks if the letter listener is actively listening for events.
    /// </summary>
    public bool IsListening => _subscriptionIds.Any();

    /// <summary>
    /// Initializes a new instance of the <see cref="LetterListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    public LetterListener(KeyboardListener keyboardListener)
    {
        _subscriptionIds = new HashSet<Guid>();
        _keyboardListener = keyboardListener;
    }

    /// <summary>
    /// Subscribes to numpad key presses and triggers the specified action when a numpad key is pressed.
    /// </summary>
    /// <param name="onLetterClick">The action to execute when a letter key is pressed. It takes a string argument representing the pressed letter.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    public void Subscribe(Action<string> onLetterClick, TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keys = _letKeys.Select(n => n.Key);

        var subscriptions = _keyboardListener.Subscribe(keys, key =>
        {
            var letterKey = _letKeys.FirstOrDefault(n => n.Key == key);

            if (letterKey is null)
                throw new NotImplementedException($"{key} was not implemented!");

            onLetterClick(letterKey.Letter);
        },
        interval,
        keyboardEvent);

        foreach (var subscription in subscriptions)
            _subscriptionIds.Add(subscription.Id);
    }

    /// <summary>
    /// Unsubscribes from listening to letter key presses.
    /// </summary>
    public void Unsubscribe()
    {
        _keyboardListener.Unsubscribe(_subscriptionIds);
        _subscriptionIds.Clear();
    }
}

