using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to listen for various layout letter key presses and trigger corresponding actions.
/// </summary>
public sealed class LetterListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly HashSet<Guid> _subscriptionIds;

    private readonly Dictionary<KeyboardLayoutType, LetterButton[]> _layouts = KeyboardLayouts.Layouts;

    private readonly LetterButton[] _letKeys;

    /// <summary>
    /// Checks if the letter listener is actively listening for events.
    /// </summary>
    public bool IsListening => _subscriptionIds.Count != 0;

    /// <summary>
    /// Gets letter keys
    /// </summary>
    public IEnumerable<Key> Keys => _letKeys.Select(letter => letter.Key);

    /// <summary>
    /// Initializes a new instance of the <see cref="LetterListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    /// <param name="keyboardLayoutType">The keyboard layout to be used. By default, it is QWERTY</param>
    public LetterListener(KeyboardListener keyboardListener, KeyboardLayoutType keyboardLayoutType = KeyboardLayoutType.Qwerty)
    {
        _subscriptionIds = new HashSet<Guid>();
        _keyboardListener = keyboardListener;
        _letKeys = _layouts[keyboardLayoutType];
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