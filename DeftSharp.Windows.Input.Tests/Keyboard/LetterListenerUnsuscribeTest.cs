namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerUnsubscribeTests
{
    private readonly LetterButton[] _letKeys =
    {
        new(Key.A, "A"),
        new(Key.B, "B"),
        new(Key.C, "C"),
        new(Key.D, "D"),
        new(Key.E, "E"),
        new(Key.F, "F"),
        new(Key.G, "G"),
        new(Key.H, "H"),
        new(Key.I, "I"),
        new(Key.J, "J"),
        new(Key.K, "K"),
        new(Key.L, "L"),
        new(Key.M, "M"),
        new(Key.N, "N"),
        new(Key.O, "O"),
        new(Key.P, "P"),
        new(Key.Q, "Q"),
        new(Key.R, "R"),
        new(Key.S, "S"),
        new(Key.T, "T"),
        new(Key.U, "U"),
        new(Key.V, "V"),
        new(Key.W, "W"),
        new(Key.X, "X"),
        new(Key.Y, "Y"),
        new(Key.Z, "Z")
    };

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
        var keys = _letKeys.Select(n => n.Key).ToArray();

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