namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumberListenerUnsubscribeTests
{
    private readonly NumButton[] _numKeys =
    {
        new(Key.D1, 1), new(Key.D2, 2), new(Key.D3, 3), new(Key.D4, 4), new(Key.D5, 5),
        new(Key.D6, 6), new(Key.D7, 7), new(Key.D8, 8), new(Key.D9, 9), new(Key.D0, 0),
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };

    [Fact]
    public async void NumberListener_SubscribeUnsubscribe()
    {
        var keyboardListener = new KeyboardListener();
        var numberListener = new NumberListener(keyboardListener);

        await Task.Run(() =>
        {
            numberListener.Subscribe(_ => { });
            numberListener.Unsubscribe();

            Assert.False(keyboardListener.IsListening);
            Assert.False(numberListener.IsListening);
        });
    }

    [Fact]
    public async void NumberListener_SubscribeUnsubscribeOnlyNumberListenerSubscriptions()
    {
        var keyboardListener = new KeyboardListener();
        var numberListener = new NumberListener(keyboardListener);
        var keys = _numKeys.Select(n => n.Key).ToArray();

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(keys, _ => { });

            numberListener.Subscribe(_ => { });
            numberListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), keys.Length);
            Assert.True(keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numberListener.IsListening);
        });
    }
}