using System.Windows.Input;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public class UnregisterTests
{
    private readonly WPFEmulator _emulator;

    public UnregisterTests()
    {
        _emulator = new WPFEmulator();
    }
    
    private void RunTest(Action<KeyboardListener> onTest)
    {
        var keyboardListener = new KeyboardListener();
        _emulator.Run(() => onTest(keyboardListener));
        
        Assert.False(keyboardListener.IsListening,
            "The Unregister function is not called after unsubscribing from all events.");
    }

    [Fact]
    public void Unregister_Test()
    {
        RunTest(listener =>
        {
            listener.Subscribe(Key.A, key => { });
            listener.Unsubscribe(Key.A);
        });
    }

    [Fact]
    public void Unregister_Test2()
    {
        RunTest(listener =>
        {
            listener.Subscribe(Key.A, key => { });
            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Unregister_Test3()
    {
        RunTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, key => { });
            listener.UnsubscribeAll(keys);
        });
    }
    
    [Fact]
    public void Unregister_Test4()
    {
        RunTest(listener =>
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
    public void Unregister_Test5()
    {
        RunTest(listener =>
        {
            Key[] keys = { Key.W, Key.A, Key.S, Key.D };
            listener.Subscribe(keys, key => { });
            listener.Subscribe(Key.A, key => {});
            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Unregister_Test6()
    {
        RunTest(listener => listener.UnsubscribeAll());
    }
    
    [Fact]
    public void Unregister_Test7()
    {
        RunTest(_ => { });
    }
}