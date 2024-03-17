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

internal sealed class KeyboardCombinationListenerInterceptor : KeyboardInterceptor, IKeyboardCombinationListener
{
    private readonly List<Key> _heldKeys;
    private readonly ObservableCollection<KeyboardCombinationSubscription> _subscriptions;
    public IEnumerable<KeyboardCombinationSubscription> Subscriptions => _subscriptions;
    
    public KeyboardCombinationListenerInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _heldKeys = new List<Key>();
        _subscriptions = new ObservableCollection<KeyboardCombinationSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    ~KeyboardCombinationListenerInterceptor() => Dispose();
    
    public void Subscribe(KeyboardCombinationSubscription subscription)
    {
        if (Subscriptions.Any(sub => sub.Id.Equals(subscription.Id)))
            return;

        _subscriptions.Add(subscription);
    }

    public void UnsubscribeAll()
    {
        if (_subscriptions.Any())
            _subscriptions.Clear();
    }

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
    
    protected override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(true, InterceptorType.Listener, () => HandleKeyPressed(args));

    protected override bool OnInterceptorUnhookRequested() => !Subscriptions.Any();
    
    private IEnumerable<KeyboardCombinationSubscription> GetMatchedCombinations() =>
        _subscriptions.Where(subscription => subscription.Combination.All(key => _heldKeys.Contains(key)));

    private void HandleKeyPressed(KeyPressedArgs args)
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
        
        _heldKeys.Clear();
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Hook();

        if (!_subscriptions.Any())
            Unhook();
    }
}