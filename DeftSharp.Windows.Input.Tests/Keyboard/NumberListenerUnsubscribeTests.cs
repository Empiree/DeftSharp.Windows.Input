namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumberListenerUnsubscribeTests
{
    private readonly Key[] _keys =
    {
        Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0,
        Key.NumPad7, Key.NumPad8, Key.NumPad9,
        Key.NumPad4, Key.NumPad5, Key.NumPad6,
        Key.NumPad1, Key.NumPad2, Key.NumPad3,
        Key.NumPad0
    };

    [Fact]
    public async void NumberListener_SubscribeUnsubscribe()
    {
        var keyboardListener = new KeyboardListener();
        var numberListener = new NumberListener(keyboardListener);

        await Task.Run(() =>
        {
            numberListener.Subscribe(_ => { });
            numberListener.Unsubscribe();

            Assert.False(keyboardListener.IsListening);
            Assert.False(numberListener.IsListening);
        });
    }

    [Fact]
    public async void NumberListener_SubscribeUnsubscribeOnlyNumberListenerSubscriptions()
    {
        var keyboardListener = new KeyboardListener();
        var numberListener = new NumberListener(keyboardListener);

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(_keys, _ => { });

            numberListener.Subscribe(_ => { });
            numberListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Length);
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numberListener.IsListening);
        });
    }
}