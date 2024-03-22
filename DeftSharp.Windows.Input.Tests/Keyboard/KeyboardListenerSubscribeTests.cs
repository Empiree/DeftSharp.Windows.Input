using DeftSharp.Windows.Input.Shared.Exceptions;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardListenerSubscribeTests
{
    private readonly ThreadRunner _threadRunner = new();

    [Fact]
    public async void KeyboardListener_Subscribe()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.A, _ => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Keys);

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_Subscribe3()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.A, _ => { });
            listener.Subscribe(Key.A, _ => { });
            listener.Subscribe(Key.A, _ => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(3, listener.Keys.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeMany()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.Subscribe(keys, _ => { });
            listener.Subscribe(keys, _ => { });
            listener.Subscribe(keys, _ => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(6, listener.Keys.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_Subscribe12()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.Subscribe(keys, _ => { });
            listener.Subscribe(keys, _ => { });
            listener.Subscribe(keys, _ => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(12, listener.Keys.Count());

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeOnce()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            Key[] keys = { Key.A, Key.K, Key.A, Key.B };
            listener.SubscribeOnce(Key.C, _ => { });
            listener.Subscribe(keys, key => { });
            listener.Subscribe(keys, key => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(9, listener.Keys.Count());

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeBack()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.Back, _ => { });
            listener.Subscribe(Key.Back, _ => { });
            listener.Subscribe(Key.Back, _ => { });
            listener.Subscribe(Key.Back, _ => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");

            Assert.Equal(4, listener.Keys.Count());
            Assert.Equal(4, listener.Keys.Count(s => s.Key == Key.Back));
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeWithDispose()
    {
        var listener = new KeyboardListener();

        await _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.J, _ => { });
            listener.Dispose();
        });

        Assert.False(listener.IsListening, "Keyboard listener is not listening subscription events.");
    }

    [Fact]
    public async void KeyboardListener_SubscribeCombination()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D, };
        await _threadRunner.Run(() =>
        {
            listener.SubscribeCombination(combination, () => { });

            Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable<Key>())),
                "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Combinations);
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeCombinationMany()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D, };
        await _threadRunner.Run(() =>
        {
            for (var i = 0; i < 10; i++)
                listener.SubscribeCombination(combination, () => { });

            Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable())),
                    "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(10, listener.Combinations.Count());
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeCombinationOnce()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.C, Key.D };
        await _threadRunner.Run(() =>
        {
            for (var i = 0; i < 5; i++)
            {
                listener.SubscribeCombinationOnce(combination, () => { });
                listener.SubscribeCombination(combination, () => { });
            }
            Assert.True(listener.Combinations.All(x => x.Combination.SequenceEqual(combination.AsEnumerable())),
                    "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(10, listener.Combinations.Count());
            Assert.Equal(5, listener.Combinations.Count(x => x.SingleUse));
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeCombinationSingleThrowsException()
    {
        var listener = new KeyboardListener();

        Key[] combination = { Key.D };
        await _threadRunner.Run(() =>
        {
            Assert.Throws<KeyCombinationLengthException>(() => listener.SubscribeCombination(combination, () => { }));
            Assert.False(listener.IsListening);
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeSequence()
    {
        var listener = new KeyboardListener();

        Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
        await _threadRunner.Run(() =>
        {
            listener.SubscribeSequence(sequence, () => { });

            Assert.True(listener.Sequences.All(x => x.Sequence.SequenceEqual(sequence.AsEnumerable<Key>())),
                    "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Sequences);
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeSequenceMany()
    {
        var listener = new KeyboardListener();

        Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
        await _threadRunner.Run(() =>
        {
            for (var i = 0; i < 10; i++)
                listener.SubscribeSequence(sequence, () => { });

            Assert.True(listener.Sequences.All(x => x.Sequence.SequenceEqual(sequence.AsEnumerable())),
                    "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(10, listener.Sequences.Count());
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeSequenceOnce()
    {
        var listener = new KeyboardListener();

        Key[] sequence = { Key.A, Key.K, Key.A, Key.B };
        await _threadRunner.Run(() =>
        {
            for (var i = 0; i < 5; i++)
            {
                listener.SubscribeSequenceOnce(sequence, () => { });
                listener.SubscribeSequence(sequence, () => { });
            }
            Assert.True(listener.Sequences.All(x => x.Sequence.SequenceEqual(sequence.AsEnumerable())),
                    "In the listener, the subscribed combination is not found within the combinations.");
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(10, listener.Sequences.Count());
            Assert.Equal(5, listener.Sequences.Count(x => x.SingleUse));
        });
    }

    [Fact]
    public async void KeyboardListener_SubscribeSequenceSingleThrowsException()
    {
        var listener = new KeyboardListener();

        Key[] sequence = { Key.D };
        await _threadRunner.Run(() =>
        {
            Assert.Throws<KeyCombinationLengthException>(() => listener.SubscribeCombination(sequence, () => { }));
            Assert.False(listener.IsListening);
        });
    }
}