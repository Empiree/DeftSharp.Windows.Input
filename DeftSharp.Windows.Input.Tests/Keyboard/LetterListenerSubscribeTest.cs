namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerSubscribeTests
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
    public async void LetterListener_Subscribe()
    {
        var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener);

        await Task.Run(() =>
        {
            letterListener.Subscribe(_ => { });

            Assert.True(keyboardListener.IsListening, "Keyboard listener is not listening subscription events.");
            Assert.True(letterListener.IsListening, "Letter listener is not listening subscription events.");
            Assert.Equal(keyboardListener.Keys.Count(), _letKeys.Length);
            Assert.True(_letKeys.All(n => keyboardListener.Keys.Select(s => s.Key).Contains(n.Key)));
        });
    }
}