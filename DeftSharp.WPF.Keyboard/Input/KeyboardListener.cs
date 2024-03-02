using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Keyboard.InteropServices;
using DeftSharp.Windows.Keyboard.Shared.Exceptions;
using DeftSharp.Windows.Keyboard.Shared.Models;

namespace DeftSharp.Windows.Keyboard.Input;

public sealed class KeyboardListener : WindowsKeyboardListener
{
    private readonly List<KeyboardButton> _keyboardButtons;

    public bool IsListening { get; private set; }

    public KeyboardListener()
    {
        _keyboardButtons = new List<KeyboardButton>();
        KeyPressed += OnKeyPressed;
    }

    public void Register()
    {
        if (IsListening)
            return;

        IsListening = true;
        HookKeyboard();
    }

    public void Unregister()
    {
        if (!IsListening)
            return;

        UnsubscribeAll();
        UnHookKeyboard();
        IsListening = false;
    }

    public void Subscribe(Key key, Action<Key> onClick, TimeSpan? intervalOfClick = null)
    {
        var keyboardEvent = new KeyboardButton(key, onClick)
        {
            IntervalOfClick = intervalOfClick ?? TimeSpan.Zero
        };

        AddKeyboardEvent(keyboardEvent);
    }

    public void Subscribe(IEnumerable<Key> keys, Action<Key> onClick, TimeSpan? intervalOfClick = null)
    {
        foreach (var key in keys)
            Subscribe(key, onClick, intervalOfClick);
    }

    public void SubscribeOnce(Key key, Action<Key> onClick) =>
        AddKeyboardEvent(new KeyboardButton(key, onClick, true));

    public void Unsubscribe(Key key) =>
        _keyboardButtons.RemoveAll(e => e.Key.Equals(key));

    public void Unsubscribe(IEnumerable<Key> keys)
    {
        foreach (var key in keys)
            Unsubscribe(key);
    }

    public void UnsubscribeAll() => _keyboardButtons.Clear();

    public void Unsubscribe(Guid guid) =>
        _keyboardButtons.RemoveAll(e => e.Id.Equals(guid));

    private void AddKeyboardEvent(KeyboardButton keyboardButton)
    {
        if (!IsListening)
            throw new KeyboardListenerException(
                "Cannot perform operation because the KeyboardListener is not currently listening for keyboard input. " +
                "Please ensure that you have called the Register method to start listening for keyboard events before attempting " +
                "this operation.");

        _keyboardButtons.Add(keyboardButton);
    }

    private void OnKeyPressed(object? sender, KeyPressedArgs e)
    {
        var keyboardEvents =
            _keyboardButtons.Where(b => b.Key.Equals(e.KeyPressed)).ToArray();

        foreach (var keyboardEvent in keyboardEvents)
        {
            if (keyboardEvent.SingleUse)
                Unsubscribe(keyboardEvent.Id);

            keyboardEvent.Invoke();
        }
    }
}