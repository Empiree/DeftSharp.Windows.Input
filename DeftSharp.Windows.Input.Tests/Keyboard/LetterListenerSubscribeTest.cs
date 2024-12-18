namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerSubscribeTests
{
    [Fact]
    public async void LetterListener_Subscribe()
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener);

        var layoutKeys = letterListener.Keys.ToArray();

        await Task.Run(() =>
        {
            letterListener.Subscribe(_ => { });

            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(letterListener.IsListening, "Letter listener is not listening subscription events.");
            Assert.Equal(keyboardListener.Keys.Count(), layoutKeys.Length);
            Assert.True(layoutKeys.All(key => keyboardListener.Keys.Select(s => s.Key).Contains(key)));
        });
    }
}