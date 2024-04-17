using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to subscribe to global keyboard events and get current state.
/// </summary>
public sealed class KeyboardListener : IKeyboardListener
{
    private readonly KeyboardListenerInterceptor _listener = new();
    private readonly KeyboardSequenceListenerInterceptor _sequenceListener = new();
    private readonly KeyboardCombinationListenerInterceptor _combinationListener = new();

    /// <summary>
    /// Gets the subscriptions for keys.
    /// </summary>
    public IEnumerable<KeySubscription> Keys => _listener.Subscriptions;

    /// <summary>
    /// Gets the subscriptions for key sequences.
    /// </summary>
    public IEnumerable<KeySequenceSubscription> Sequences => _sequenceListener.Subscriptions;

    /// <summary>
    /// Gets the subscriptions for key combinations.
    /// </summary>
    public IEnumerable<KeyCombinationSubscription> Combinations => _combinationListener.Subscriptions;

    /// <summary>
    /// Checks if the Caps Lock key is active.
    /// </summary>
    public bool IsCapsLockActive => _listener.IsKeyActive(Key.Capital);

    /// <summary>
    /// Checks if the Num Lock key is active.
    /// </summary>
    public bool IsNumLockActive => _listener.IsKeyActive(Key.NumLock);

    /// <summary>
    /// Checks if either of the Shift keys is pressed.
    /// </summary>
    public bool IsShiftPressed => IsKeyPressed(Key.LeftShift) || IsKeyPressed(Key.RightShift);

    /// <summary>
    /// Checks if either of the Ctrl keys is pressed.
    /// </summary>
    public bool IsCtrlPressed => IsKeyPressed(Key.LeftCtrl) || IsKeyPressed(Key.RightCtrl);

    /// <summary>
    /// Checks if either of the Alt keys is pressed.
    /// </summary>
    public bool IsAltPressed => IsKeyPressed(Key.LeftAlt) || IsKeyPressed(Key.RightAlt);

    /// <summary>
    /// Checks if either of the Windows keys is pressed.
    /// </summary>
    public bool IsWinPressed => IsKeyPressed(Key.LWin) || IsKeyPressed(Key.RWin);

    /// <summary>
    /// Checks if the keyboard listener is actively listening for events.
    /// </summary>
    public bool IsListening => Keys.Any() || Sequences.Any() || Combinations.Any();

    /// <summary>
    /// Subscribes to a key with the specified event handler.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action<Key, KeyboardInputEvent> action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, action, interval ?? TimeSpan.Zero, keyboardEvent);
        _listener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <param name="keys">The keyboard keys.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => Subscribe(k, action, interval, keyboardEvent)).ToList();

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param> 
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action<Key, KeyboardInputEvent> action,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, action, keyboardEvent, true);
        _listener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler to be executed once.
    /// </summary>
    /// <param name="keys">The keyboard keys.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> action,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => SubscribeOnce(k, action, keyboardEvent)).ToList();

    /// <summary>
    /// Subscribes to a key sequence with the specified event handler.
    /// </summary>
    /// <param name="sequence">The keyboard key sequence.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <returns>The created subscription.</returns>
    public KeySequenceSubscription SubscribeSequence(IEnumerable<Key> sequence, Action action,
        TimeSpan? interval = null)
    {
        var subscription = new KeySequenceSubscription(sequence, action, interval ?? TimeSpan.Zero);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key sequence with the specified event handler to be executed once.
    /// </summary>
    /// <param name="sequence">The keyboard key sequence.</param>
    /// <param name="action">The work to execute.</param>
    /// <returns>The created subscription.</returns>
    public KeySequenceSubscription SubscribeSequenceOnce(IEnumerable<Key> sequence, Action action)
    {
        var subscription = new KeySequenceSubscription(sequence, action, true);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key combination with the specified event handler.
    /// </summary>
    /// <param name="combination">The keyboard key combination.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <returns>The created subscription.</returns>
    public KeyCombinationSubscription SubscribeCombination(IEnumerable<Key> combination, Action action,
        TimeSpan? interval = null)
    {
        var subscription = new KeyCombinationSubscription(combination, action, interval ?? TimeSpan.Zero);
        _combinationListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key combination with the specified event handler to be executed once.
    /// </summary>
    /// <param name="combination">The keyboard key combination.</param>
    /// <param name="action">The work to execute.</param>
    /// <returns>The created subscription.</returns>
    public KeyCombinationSubscription SubscribeCombinationOnce(IEnumerable<Key> combination, Action action)
    {
        var subscription = new KeyCombinationSubscription(combination, action, true);
        _combinationListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action<Key, KeyboardInputEvent> action,
        TimeSpan? interval = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keys = Enum.GetValues(typeof(Key))
            .OfType<Key>()
            .Distinct()
            .ToList();

        keys.Remove(Key.None);

        return Subscribe(keys, action, interval, keyboardEvent);
    }

    /// <summary>
    /// Subscribes to a key with the specified event handler and optional parameters.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action<Key> action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(key, (k, _) => action(k), interval, keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler and optional parameters.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(key, (_, _) => action(), interval, keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <param name="keys">The keyboard keys.</param>
    /// <param name="action">The work to execute.</param> 
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param> 
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key> action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(keys, (k, _) => action(k), interval, keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <param name="keys">The keyboard keys.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action action,
        TimeSpan? interval = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(keys, (_, _) => action(), interval, keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action<Key> action,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(key, (k, _) => action(k), keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <param name="key">The keyboard key.</param>
    /// <param name="action">The work to execute.</param> 
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action action,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(key, (_, _) => action(), keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler to be executed once.
    /// </summary>
    /// <param name="keys">The keyboard keys.</param>
    /// <param name="action">The work to execute.</param> 
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key> action,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(keys, (k, _) => action(k), keyboardEvent);

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <param name="action">The work to execute.</param> 
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action<Key> action, TimeSpan? interval = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeAll((k, _) => action(k), interval, keyboardEvent);

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <param name="action">The work to execute.</param>
    /// <param name="interval">Frequency of subscription triggering.</param>
    /// <param name="keyboardEvent">The keyboard subscription event which triggers the action.</param>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action action, TimeSpan? interval = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeAll((_, _) => action(), interval, keyboardEvent);

    /// <summary>
    /// Unsubscribes the specified key.
    /// </summary>
    public void Unsubscribe(Key key) => _listener.Unsubscribe(key);

    /// <summary>
    /// Unsubscribes all subscriptions associated with the specified identifier.
    /// </summary>
    public void Unsubscribe(Guid id)
    {
        _listener.Unsubscribe(id);
        _sequenceListener.Unsubscribe(id);
        _combinationListener.Unsubscribe(id);
    }

    /// <summary>
    /// Unsubscribes all subscriptions associated with the specified identifiers.
    /// </summary>
    public void Unsubscribe(IEnumerable<Guid> ids)
    {
        foreach (var id in ids.Distinct())
            Unsubscribe(id);
    }

    /// <summary>
    /// Unsubscribes all subscriptions.
    /// </summary>
    public void Unsubscribe()
    {
        _listener.Unsubscribe();
        _sequenceListener.Unsubscribe();
        _combinationListener.Unsubscribe();
    }

    /// <summary>
    /// Checks if the specified key is currently pressed.
    /// </summary>
    public bool IsKeyPressed(Key key) => _listener.IsKeyPressed(key);

    /// <summary>
    /// Disposes of all resources used by the keyboard listener.
    /// </summary>
    public void Dispose()
    {
        _listener.Dispose();
        _sequenceListener.Dispose();
        _combinationListener.Dispose();
    }
}