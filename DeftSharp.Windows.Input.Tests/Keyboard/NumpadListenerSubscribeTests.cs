namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerSubscribeTests
{
    private readonly NumButton[] _numKeys =
    {
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };

    [Fact]
    public async void NumpadListener_Subscribe()
    {
        var keyboardListener = new KeyboardListener();
        var numpadListener = new NumpadListener(keyboardListener);

        await Task.Run(() =>
        {
            numpadListener.Subscribe(_ => { });

            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(numpadListener.IsListening, "Numpad listener is not listening subscription events.");
            Assert.Equal(keyboardListener.Keys.Count(), _numKeys.Length);
            Assert.True(_numKeys.All(n => keyboardListener.Keys.Select(s => s.Key).Contains(n.Key)));
        });
    }
}