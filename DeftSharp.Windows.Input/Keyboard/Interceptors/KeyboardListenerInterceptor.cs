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

internal sealed class KeyboardListenerInterceptor : KeyboardInterceptor, IKeyboardListener
{
    private readonly ObservableCollection<KeyboardSubscription> _subscriptions;
    public IEnumerable<KeyboardSubscription> Subscriptions => _subscriptions;

    public KeyboardListenerInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _subscriptions = new ObservableCollection<KeyboardSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    ~KeyboardListenerInterceptor() => Dispose();

    public void Subscribe(KeyboardSubscription subscription)
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

    public void Unsubscribe(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
            Unsubscribe(key);
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

    protected override bool OnInterceptorUnhookRequested() => !Subscriptions.Any();

    protected override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(true, InterceptorType.Listener, () => HandleKeyPressed(args));

    private void HandleKeyPressed(KeyPressedArgs args)
    {
        var keyboardEvents =
            _subscriptions
                .Where(s => s.Key.Equals(args.KeyPressed) && s.Event == args.Event)
                .ToArray();

        foreach (var keyboardEvent in keyboardEvents)
        {
            if (keyboardEvent.SingleUse)
                Unsubscribe(keyboardEvent.Id);

            keyboardEvent.Invoke();
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