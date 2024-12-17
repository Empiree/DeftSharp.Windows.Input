namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerUnsubscribeTests
{
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
        var keys = letterListener.Keys.ToArray();

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(keys, _ => { });

            letterListener.Subscribe(_ => { });
            letterListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), keys.Length);
            Assert.True(keys.All(key => keyboardListener.Keys.Select(s => s.Key).Contains(key)));
            Assert.False(letterListener.IsListening);
        });
    }
}