using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DeftSharp.Windows.Input.Shared.Listeners;

public abstract class InputListener<TSubscription>
{
    protected readonly ObservableCollection<TSubscription> _subscriptions;

    public bool IsListening { get; private set; }

    public ReadOnlyCollection<TSubscription> Subscriptions => _subscriptions.AsReadOnly();

    protected InputListener()
    {
        _subscriptions = new ObservableCollection<TSubscription>();
        _subscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Register();

        if (!_subscriptions.Any())
            Unregister();
    }

    public void UnsubscribeAll()
    {
        if (_subscriptions.Any())
            _subscriptions.Clear();
    }

    protected virtual void Register() => IsListening = true;
    protected virtual void Unregister() => IsListening = false;
}