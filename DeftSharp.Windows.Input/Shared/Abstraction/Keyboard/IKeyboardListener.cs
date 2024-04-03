using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardListener : IDisposable
{
    public IEnumerable<KeySubscription> Keys { get; }
    public IEnumerable<KeySequenceSubscription> Sequences { get; }
    public IEnumerable<KeyCombinationSubscription> Combinations { get; }

    bool IsCapsLockActive { get; }
    bool IsNumLockActive { get; }
    bool IsShiftPressed { get; }
    bool IsCtrlPressed { get; }
    bool IsAltPressed { get; }
    bool IsWinPressed { get; }
    bool IsListening { get; }

    public KeySubscription Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    KeySubscription Subscribe(Key key, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);


    public IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    IEnumerable<KeySubscription> Subscribe(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);


    public IEnumerable<KeySubscription> SubscribeAll(Action<Key> onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    IEnumerable<KeySubscription> SubscribeAll(Action<Key, KeyboardInputEvent> onClick, TimeSpan? intervalOfClick = null,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);


    public KeySubscription SubscribeOnce(Key key, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    KeySubscription SubscribeOnce(Key key, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    public IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);

    IEnumerable<KeySubscription> SubscribeOnce(IEnumerable<Key> keys, Action<Key, KeyboardInputEvent> onClick,
        KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown);


    KeySequenceSubscription SubscribeSequence(IEnumerable<Key> sequence, Action onClick,
        TimeSpan? intervalOfClick = null);

    KeySequenceSubscription SubscribeSequenceOnce(IEnumerable<Key> sequence, Action onClick);


    KeyCombinationSubscription SubscribeCombination(IEnumerable<Key> combination, Action onClick,
        TimeSpan? intervalOfClick = null);

    KeyCombinationSubscription SubscribeCombinationOnce(IEnumerable<Key> combination, Action onClick);

    bool IsKeyPressed(Key key);

    void Unsubscribe(Key key);
    void Unsubscribe(Guid id);
    void UnsubscribeAll();
}