namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumpadListenerUnsubscribeTests
{
    private readonly IEnumerable<Key> _keys = KeyboardKeySet.NumpadKeys;

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
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Count());
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numpadListener.IsListening);
        });
    }
}