namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerSubscribeTests
{
    private readonly Dictionary<KeyboardLayoutType, LetterButton[]> _layouts = KeyboardLayoutTypes.Layouts;

    [Fact]
    public async void LetterListener_Subscribe()
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener);

        var letKeys = _layouts[KeyboardLayoutType.Qwerty];
        
        await Task.Run(() =>
        {
            letterListener.Subscribe(_ => { });

            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(letterListener.IsListening, "Letter listener is not listening subscription events.");
            Assert.Equal(keyboardListener.Keys.Count(), letKeys.Length);
            Assert.True(letKeys.All(n => keyboardListener.Keys.Select(s => s.Key).Contains(n.Key)));
        });
    }
    
    [Theory]
    [InlineData(KeyboardLayoutType.Qwerty)]
    [InlineData(KeyboardLayoutType.Dvorak)]
    [InlineData(KeyboardLayoutType.Colemak)]
    [InlineData(KeyboardLayoutType.Azerty)]
    [InlineData(KeyboardLayoutType.Workman)]
    [InlineData(KeyboardLayoutType.Norman)]
    public async Task LetterListener_SubscribeDifferentLayouts(KeyboardLayoutType layout)
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener, layout);
        var layoutKeys = _layouts[layout].Select(s => s.Key).ToHashSet();
        
        await Task.Run(() =>
        {
            letterListener.Subscribe(_ => {});
            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(letterListener.IsListening, "Letter listener is not listening subscription events.");
            Assert.Equal(layoutKeys.Count, keyboardListener.Keys.Count());
            Assert.True(layoutKeys.SetEquals(keyboardListener.Keys.Select(k => k.Key)),
                $"They keys in the KeyboardListener do not match the {layout} layout.");
        });
    }
}