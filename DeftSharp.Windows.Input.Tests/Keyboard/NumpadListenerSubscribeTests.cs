namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerSubscribeTests
{
    private readonly Key[] _keys =
    {
        Key.NumPad7, Key.NumPad8, Key.NumPad9,
        Key.NumPad4, Key.NumPad5, Key.NumPad6,
        Key.NumPad1, Key.NumPad2, Key.NumPad3,
        Key.NumPad0
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
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Length);
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
        });
    }
}