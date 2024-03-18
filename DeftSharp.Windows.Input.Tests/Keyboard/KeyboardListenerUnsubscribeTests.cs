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
            listener.Subscribe(Key.A, key => { });
            listener.Unsubscribe(Key.A);
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(Key.A, key => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void KeyboardListener_SubscribeUnsubscribeMany()
    {
        RunListenerTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, key => { });
            
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
            listener.Subscribe(keys, key => { });
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
            listener.Subscribe(keys, key => { });
            listener.Subscribe(Key.A, key => { });
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
}