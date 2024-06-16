namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class NumberListenerUnsubscribeTests
{
    private readonly IEnumerable<Key> _keys = KeyboardKeySet.NumberKeys;

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
            Assert.Equal(keyboardListener.Keys.Count(), _keys.Count());
            Assert.True(_keys.All(k => keyboardListener.Keys.Select(s => s.Key).Contains(k)));
            Assert.False(numberListener.IsListening);
        });
    }
}