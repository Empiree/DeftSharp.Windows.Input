using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardSequenceListenerInterceptor : KeyboardInterceptor
{
    private const int MinimumSequenceLength = 2;
    private const int MaximumSequenceLength = 10;

    private readonly ObservableCollection<KeySequenceSubscription> _subscriptions;
    private readonly Queue<Key> _pressedKeys;
    public IEnumerable<KeySequenceSubscription> Subscriptions => _subscriptions;

    public KeyboardSequenceListenerInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _pressedKeys = new Queue<Key>();
        _subscriptions = new ObservableCollection<KeySequenceSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    public void Subscribe(KeySequenceSubscription subscription)
    {
        if (Subscriptions.Any(sub => sub.Id.Equals(subscription.Id)))
            return;

        CheckSequenceLength(subscription.Sequence);

        _subscriptions.Add(subscription);
    }

    public void Unsubscribe(Guid id)
    {
        var keyboardSubscribe =
            _subscriptions.FirstOrDefault(sub => sub.Id.Equals(id));

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
        new(true, new InterceptorInfo(Name, InterceptorType.Listener), () => HandleKeyPressed(args));

    protected override bool OnPipelineUnhookRequested() => !Subscriptions.Any();

    private void HandleKeyPressed(KeyPressedArgs args)
    {
        if (args.Event == KeyboardEvent.KeyUp)
            return;

        Enqueue(args.KeyPressed);

        var matched = GetMatchedSequences().ToArray();

        foreach (var sequence in matched)
        {
            if (sequence.SingleUse)
                Unsubscribe(sequence.Id);

            sequence.Invoke();
        }
    }

    private void Enqueue(Key key)
    {
        if (_pressedKeys.Count == MaximumSequenceLength)
            _pressedKeys.Dequeue();

        _pressedKeys.Enqueue(key);
    }

    private IEnumerable<KeySequenceSubscription> GetMatchedSequences() =>
        _subscriptions.Where(subscription => IsSequenceMatch(subscription.Sequence.ToArray()));

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }

    private void CheckSequenceLength(IEnumerable<Key> sequence)
    {
        var keySequence = sequence.ToArray();

        switch (keySequence.Length)
        {
            case < MinimumSequenceLength:
                throw new KeySequenceLengthException(
                    $"A sequence cannot be the size of {keySequence.Length} elements. " +
                    $"The minimum size is {MinimumSequenceLength} elements.");
            case > MaximumSequenceLength:
                throw new KeySequenceLengthException(
                    $"The sequence cannot be larger than {MaximumSequenceLength} elements.");
        }
    }

    private bool IsSequenceMatch(IReadOnlyCollection<Key> sequence)
    {
        if (_pressedKeys.Count < sequence.Count)
            return false;

        var inputArray = _pressedKeys.TakeLast(sequence.Count);
        return inputArray.SequenceEqual(sequence);
    }
}