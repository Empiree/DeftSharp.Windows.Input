namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardListenerUnsubscribeTests
{
    private readonly ThreadRunner _threadRunner = new();

    private void RunListenerTest(Action<KeyboardListener> onTest)
    {
        var keyboardListener = new KeyboardListener();
        _threadRunner.Run(() => onTest(keyboardListener));

        Assert.False(keyboardListener.IsListening,
            "The Unregister function is not called after unsubscribing from all events.");
    }

    [Fact]
    public void KeyboardListener_SubscribeUnsubscribe()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(Key.A, _ => { });
            listener.Unsubscribe(Key.A);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(Key.A, _ => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeUnsubscribeMany()
    {
        RunListenerTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, _ => { });

            foreach (var key in keys)
                listener.Unsubscribe(key);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeManyUnsubscribe()
    {
        RunListenerTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, _ => { });
            listener.Unsubscribe(Key.D);
            listener.Unsubscribe(Key.S);
            listener.Unsubscribe(Key.A);
            listener.Unsubscribe(Key.W);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeCoupleUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, _ => { });
            listener.Subscribe(Key.A, _ => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_UnsubscribeAll()
    {
        RunListenerTest(listener => listener.UnsubscribeAll());
    }

    [Fact]
    public void KeyboardListener_Empty()
    {
        RunListenerTest(_ => { });
    }

    [Fact]
    public void KeyboardListener_SubscribeCombinationUnsubscribeById()
    {
        RunListenerTest(listener =>
        {
            Key[] combination = { Key.W, Key.A };
            listener.SubscribeCombination(combination, () => { });
            var combinationId = listener.Combinations.First().Id;
            listener.Unsubscribe(combinationId);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeCombinationUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            Key[] combination = { Key.W, Key.A };
            for (var i = 0; i < 5; i++) 
                listener.SubscribeCombination(combination, () => { });

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeCombinationUnsubscribeSingleKey()
    {
        var keyboardListener = new KeyboardListener();
        Key[] combination = { Key.W, Key.A };
        keyboardListener.SubscribeCombination(combination, () => { });
        keyboardListener.Unsubscribe(Key.W);
        keyboardListener.Unsubscribe(Key.A);

        Assert.Single(keyboardListener.Combinations);
        Assert.True(keyboardListener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable())));
    }
}