using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class SubscriptionTests
{
    private readonly WPFEmulator _emulator;

    public SubscriptionTests()
    {
        _emulator = new WPFEmulator();
    }

    [Fact]
    public void Subscribe_Test1()
    {
        var listener = new MouseListener();

        _emulator.Run(() =>
        {
            listener.Subscribe(MouseEvent.Move, () => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Subscriptions);

            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void Subscribe_Test2()
    {
        var listener = new MouseListener();

        _emulator.Run(() =>
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
    public void Subscribe_Test3()
    {
        var listener = new MouseListener();

        _emulator.Run(() =>
        {
            listener.SubscribeOnce(MouseEvent.LeftButtonDown, () => { });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(1, listener.Subscriptions.Count(s => s.SingleUse));
            Assert.Single(listener.Subscriptions);

            listener.UnsubscribeAll();
        });
    }
}