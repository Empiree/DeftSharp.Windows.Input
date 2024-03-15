using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardSequenceListenerInterceptor : KeyboardInterceptor, IKeyboardSequenceListener
{
    private readonly ObservableCollection<KeyboardSequenceSubscription> _subscriptions;
    private readonly Queue<Key> _pressedKeys;
    public IEnumerable<KeyboardSequenceSubscription> Subscriptions => _subscriptions;

    public KeyboardSequenceListenerInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _pressedKeys = new Queue<Key>();
        _subscriptions = new ObservableCollection<KeyboardSequenceSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    ~KeyboardSequenceListenerInterceptor()
    {
        Dispose();
    }

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    public KeyboardSequenceSubscription Subscribe(IEnumerable<Key> sequence, Action onClick, TimeSpan intervalOfClick)
    {
        var subscription = new KeyboardSequenceSubscription(sequence, onClick, intervalOfClick);
        _subscriptions.Add(subscription);
        return subscription;
    }

    public KeyboardSequenceSubscription SubscribeOnce(IEnumerable<Key> sequence, Action onClick)
    {
        var subscription = new KeyboardSequenceSubscription(sequence, onClick, true);
        _subscriptions.Add(subscription);
        return subscription;
    }

    public void Unsubscribe(Guid id)
    {
        var keyboardSubscribe = _subscriptions.FirstOrDefault(s => s.Id == id);

        if (keyboardSubscribe is null)
            return;

        _subscriptions.Remove(keyboardSubscribe);
    }

    public void UnsubscribeAll()
    {
        if (_subscriptions.Any())
            _subscriptions.Clear();
    }

    protected override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(true, InterceptorType.Listener, () => HandleKeyPressed(this, args));

    private void HandleKeyPressed(object? sender, KeyPressedArgs e)
    {
        if (e.Event == KeyboardEvent.KeyUp)
            return;

        _pressedKeys.Enqueue(e.KeyPressed);

        var pressedSequences = GetPressedSequences().ToArray();

        foreach (var subscription in pressedSequences)
        {
            if (subscription.SingleUse)
                Unsubscribe(subscription.Id);

            subscription.Invoke();
        }
    }

    private IEnumerable<KeyboardSequenceSubscription> GetPressedSequences()
    {
        foreach (var subscription in _subscriptions)
        {
            if (IsSequenceMatch(_pressedKeys, subscription.Sequence.ToArray()))
                yield return subscription;
        }
    }

    protected override bool OnInterceptorUnhookRequested() => !Subscriptions.Any();

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }

    private bool IsSequenceMatch(Queue<Key> pressedKeys, IReadOnlyCollection<Key> sequence)
    {
        if (pressedKeys.Count < sequence.Count)
            return false;

        var inputArray = pressedKeys.ToArray();
        return inputArray.SequenceEqual(sequence);
    }
}