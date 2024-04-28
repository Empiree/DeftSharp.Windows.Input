namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerUnsubscribeTests
{
    private readonly NumButton[] _numKeys =
    {
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
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
        var keys = _numKeys.Select(n => n.Key).ToArray();

        await Task.Run(() =>
        {
            keyboardListener.Subscribe(keys, _ => { });

            numpadListener.Subscribe(_ => { });
            numpadListener.Unsubscribe();

            Assert.True(keyboardListener.IsListening);
            Assert.Equal(keyboardListener.Keys.Count(), keys.Length);
            Assert.True(keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numpadListener.IsListening);
        });
    }
}