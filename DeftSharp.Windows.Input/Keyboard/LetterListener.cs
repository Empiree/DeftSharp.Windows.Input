using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Native;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to listen for letter key presses in the active keyboard layout and trigger corresponding actions.
/// </summary>
public sealed class LetterListener
{
    // Keys that type a letter in at least one keyboard layout. Besides A-Z, some layouts put letters
    // on the number row (é in French, č in Czech) and on the punctuation keys (ж in Russian, ñ in Spanish).
    private static readonly ReadOnlyCollection<Key> LetterKeys =
        Enumerable.Range((int)Key.A, 26)
            .Concat(Enumerable.Range((int)Key.D0, 10))
            .Select(key => (Key)key)
            .Concat(new[]
            {
                Key.OemSemicolon, Key.OemPlus, Key.OemComma, Key.OemMinus, Key.OemPeriod, Key.OemQuestion,
                Key.OemTilde, Key.OemOpenBrackets, Key.OemPipe, Key.OemCloseBrackets, Key.OemQuotes, Key.Oem8,
                Key.OemBackslash
            })
            .ToList()
            .AsReadOnly();

    private readonly KeyboardListener _keyboardListener;

    private readonly HashSet<Guid> _subscriptionIds;

    private readonly Func<Key, string?> _getLetter;

    /// <summary>
    /// Checks if the letter listener is actively listening for events.
    /// </summary>
    public bool IsListening => _subscriptionIds.Count != 0;

    /// <summary>
    /// Gets the keys that can type a letter.
    /// </summary>
    public IEnumerable<Key> Keys => LetterKeys;

    /// <summary>
    /// Initializes a new instance of the <see cref="LetterListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    public LetterListener(KeyboardListener keyboardListener)
        : this(keyboardListener, KeyboardAPI.GetLetter)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LetterListener"/> class.
    /// </summary>
    /// <param name="keyboardListener">The keyboard listener instance to use for listening to key presses.</param>
    /// <param name="keyboardLayoutType">Not used. The letters come from the active keyboard layout in Windows.</param>
    [Obsolete("Windows already applies the active keyboard layout, so the layout type is ignored. Use the constructor without it.")]
    public LetterListener(KeyboardListener keyboardListener, KeyboardLayoutType keyboardLayoutType)
        : this(keyboardListener)
    {
    }

    internal LetterListener(KeyboardListener keyboardListener, Func<Key, string?> getLetter)
    {
        _subscriptionIds = new HashSet<Guid>();
        _keyboardListener = keyboardListener;
        _getLetter = getLetter;
    }

    /// <summary>
    /// Subscribes to letter key presses and triggers the specified action when a key types a letter.
    /// </summary>
    /// <param name="onLetterClick">The action to execute when a letter key is pressed. It takes the letter in upper case, as the active keyboard layout types it.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    public void Subscribe(Action<string> onLetterClick, TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscriptions = _keyboardListener.Subscribe(LetterKeys, key => OnKeyPressed(key, onLetterClick),
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

    internal void OnKeyPressed(Key key, Action<string> onLetterClick)
    {
        var letter = _getLetter(key);

        if (letter is not null)
            onLetterClick(letter);
    }
}
