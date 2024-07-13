namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumberListenerSubscribeTests
{
    private readonly IEnumerable<Key> _keys = KeyboardKeySet.NumberKeys;

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
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Count());
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
        });
    }
}