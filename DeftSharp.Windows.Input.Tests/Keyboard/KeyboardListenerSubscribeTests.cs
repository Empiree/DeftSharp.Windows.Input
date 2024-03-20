using DeftSharp.Windows.Input.Shared.Subscriptions;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardListenerSubscribeTests
{
    private readonly ThreadRunner _threadRunner = new();

    [Fact]
    public void KeyboardListener_Subscribe()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.A, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Keys);

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_Subscribe3()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.A, key => { });
            listener.Subscribe(Key.A, key => { });
            listener.Subscribe(Key.A, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(3, listener.Keys.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeMany()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(6, listener.Keys.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_Subscribe12()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(12, listener.Keys.Count());

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeOnce()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.SubscribeOnce(Key.C, key => { });
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(9, listener.Keys.Count());

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeBack()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.Back, key => { });
            listener.Subscribe(Key.Back, key => { });
            listener.Subscribe(Key.Back, key => { });
            listener.Subscribe(Key.Back, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");

            Assert.Equal(4, listener.Keys.Count());
            Assert.Equal(4, listener.Keys.Count(s => s.Key == Key.Back));
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeWithDispose()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.J, key => { });
            listener.Dispose();
        });

        Assert.False(listener.IsListening, "Keyboard listener is not listening subscription events.");
    }

    [Fact]
    public void KeyboardListener_SubscribeCombination()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D, };
        _threadRunner.Run(() =>
        {
            listener.SubscribeCombination(combination, () => { });
        });

        Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable<Key>())),
            "In the listener, the subscribed combination is not found within the combinations.");
        Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
        Assert.Single(listener.Combinations);
    }

    [Fact]
    public void KeyboardListener_SubscribeCombinationMany()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D, };
        _threadRunner.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                listener.SubscribeCombination(combination, () => { });
            }
        });

        Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable<Key>())),
            "In the listener, the subscribed combination is not found within the combinations.");

        Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
        Assert.Equal(10, listener.Combinations.Count());
    }

    [Fact]
    public void KeyboardListener_SubscribeCombinationOnce()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D };
        _threadRunner.Run(() =>
        {
            for (int i = 0; i < 5; i++)
            {
            listener.SubscribeCombinationOnce(combination, () => { });
            listener.SubscribeCombination(combination, () => { });
            }
        });

        Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable<Key>())),
            "In the listener, the subscribed combination is not found within the combinations.");

        Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
        Assert.Equal(10, listener.Combinations.Count());
        Assert.Equal(5, listener.Combinations.Count(x => x.SingleUse));
    }
}