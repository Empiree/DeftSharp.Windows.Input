namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumberListenerSubscribeTests
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
    public async void NumberListener_Subscribe()
    {
        var keyboardListener = new KeyboardListener();
        var numberListener = new NumberListener(keyboardListener);

        await Task.Run(() =>
        {
            numberListener.Subscribe(_ => { });

            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(numberListener.IsListening, "Number listener is not listening subscription events.");
            Assert.Equal(keyboardListener.Keys.Count(), _numKeys.Length);
            Assert.True(_numKeys.All(n => keyboardListener.Keys.Select(s => s.Key).Contains(n.Key)));
        });
    }
}