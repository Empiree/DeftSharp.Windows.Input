namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerSubscribeTests
{
    private readonly IEnumerable<Key> _keys = KeyboardKeySet.NumpadKeys;

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
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Count());
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
        });
    }
}