namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class MouseListenerUnsubscribeTests
{
    private readonly ThreadRunner _threadRunner = new();

    private async void RunListenerTest(Action<MouseListener> onTest)
    {
        var mouseListener = new MouseListener();
        await _threadRunner.Run(() => onTest(mouseListener));

        Assert.False(mouseListener.IsListening,
            "The Unregister function is not called after unsubscribing from all events.");
    }

    [Fact]
    public void MouseListener_SubscribeUnsubscribe()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.Unsubscribe(MouseEvent.LeftButtonDown);
        });
    }

    [Fact]
    public void MouseListener_SubscribeUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void MouseListener_SubscribeManyUnsubscribeAll()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });
            listener.Subscribe(MouseEvent.RightButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void MouseListener_SubscribeManyUnsubscribeMany()
    {
        RunListenerTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });
            listener.Subscribe(MouseEvent.RightButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });

            listener.Unsubscribe(MouseEvent.LeftButtonDown);
            listener.Unsubscribe(MouseEvent.RightButtonUp);
            listener.Unsubscribe(MouseEvent.RightButtonDown);
            listener.Unsubscribe(MouseEvent.RightButtonUp);
        });
    }

    [Fact]
    public void MouseListener_UnsubscribeAll()
    {
        RunListenerTest(listener => listener.UnsubscribeAll());
    }

    [Fact]
    public void MouseListener_Empty()
    {
        RunListenerTest(_ => { });
    }
}