using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardListener : IKeyboardListener
{
    private readonly KeyboardListenerInterceptor _listener = new();
    private readonly KeyboardSequenceListenerInterceptor _sequenceListener = new();
    private readonly KeyboardCombinationListenerInterceptor _combinationListener = new();
    public IEnumerable<KeySubscription> Keys => _listener.Subscriptions;
    public IEnumerable<KeySequenceSubscription> Sequences => _sequenceListener.Subscriptions;
    public IEnumerable<KeyCombinationSubscription> Combinations => _combinationListener.Subscriptions;

    public bool IsCapsLockActive => _listener.IsKeyActive(Key.Capital);
    public bool IsNumLockActive => _listener.IsKeyActive(Key.NumLock);
    public bool IsShiftPressed => IsKeyPressed(Key.LeftShift) || IsKeyPressed(Key.RightShift);
    public bool IsCtrlPressed => IsKeyPressed(Key.LeftCtrl) || IsKeyPressed(Key.RightCtrl);
    public bool IsAltPressed => IsKeyPressed(Key.LeftAlt) || IsKeyPressed(Key.RightAlt);
    public bool IsWinPressed => IsKeyPressed(Key.LWin) || IsKeyPressed(Key.RWin);
    public bool IsListening => Keys.Any() || Sequences.Any() || Combinations.Any();

    public KeySubscription Subscribe(Key key, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);
        _listener.Subscribe(subscription);
        return subscription;
    }

    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => Subscribe(k, onClick, intervalOfClick, keyboardEvent)).ToList();

    public KeySubscription SubscribeOnce(Key key, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var subscription = new KeySubscription(key, onClick, keyboardEvent, true);
        _listener.Subscribe(subscription);
        return subscription;
    }

    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        keys.Select(k => SubscribeOnce(k, onClick, keyboardEvent)).ToList();

    public KeySequenceSubscription SubscribeSequence(IEnumerable<Key> sequence, Action onClick,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new KeySequenceSubscription(sequence, onClick, intervalOfClick ?? TimeSpan.Zero);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    public KeySequenceSubscription SubscribeSequenceOnce(IEnumerable<Key> sequence, Action onClick)
    {
        var subscription = new KeySequenceSubscription(sequence, onClick, true);
        _sequenceListener.Subscribe(subscription);
        return subscription;
    }

    public KeyCombinationSubscription SubscribeCombination(IEnumerable<Key> combination, Action onClick,
        TimeSpan? intervalOfClick = null)
    {
        var subscription = new KeyCombinationSubscription(combination, onClick, intervalOfClick ?? TimeSpan.Zero);
        _combinationListener.Subscribe(subscription);
        return subscription;
    }

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
    public IEnumerable<KeySubscription> SubscribeAll(Action<Key, KeyboardInputEvent> onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keys = Enum.GetValues(typeof(Key))
            .OfType<Key>()
            .ToList();

        keys.Remove(Key.None);

        return Subscribe(keys, onClick, intervalOfClick, keyboardEvent);
    }
    
    public KeySubscription Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(key, (k, _) => onClick(k), intervalOfClick, keyboardEvent);

    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        Subscribe(keys, (k, _) => onClick(k), intervalOfClick, keyboardEvent);

    public KeySubscription SubscribeOnce(Key key, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(key, (k, _) => onClick(k), keyboardEvent);

    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeOnce(keys, (k, _) => onClick(k), keyboardEvent);

    public IEnumerable<KeySubscription> SubscribeAll(Action<Key> onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        SubscribeAll((k, _) => onClick(k), intervalOfClick, keyboardEvent);
        

    public void Unsubscribe(Key key) => _listener.Unsubscribe(key);

    public void Unsubscribe(Guid id)
    {
        _listener.Unsubscribe(id);
        _sequenceListener.Unsubscribe(id);
        _combinationListener.Unsubscribe(id);
    }

    public void UnsubscribeAll()
    {
        _listener.UnsubscribeAll();
        _sequenceListener.UnsubscribeAll();
        _combinationListener.UnsubscribeAll();
    }

    public bool IsKeyPressed(Key key) => _listener.IsKeyPressed(key);

    public void Dispose()
    {
        _listener.Dispose();
        _sequenceListener.Dispose();
        _combinationListener.Dispose();
    }
}