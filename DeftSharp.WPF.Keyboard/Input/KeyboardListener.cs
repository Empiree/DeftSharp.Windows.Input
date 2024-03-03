using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Keyboard.InteropServices;
using DeftSharp.Windows.Keyboard.Shared.Models;

namespace DeftSharp.Windows.Keyboard.Input;

public sealed class KeyboardListener : WindowsKeyboardListener, IDisposable
{
    private readonly ObservableCollection<KeyboardButtonSubscription> _subscriptions;

    public bool IsListening { get; private set; }

    public KeyboardListener()
    {
        _subscriptions = new ObservableCollection<KeyboardButtonSubscription>();

        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;

        KeyPressed += OnKeyPressed;
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && !IsListening)
            Register();

        if (_subscriptions.Count == 0)
            Unregister();
    }

    public void Subscribe(Key key, Action<Key> onClick, TimeSpan? intervalOfClick = null)
    {
        var keyboardEvent = new KeyboardButtonSubscription(key, onClick, intervalOfClick);

        AddKeyboardEvent(keyboardEvent);
    }

    public void Subscribe(IEnumerable<Key> keys, Action<Key> onClick, TimeSpan? intervalOfClick = null)
    {
        foreach (var key in keys)
            Subscribe(key, onClick, intervalOfClick);
    }

    public void SubscribeOnce(Key key, Action<Key> onClick) =>
        AddKeyboardEvent(new KeyboardButtonSubscription(key, onClick, singleUse: true));

    public void Unsubscribe(Key key)
    {
        var subscriptions = _subscriptions.Where(e => e.Key.Equals(key)).ToArray();

        foreach (var buttonSubscription in subscriptions)
            _subscriptions.Remove(buttonSubscription);
    }

    public void UnsubscribeAll() => _subscriptions.Clear();

    public void UnsubscribeAll(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
            Unsubscribe(key);
    }

    public void Unsubscribe(Guid guid)
    {
        var subscriptions = _subscriptions.Where(e => e.Id.Equals(guid)).ToArray();
        foreach (var buttonSubscription in subscriptions)
            _subscriptions.Remove(buttonSubscription);
    }

    private void Register()
    {
        if (IsListening)
            return;

        IsListening = true;
        HookKeyboard();
    }

    private void Unregister()
    {
        if (!IsListening)
            return;

        if (_subscriptions.Count > 0)
            UnsubscribeAll();

        UnHookKeyboard();
        IsListening = false;
    }

    private void AddKeyboardEvent(KeyboardButtonSubscription keyboardButtonSubscription) =>
        _subscriptions.Add(keyboardButtonSubscription);

    public new void Dispose() => Unregister();

    private void OnKeyPressed(object? sender, KeyPressedArgs e)
    {
        var keyboardEvents =
            _subscriptions.Where(b => b.Key.Equals(e.KeyPressed));

        foreach (var keyboardEvent in keyboardEvents)
        {
            if (keyboardEvent.SingleUse)
                Unsubscribe(keyboardEvent.Id);

            keyboardEvent.Invoke();
        }
    }
}