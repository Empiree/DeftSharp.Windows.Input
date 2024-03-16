namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class MouseListenerSubscribeTests
{
    private readonly ThreadRunner _threadRunner = new();

    [Fact]
    public void MouseListener_SingleSubscribe()
    {
        var listener = new MouseListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(MouseEvent.Move, () => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Subscriptions);

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void MouseListener_Identical3Subscriptions()
    {
        var listener = new MouseListener();

        _threadRunner.Run(() =>
        {
            listener.Subscribe(MouseEvent.Move, () => { });
            listener.Subscribe(MouseEvent.Move, () => { });
            listener.Subscribe(MouseEvent.Move, () => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(3, listener.Subscriptions.Count(s => s.Event == MouseEvent.Move));

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void MouseListener_SubscribeOnce()
    {
        var listener = new MouseListener();

        _threadRunner.Run(() =>
        {
            listener.SubscribeOnce(MouseEvent.LeftButtonDown, () => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(1, listener.Subscriptions.Count(s => s.SingleUse));
            Assert.Single(listener.Subscriptions);

            listener.UnsubscribeAll();
        });
    }
}