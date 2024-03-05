using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace DeftSharp.Windows.Input.Shared.Listeners;

public abstract class InputListener<TSubscription>
{
    protected readonly ObservableCollection<TSubscription> InputSubscriptions;

    public bool IsListening { get; private set; }

    public ReadOnlyCollection<TSubscription> Subscriptions => InputSubscriptions.AsReadOnly();

    protected InputListener()
    {
        InputSubscriptions = new ObservableCollection<TSubscription>();
        InputSubscriptions.CollectionChanged += SubscriptionsOnCollectionChanged;
    }

    private void SubscriptionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action == NotifyCollectionChangedAction.Add)
            Register();

        if (!InputSubscriptions.Any())
            Unregister();
    }

    public void UnsubscribeAll()
    {
        if (InputSubscriptions.Any())
            InputSubscriptions.Clear();
    }

    protected virtual void Register() => IsListening = true;
    protected virtual void Unregister() => IsListening = false;
}