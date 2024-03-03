using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interfaces;
using DeftSharp.Windows.Input.Shared.Models;

namespace DeftSharp.Windows.Input;

public sealed class MouseListener : IDisposable
{
    private readonly ObservableCollection<MouseSubscription> _subscriptions;
    private readonly IMouseAPI _mouseAPI;

    public bool IsListening { get; private set; }

    public ReadOnlyCollection<MouseSubscription> Subscriptions => _subscriptions.AsReadOnly();

    public MouseListener()
    {
        _mouseAPI = new WindowsMouseListener();
        _mouseAPI.MouseInput += OnMouseInput;

        _subscriptions = new ObservableCollection<MouseSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add && !IsListening)
            Register();

        if (_subscriptions.Count == 0)
            Unregister();
    }

    public void Subscribe(MouseEvent mouseEvent, Action onAction, TimeSpan? intervalOfClick = null)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, intervalOfClick ?? TimeSpan.Zero);
        _subscriptions.Add(subscription);
    }

    public void SubscribeOnce(MouseEvent mouseEvent, Action onAction)
    {
        var subscription = new MouseSubscription(mouseEvent, onAction, true);
        _subscriptions.Add(subscription);
    }

    public void Unsubscribe(MouseEvent mouseEvent)
    {
        var subscriptions = _subscriptions.Where(e => e.Event.Equals(mouseEvent)).ToArray();

        foreach (var mouseSubscription in subscriptions)
            _subscriptions.Remove(mouseSubscription);
    }

    public void Unsubscribe(Guid id)
    {
        var mouseEvent = _subscriptions.FirstOrDefault(s => s.Id == id);

        if (mouseEvent is null)
            return;

        _subscriptions.Remove(mouseEvent);
    }

    public void UnsubscribeAll() => _subscriptions.Clear();

    private void Register()
    {
        if (IsListening)
            return;

        IsListening = true;
        _mouseAPI.Hook();
    }

    private void Unregister()
    {
        if (!IsListening)
            return;

        if (_subscriptions.Count > 0)
            UnsubscribeAll();

        _mouseAPI.Unhook();
        IsListening = false;
    }

    public void Dispose() => Unregister();

    private void OnMouseInput(object? sender, MouseInputArgs e)
    {
        var mouseEvents =
            _subscriptions.Where(s => s.Event.Equals(e.Event)).ToArray();

        foreach (var mouseEvent in mouseEvents)
        {
            if (mouseEvent.SingleUse)
                Unsubscribe(mouseEvent.Id);

            mouseEvent.Invoke();
        }
    }
}