using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardListenerInterceptor : KeyboardInterceptor
{
    private readonly ObservableCollection<KeySubscription> _subscriptions;
    public IEnumerable<KeySubscription> Subscriptions => _subscriptions;

    public KeyboardListenerInterceptor()
        : base(InterceptorType.Listener)
    {
        _subscriptions = new ObservableCollection<KeySubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    public void Subscribe(KeySubscription subscription)
    {
        if (Subscriptions.Any(sub => sub.Id.Equals(subscription.Id)))
            return;

        _subscriptions.Add(subscription);
    }

    public void Unsubscribe(Key key)
    {
        var subscriptions = _subscriptions.Where(e => e.Key.Equals(key)).ToArray();

        foreach (var buttonSubscription in subscriptions)
            _subscriptions.Remove(buttonSubscription);
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

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    public bool IsKeyActive(Key key) => Keyboard.IsKeyActive(key);
    public bool IsKeyPressed(Key key) => Keyboard.IsKeyPressed(key);

    internal override bool OnPipelineUnhookRequested() => !Subscriptions.Any();
    protected override bool IsInputAllowed(KeyPressedArgs args) => true;

    protected override void OnInputSuccess(KeyPressedArgs args)
    {
        var events = args.Event.ToKeyboardEvents();
        
        var keyboardEvents = _subscriptions
            .Where(s => events.Contains(s.Event) && s.Key.Equals(args.KeyPressed))
            .ToList();

        foreach (var keyboardEvent in keyboardEvents)
        {
            if (keyboardEvent.SingleUse)
                Unsubscribe(keyboardEvent.Id);

            keyboardEvent.Invoke(args.Event);
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