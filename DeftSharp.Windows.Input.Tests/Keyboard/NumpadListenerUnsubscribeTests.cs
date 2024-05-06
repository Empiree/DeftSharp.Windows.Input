namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerUnsubscribeTests
{
    private readonly Key[] _keys =
    {
        Key.NumPad7, Key.NumPad8, Key.NumPad9,
        Key.NumPad4, Key.NumPad5, Key.NumPad6,
        Key.NumPad1, Key.NumPad2, Key.NumPad3,
        Key.NumPad0
    };

    [Fact]
    public async void NumpadListener_SubscribeUnsubscribe()
    {
        var keyboardListener = new KeyboardListener();
        var numpadListener = new NumpadListener(keyboardListener);

        await Task.Run(() =>
        {
            numpadListener.Subscribe(_ => { });
            numpadListener.Unsubscribe();

            Assert.False(keyboardListener.IsListening);
            Assert.False(numpadListener.IsListening);
        });
    }

    [Fact]
    public async void NumpadListener_SubscribeUnsubscribeOnlyNumpadListenerSubscriptions()
    {
        var keyboardListener = new KeyboardListener();
        var numpadListener = new NumpadListener(keyboardListener);

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(_keys, _ => { });

            numpadListener.Subscribe(_ => { });
            numpadListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Length);
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numpadListener.IsListening);
        });
    }
}