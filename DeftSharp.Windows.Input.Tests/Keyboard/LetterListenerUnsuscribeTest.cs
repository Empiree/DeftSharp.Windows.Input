namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerUnsubscribeTests
{
    private readonly Dictionary<KeyboardLayoutType, LetterButton[]> _layouts = KeyboardLayoutTypes.Layouts;

    [Fact]
    public async void LetterListener_SubscribeUnsubscribe()
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener);

        await Task.Run(() =>
        {
            letterListener.Subscribe(_ => { });
            letterListener.Unsubscribe();

            Assert.False(keyboardListener.IsListening);
            Assert.False(letterListener.IsListening);
        });
    }

    [Fact]
    public async void LetterListener_SubscribeUnsubscribeOnlyLetterListenerSubscriptions()
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener);
        var keys = _layouts[KeyboardLayoutType.Qwerty].Select(l => l.Key).ToArray();

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(keys, _ => { });

            letterListener.Subscribe(_ => { });
            letterListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), keys.Length);
            Assert.True(keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(letterListener.IsListening);
        });
    }
}