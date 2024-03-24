using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Shared.Exceptions;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardCombinationListenerInterceptor : KeyboardInterceptor
{
    private const int MinimumCombinationLength = 2;
    private const int MaximumCombinationLength = 10;

    private readonly HashSet<Key> _heldKeys;
    private readonly ObservableCollection<KeyCombinationSubscription> _subscriptions;
    public IEnumerable<KeyCombinationSubscription> Subscriptions => _subscriptions;

    public KeyboardCombinationListenerInterceptor()
        : base(InterceptorType.Listener)
    {
        _heldKeys = new HashSet<Key>();
        _subscriptions = new ObservableCollection<KeyCombinationSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    public void Subscribe(KeyCombinationSubscription subscription)
    {
        if (Subscriptions.Any(sub => sub.Id.Equals(subscription.Id)))
            return;

        CheckCombinationLength(subscription.Combination);

        _subscriptions.Add(subscription);
    }

    public void UnsubscribeAll() => _subscriptions.Clear();

    public void Unsubscribe(Guid id)
    {
        var keyboardSubscribe =
            _subscriptions.FirstOrDefault(sub => sub.Id.Equals(id));

        if (keyboardSubscribe is null)
            return;

        _subscriptions.Remove(keyboardSubscribe);
    }

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !Subscriptions.Any();
    protected override bool IsInputAllowed(KeyPressedArgs args) => true;

    protected override void OnInputSuccess(KeyPressedArgs args)
    {
        if (args.Event is KeyboardEvent.KeyUp)
        {
            _heldKeys.Remove(args.KeyPressed);
            return;
        }

        _heldKeys.Add(args.KeyPressed);

        var matched = GetMatchedCombinations().ToArray();

        if (matched.Length == 0)
            return;

        foreach (var subscription in matched)
        {
            if (subscription.SingleUse)
                Unsubscribe(subscription.Id);

            subscription.Invoke();
        }
    }

    private IEnumerable<KeyCombinationSubscription> GetMatchedCombinations() =>
        _subscriptions.Where(subscription => subscription.Combination.All(key => _heldKeys.Contains(key)));

    private void CheckCombinationLength(IEnumerable<Key> combination)
    {
        var keyCombination = combination.ToArray();

        switch (keyCombination.Length)
        {
            case < MinimumCombinationLength:
                throw new KeyCombinationLengthException(
                    $"The combination cannot be the size of {keyCombination.Length} elements. " +
                    $"The minimum size is {MinimumCombinationLength} elements.");
            case > MaximumCombinationLength:
                throw new KeyCombinationLengthException(
                    $"The combination cannot be larger than {MaximumCombinationLength} elements.");
        }
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }
}