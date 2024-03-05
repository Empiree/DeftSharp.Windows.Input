using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Listeners;
using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardListener : InputListener<KeyboardSubscription>, IDisposable
{
    private readonly IKeyboardInterceptor _keyboardInterceptor;

    public KeyboardListener()
    {
        _keyboardInterceptor = WindowsKeyboardInterceptor.Instance;
        _keyboardInterceptor.KeyPressed += OnKeyPressed;
        _keyboardInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
    }

    public void Subscribe(Key key, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        var keyboardSubscription =
            new KeyboardSubscription(key, onClick, intervalOfClick ?? TimeSpan.Zero, keyboardEvent);

        InputSubscriptions.Add(keyboardSubscription);
    }

    public void Subscribe(IEnumerable<Key> keys, Action<Key> onClick,
        TimeSpan? intervalOfClick = null, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown)
    {
        foreach (var key in keys)
            Subscribe(key, onClick, intervalOfClick, keyboardEvent);
    }

    public void SubscribeOnce(Key key, Action<Key> onClick, KeyboardEvent keyboardEvent = KeyboardEvent.KeyDown) =>
        InputSubscriptions.Add(new KeyboardSubscription(key, onClick, keyboardEvent, true));

    public void Unsubscribe(Key key)
    {
        var subscriptions = InputSubscriptions.Where(e => e.Key.Equals(key)).ToArray();

        foreach (var buttonSubscription in subscriptions)
            InputSubscriptions.Remove(buttonSubscription);
    }

    public void UnsubscribeAll(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
            Unsubscribe(key);
    }

    public void Unsubscribe(Guid id)
    {
        var keyboardSubscribe = InputSubscriptions.FirstOrDefault(s => s.Id == id);

        if (keyboardSubscribe is null)
            return;

        InputSubscriptions.Remove(keyboardSubscribe);
    }

    public void Dispose()
    {
        Unregister();

        _keyboardInterceptor.KeyPressed -= OnKeyPressed;
        _keyboardInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    private bool OnInterceptorUnhookRequested() => !InputSubscriptions.Any();

    protected override void Register()
    {
        if (IsListening)
            return;

        _keyboardInterceptor.Hook();
        base.Register();
    }

    protected override void Unregister()
    {
        if (!IsListening)
            return;

        UnsubscribeAll();
        _keyboardInterceptor.Unhook();
        base.Unregister();
    }

    private void OnKeyPressed(object? sender, KeyPressedArgs e)
    {
        var keyboardEvents =
            InputSubscriptions.Where(s => s.Key.Equals(e.KeyPressed) && s.Event == e.Event).ToArray();

        foreach (var keyboardEvent in keyboardEvents)
        {
            if (keyboardEvent.SingleUse)
                Unsubscribe(keyboardEvent.Id);

            keyboardEvent.Invoke();
        }
    }
}