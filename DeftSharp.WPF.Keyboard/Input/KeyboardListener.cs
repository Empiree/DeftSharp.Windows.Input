using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Keyboard.InteropServices.Keyboard;
using DeftSharp.Windows.Keyboard.Shared.Interfaces;
using DeftSharp.Windows.Keyboard.Shared.Models;

namespace DeftSharp.Windows.Keyboard.Input;

public sealed class KeyboardListener : IDisposable
{
    private readonly ObservableCollection<KeyboardButtonSubscription> _subscriptions;
    private readonly IKeyboardAPI _keyboardAPI;

    public bool IsListening { get; private set; }

    public ReadOnlyCollection<KeyboardButtonSubscription> Subscriptions => _subscriptions.AsReadOnly();

    public KeyboardListener()
    {
        _keyboardAPI = new WindowsKeyboardListener();
        _keyboardAPI.KeyPressed += OnKeyPressed;
        
        _subscriptions = new ObservableCollection<KeyboardButtonSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
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
        var keyboardEvent = new KeyboardButtonSubscription(key, onClick, intervalOfClick ?? TimeSpan.Zero);

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
        _keyboardAPI.Hook();
    }

    private void Unregister()
    {
        if (!IsListening)
            return;

        if (_subscriptions.Count > 0)
            UnsubscribeAll();

        _keyboardAPI.Unhook();
        IsListening = false;
    }

    private void AddKeyboardEvent(KeyboardButtonSubscription keyboardButtonSubscription) =>
        _subscriptions.Add(keyboardButtonSubscription);

    public void Dispose() => Unregister();

    private void OnKeyPressed(object? sender, KeyPressedArgs e)
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
}