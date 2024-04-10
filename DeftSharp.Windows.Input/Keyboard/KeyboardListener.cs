using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides the ability to subscribe to various keyboard events and get current state.
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
    /// Subscribes to a key with the specified event handler and optional parameters.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);
        _listener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => Subscribe(k, onClick, intervalOfClick, keyboardEvent)).ToList();

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, onClick, keyboardEvent, true);
        _listener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => SubscribeOnce(k, onClick, keyboardEvent)).ToList();

    /// <summary>
    /// Subscribes to a key sequence with the specified event handler.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySequenceSubscription SubscribeSequence(IEnumerable<Key> sequence, Action onClick,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new KeySequenceSubscription(sequence, onClick, intervalOfClick ?? TimeSpan.Zero);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key sequence with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySequenceSubscription SubscribeSequenceOnce(IEnumerable<Key> sequence, Action onClick)
    {
        var subscription = new KeySequenceSubscription(sequence, onClick, true);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key combination with the specified event handler.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeyCombinationSubscription SubscribeCombination(IEnumerable<Key> combination, Action onClick,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new KeyCombinationSubscription(combination, onClick, intervalOfClick ?? TimeSpan.Zero);
        _combinationListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to a key combination with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeyCombinationSubscription SubscribeCombinationOnce(IEnumerable<Key> combination, Action onClick)
    {
        var subscription = new KeyCombinationSubscription(combination, onClick, true);
        _combinationListener.Subscribe(subscription);
        return subscription;
    }

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keys = Enum.GetValues(typeof(Key))
            .OfType<Key>()
            .Distinct()
            .ToList();

        keys.Remove(Key.None);

        return Subscribe(keys, onClick, intervalOfClick, keyboardEvent);
    }

    /// <summary>
    /// Subscribes to a key with the specified event handler and optional parameters.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(key, (k, _) => onClick(k), intervalOfClick, keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler and optional parameters.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription Subscribe(Key key, Action onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(key, (_, _) => onClick(), intervalOfClick, keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(keys, (k, _) => onClick(k), intervalOfClick, keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(keys, (_, _) => onClick(), intervalOfClick, keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(key, (k, _) => onClick(k), keyboardEvent);

    /// <summary>
    /// Subscribes to a key with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The created subscription.</returns>
    public KeySubscription SubscribeOnce(Key key, Action onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(key, (_, _) => onClick(), keyboardEvent);

    /// <summary>
    /// Subscribes to multiple keys with the specified event handler to be executed once.
    /// </summary>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(keys, (k, _) => onClick(k), keyboardEvent);

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action<Key> onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeAll((k, _) => onClick(k), intervalOfClick, keyboardEvent);

    /// <summary>
    /// Subscribes to all possible enum values of type <see cref="Key"/>.
    /// </summary>
    /// <remarks>Except <b>Key.None</b></remarks>
    /// <returns>The collection of created subscriptions.</returns>
    public IEnumerable<KeySubscription> SubscribeAll(Action onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeAll((_, _) => onClick(), intervalOfClick, keyboardEvent);

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
    /// Unsubscribes all subscriptions.
    /// </summary>
    public void UnsubscribeAll()
    {
        _listener.UnsubscribeAll();
        _sequenceListener.UnsubscribeAll();
        _combinationListener.UnsubscribeAll();
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