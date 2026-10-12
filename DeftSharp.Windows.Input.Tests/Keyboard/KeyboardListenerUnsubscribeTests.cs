namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardListenerUnsubscribeTests
{
    private async void RunListenerTest(Action<KeyboardListener> onTest)
    {
        var keyboardListener = new KeyboardListener();
        await Task.Run(() => onTest(keyboardListener));

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
            listener.Unsubscribe();
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
            listener.Unsubscribe();
        });
    }

    [Fact]
    public void KeyboardListener_UnsubscribeAll()
    {
        RunListenerTest(listener => listener.Unsubscribe());
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

            listener.Unsubscribe();
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

    [Fact]
    public void KeyboardListener_SubscribeSequenceUnsubscribeSingleKey()
    {
        var keyboardListener = new KeyboardListener();
        Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
        keyboardListener.SubscribeSequence(sequence, () => { });
        keyboardListener.Unsubscribe(Key.A);
        keyboardListener.Unsubscribe(Key.K);
        keyboardListener.Unsubscribe(Key.B);

        Assert.Single(keyboardListener.Sequences);
        Assert.True(keyboardListener.Sequences.All(x => x.Sequence.SequenceEqual(sequence.AsEnumerable())));
    }


    [Fact]
    public void KeyboardListener_SubscribeSequenceUnsubscribeById()
    {
        RunListenerTest(listener =>
        {
            Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
            listener.SubscribeSequence(sequence, () => { });
            var sequenceId = listener.Sequences.First().Id;
            listener.Unsubscribe(sequenceId);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeSequenceUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
            for (int i = 0; i < 5; ++i)
                listener.SubscribeSequence(sequence, () => { });

            listener.Unsubscribe();
        });
    }

    [Fact]
    public void KeyboardListener_UnsubscribeCombination_RemovesAllMatches()
    {
        using var keyboardListener = new KeyboardListener();
        keyboardListener.SubscribeCombination(new[] { Key.LeftCtrl, Key.V }, () => { });
        keyboardListener.SubscribeCombination(new[] { Key.LeftCtrl, Key.V }, () => { });
        keyboardListener.SubscribeCombination(new[] { Key.LeftCtrl, Key.C }, () => { });

        keyboardListener.UnsubscribeCombination(new[] { Key.LeftCtrl, Key.V });

        var remaining = Assert.Single(keyboardListener.Combinations);
        Assert.Equal(new[] { Key.LeftCtrl, Key.C }, remaining.Combination);
    }

    [Fact]
    public void KeyboardListener_UnsubscribeCombination_KeyOrderDoesNotMatter()
    {
        using var keyboardListener = new KeyboardListener();
        keyboardListener.SubscribeCombination(new[] { Key.LeftCtrl, Key.V }, () => { });

        keyboardListener.UnsubscribeCombination(new[] { Key.V, Key.LeftCtrl });

        Assert.Empty(keyboardListener.Combinations);
    }

    [Fact]
    public void KeyboardListener_UnsubscribeCombination_KeepsOtherSubscriptions()
    {
        using var keyboardListener = new KeyboardListener();
        keyboardListener.Subscribe(Key.V, () => { });
        keyboardListener.SubscribeSequence(new[] { Key.LeftCtrl, Key.V }, () => { });
        keyboardListener.SubscribeCombination(new[] { Key.LeftCtrl, Key.V }, () => { });

        keyboardListener.UnsubscribeCombination(new[] { Key.LeftCtrl, Key.V });

        Assert.Empty(keyboardListener.Combinations);
        Assert.Single(keyboardListener.Keys);
        Assert.Single(keyboardListener.Sequences);
    }

    [Fact]
    public void KeyboardListener_UnsubscribeSequence_KeyOrderMatters()
    {
        using var keyboardListener = new KeyboardListener();
        keyboardListener.SubscribeSequence(new[] { Key.A, Key.B, Key.C }, () => { });

        keyboardListener.UnsubscribeSequence(new[] { Key.C, Key.B, Key.A });
        Assert.Single(keyboardListener.Sequences);

        keyboardListener.UnsubscribeSequence(new[] { Key.A, Key.B, Key.C });
        Assert.Empty(keyboardListener.Sequences);
    }
}