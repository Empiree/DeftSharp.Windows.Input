using System;
using System.Collections.Generic;
using System.Linq;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to listen for numpad key presses and trigger corresponding actions.
/// </summary>
public sealed class NumpadListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly HashSet<Guid> _subscriptionIds;

    private readonly IEnumerable<NumButton> _numKeys = KeyboardButtonSet.NumpadButtons;

    /// <summary>
    /// Checks if the numpad listener is actively listening for events.
    /// </summary>
    public bool IsListening => _subscriptionIds.Any();

    /// <summary>
    /// Initializes a new instance of the <see cref="NumpadListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    public NumpadListener(KeyboardListener keyboardListener)
    {
        _subscriptionIds = new HashSet<Guid>();
        _keyboardListener = keyboardListener;
    }

    /// <summary>
    /// Subscribes to numpad key presses and triggers the specified action when a numpad key is pressed.
    /// </summary>
    /// <param name="onNumClick">The action to execute when a numpad key is pressed. It takes an integer argument representing the pressed number.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    public void Subscribe(Action<int> onNumClick, TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keys = _numKeys.Select(n => n.Key);

        var subscriptions = _keyboardListener.Subscribe(keys, key =>
        {
            var numKey = _numKeys.FirstOrDefault(n => n.Key == key);

            if (numKey is null)
                throw new NotImplementedException($"{key} was not implemented!");

            onNumClick(numKey.Number);
        },
        interval,
        keyboardEvent);

        foreach (var subscription in subscriptions)
            _subscriptionIds.Add(subscription.Id);
    }

    /// <summary>
    /// Unsubscribes from listening to numpad key presses.
    /// </summary>
    public void Unsubscribe()
    {
        _keyboardListener.Unsubscribe(_subscriptionIds);
        _subscriptionIds.Clear();
    }
}