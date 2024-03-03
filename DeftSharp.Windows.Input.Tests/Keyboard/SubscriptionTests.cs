using System.Windows.Input;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

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
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            listener.Subscribe(Key.A, key =>{ });
            
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Single(listener.Subscriptions);
            
            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Subscribe_Test2()
    {
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            listener.Subscribe(Key.A, key =>{ });
            listener.Subscribe(Key.A, key =>{ });
            listener.Subscribe(Key.A, key =>{ });
            
            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(3, listener.Subscriptions.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Subscribe_Test3()
    {
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            Key[] keys = { Key.A , Key.K, Key.A, Key.B};
            listener.Subscribe(keys, key =>{ });
            listener.Subscribe(keys, key =>{ });
            listener.Subscribe(keys, key =>{ });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(6, listener.Subscriptions.Count(s => s.Key == Key.A));

            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Subscribe_Test4()
    {
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            Key[] keys = { Key.A , Key.K, Key.A, Key.B};
            listener.Subscribe(keys, key =>{ });
            listener.Subscribe(keys, key =>{ });
            listener.Subscribe(keys, key =>{ });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(12, listener.Subscriptions.Count);

            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Subscribe_Test5()
    {
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            Key[] keys = { Key.A , Key.K, Key.A, Key.B};
            listener.SubscribeOnce(Key.C, key =>{ });
            listener.Subscribe(keys, key =>{ });
            listener.Subscribe(keys, key =>{ });

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(9, listener.Subscriptions.Count);

            listener.UnsubscribeAll();
        });
    }
    
    [Fact]
    public void Subscribe_Test6()
    {
        var listener = new KeyboardListener();

        _emulator.Run(() =>
        {
            listener.Subscribe(Key.Back, key =>{});
            listener.Subscribe(Key.Back, key =>{});
            listener.Subscribe(Key.Back, key =>{});
            listener.Subscribe(Key.Back, key =>{});

            Assert.True(listener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.Equal(4, listener.Subscriptions.Count);
            Assert.Equal(4, listener.Subscriptions.Count(s=> s.Key == Key.Back));

            listener.UnsubscribeAll();
        });
    }
}