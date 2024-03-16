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
            Assert.Single(listener.Subscriptions);

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
            Assert.Equal(3, listener.Subscriptions.Count(s => s.Key == Key.A));

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
            Assert.Equal(6, listener.Subscriptions.Count(s => s.Key == Key.A));

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
            Assert.Equal(12, listener.Subscriptions.Count());

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
            Assert.Equal(9, listener.Subscriptions.Count());

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
            Assert.Equal(4, listener.Subscriptions.Count());
            Assert.Equal(4, listener.Subscriptions.Count(s => s.Key == Key.Back));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeWithDispose()
    {
        var listener = new KeyboardListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(Key.J, key => {});
            listener.Dispose();
        });
        
        Assert.False(listener.IsListening, "Keyboard listener is not listening subscription events.");

    }
}