namespace DeftSharp.Windows.Input.Tests.Keyboard;

public class KeyboardBinderTests
{
    [Fact]
    public async void KeyboardBinder_IsKeyBound()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            keyboardBinder.Bind(Key.A, Key.B);

            Assert.True(keyboardBinder.IsKeyBounded(Key.A));
            Assert.False(keyboardBinder.IsKeyBounded(Key.B));
        });
    }

    [Fact]
    public async void KeyboardBinder_IsKeyBoundWhenBindingTheSameKey()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            keyboardBinder.Bind(Key.C, Key.C);

            Assert.False(keyboardBinder.IsKeyBounded(Key.C));
        });
    }

    [Fact]
    public async void KeyboardBinder_BindMultipleKeysAtOnce()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            var keys = new List<Key>
            {
                Key.W,
                Key.X,
                Key.Y,
                Key.Z
            };

            keyboardBinder.Bind(keys, Key.A);

            Assert.True(keyboardBinder.IsKeyBounded(Key.W));
            Assert.True(keyboardBinder.IsKeyBounded(Key.X));
            Assert.True(keyboardBinder.IsKeyBounded(Key.Y));
            Assert.True(keyboardBinder.IsKeyBounded(Key.Z));
            Assert.False(keyboardBinder.IsKeyBounded(Key.A));
        });
    }

    [Fact]
    public async void KeyboardBinder_BindEmptyKeyCollection()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            var keys = new List<Key>();

            keyboardBinder.Bind(keys, Key.A);

            Assert.Empty(keyboardBinder.BoundedKeys);
        });
    }

    [Fact]
    public async void KeyboardBinder_Unbind()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            keyboardBinder.Bind(Key.A, Key.B);
            Assert.True(keyboardBinder.IsKeyBounded(Key.A));

            keyboardBinder.Unbind(Key.A);

            Assert.Empty(keyboardBinder.BoundedKeys);
        });
    }

    [Fact]
    public void KeyboardBinder_UnbindOneOfSeveralKeys()
    {
        var keyboardBinder = new KeyboardBinder();

        keyboardBinder.Bind(Key.F1, Key.F2);
        keyboardBinder.Bind(Key.F3, Key.F4);

        keyboardBinder.Unbind(Key.F3);

        Assert.True(keyboardBinder.IsKeyBounded(Key.F1));
        Assert.False(keyboardBinder.IsKeyBounded(Key.F3));

        keyboardBinder.Unbind(Key.F1);
    }

    [Fact]
    public async void KeyboardBinder_UnbindAll()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            var keys = new List<Key>
            {
                Key.Cancel,
                Key.Enter,
                Key.End,
                Key.Tab
            };

            keyboardBinder.Bind(keys, Key.A);
            Assert.Equal(4, keyboardBinder.BoundedKeys.Count);

            keyboardBinder.Unbind();

            Assert.Empty(keyboardBinder.BoundedKeys);
        });
    }

    [Fact]
    public async void KeyboardBinder_UnbindKeyThatHasNotBeenBound()
    {
        var keyboardBinder = new KeyboardBinder();

        await Task.Run(() =>
        {
            Assert.Empty(keyboardBinder.BoundedKeys);

            keyboardBinder.Unbind(Key.A);

            Assert.Empty(keyboardBinder.BoundedKeys);
        });
    }
}