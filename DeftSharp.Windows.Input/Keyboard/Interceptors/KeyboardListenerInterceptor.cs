using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;
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

    ~KeyboardListenerInterceptor()
    {
        Dispose();
    }

    public void Subscribe(Key key, Action<Key> onClick, TimeSpan intervalOfClick, KeyboardEvent keyboardEvent)
    {
        var keyboardSubscription =
            new KeyboardSubscription(key, onClick, intervalOfClick, keyboardEvent);
        _subscriptions.Add(keyboardSubscription);
    }

    public void Subscribe(IEnumerable<Key> keys, Action<Key> onClick, TimeSpan intervalOfClick,
        KeyboardEvent keyboardEvent)
    {
        foreach (var key in keys)
            Subscribe(key, onClick, intervalOfClick, keyboardEvent);
    }

    public void SubscribeOnce(Key key, Action<Key> onClick, KeyboardEvent keyboardEvent) =>
        _subscriptions.Add(new KeyboardSubscription(key, onClick, keyboardEvent, true));

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

    public override void Dispose()
    {
        UnsubscribeAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !Subscriptions.Any();

    protected override InterceptorResponse OnInterceptorPipelineRequested(KeyPressedArgs args) =>
        new(true, () => HandleKeyPressed(this, args));

    private void HandleKeyPressed(object? sender, KeyPressedArgs e)
    {
        var keyboardEvents =
            _subscriptions.Where(s => s.Key.Equals(e.KeyPressed) && s.Event == e.Event).ToArray();
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