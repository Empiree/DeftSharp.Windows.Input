using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Keyboard;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardListenerInterceptor : IKeyboardListenerInterceptor
{
    private readonly ObservableCollection<KeyboardSubscription> _subscriptions;
    private readonly IKeyboardInterceptor _keyboardInterceptor;

    public event Func<bool>? UnhookRequested;
    public IEnumerable<KeyboardSubscription> Subscriptions => _subscriptions;

    public KeyboardListenerInterceptor()
    {
        _subscriptions = new ObservableCollection<KeyboardSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;

        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.KeyProcessing += OnKeyProcessing;
        _keyboardInterceptor.UnhookRequested += OnUnhookRequested;
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

    public void Hook() => _keyboardInterceptor.Hook();
    public void Unhook() => _keyboardInterceptor.Unhook();

    public void Dispose()
    {
        UnsubscribeAll();
        _keyboardInterceptor.KeyProcessing -= OnKeyProcessing;
        _keyboardInterceptor.UnhookRequested -= OnUnhookRequested;
    }

    private bool OnUnhookRequested() => (UnhookRequested?.Invoke() ?? true) && !Subscriptions.Any();

    private PipelineInterceptorOperation OnKeyProcessing(KeyPressedArgs args) =>
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