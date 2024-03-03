using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class UnregisterTests
{
    private readonly WPFEmulator _emulator;

    public UnregisterTests()
    {
        _emulator = new WPFEmulator();
    }

    private void RunTest(Action<MouseListener> onTest)
    {
        var mouseListener = new MouseListener();
        _emulator.Run(() => onTest(mouseListener));

        Assert.False(mouseListener.IsListening,
            "The Unregister function is not called after unsubscribing from all events.");
    }

    [Fact]
    public void Unregister_Test()
    {
        RunTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.Unsubscribe(MouseEvent.LeftButtonDown);
        });
    }

    [Fact]
    public void Unregister_Test2()
    {
        RunTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void Unregister_Test3()
    {
        RunTest(listener =>
        {
            listener.Subscribe(MouseEvent.LeftButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });
            listener.Subscribe(MouseEvent.RightButtonDown, () => { });
            listener.Subscribe(MouseEvent.RightButtonUp, () => { });
            listener.UnsubscribeAll();
        });
    }

    [Fact]
    public void Unregister_Test4()
    {
        RunTest(listener =>
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
    public void Unregister_Test5()
    {
        RunTest(listener => listener.UnsubscribeAll());
    }

    [Fact]
    public void Unregister_Test6()
    {
        RunTest(_ => { });
    }

    [Fact]
    public void Unregister_Test7()
    {
        RunTest(listener => listener.Dispose());
    }
}