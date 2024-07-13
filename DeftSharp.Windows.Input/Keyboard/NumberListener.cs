﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to listen for number key presses and trigger corresponding actions.
/// </summary>
public sealed class NumberListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly HashSet<Guid> _subscriptionIds;

    private readonly IEnumerable<NumButton> _numKeys = KeyboardButtonSet.NumberButtons;

    /// <summary>
    /// Checks if the number listener is actively listening for events.
    /// </summary>
    public bool IsListening => _subscriptionIds.Any();

    /// <summary>
    /// Initializes a new instance of the <see cref="NumberListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    public NumberListener(KeyboardListener keyboardListener)
    {
        _subscriptionIds = new HashSet<Guid>();
        _keyboardListener = keyboardListener;
    }

    /// <summary>
    /// Subscribes to number key presses and triggers the specified action when a number key is pressed.
    /// </summary>
    /// <param name="onNumClick">The action to execute when a number key is pressed. It takes an integer argument representing the pressed number.</param>
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
    /// Unsubscribes from listening to number key presses.
    /// </summary>
    public void Unsubscribe()
    {
        _keyboardListener.Unsubscribe(_subscriptionIds);
        _subscriptionIds.Clear();
    }
}