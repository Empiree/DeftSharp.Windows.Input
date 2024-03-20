namespace DeftSharp.Windows.Input.Tests.Keyboard;

public class KeyboardManipulatorTests : IDisposable
{
    private readonly ThreadRunner _threadRunner = new();

    [Fact]
    public void KeyboardManipulator_PreventBy2()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _threadRunner.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);

            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());

            keyboardManipulator1.Release(Key.A);
        });
    }

    [Fact]
    public void KeyboardManipulator_PreventReleaseAll()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();

        _threadRunner.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.A);
            keyboardManipulator2.Prevent(Key.A);

            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());

            keyboardManipulator1.ReleaseAll();

            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }

    [Fact]
    public void KeyboardManipulator_PreventBy3()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _threadRunner.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);
            keyboardManipulator3.Prevent(Key.R);

            Assert.Equal(keyboardManipulator1.LockedKeys.Count(), keyboardManipulator2.LockedKeys.Count());
            Assert.Equal(keyboardManipulator2.LockedKeys.Count(), keyboardManipulator3.LockedKeys.Count());

            keyboardManipulator1.ReleaseAll();

            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }

    [Fact]
    public void KeyboardManipulator_PreventBy2ReleaseAll()
    {
        var keyboardManipulator1 = new KeyboardManipulator();
        var keyboardManipulator2 = new KeyboardManipulator();
        var keyboardManipulator3 = new KeyboardManipulator();

        _threadRunner.Run(() =>
        {
            keyboardManipulator1.Prevent(Key.Q);
            keyboardManipulator2.Prevent(Key.W);

            keyboardManipulator3.ReleaseAll();

            Assert.Empty(keyboardManipulator1.LockedKeys);
            Assert.Empty(keyboardManipulator2.LockedKeys);
        });
    }

    [Fact]
    public void KeyboardManipulator_DisposeTest()
    {
        _threadRunner.Run(() =>
        {
            using (var keyboardManipulator1 = new KeyboardManipulator())
            {
                using (var keyboardManipulator2 = new KeyboardManipulator())
                {
                    using (var keyboardManipulator3 = new KeyboardManipulator())
                    {
                        keyboardManipulator1.Prevent(Key.Q);
                        keyboardManipulator2.Prevent(Key.W);
                    }

                    Assert.NotEmpty(keyboardManipulator1.LockedKeys);

                    keyboardManipulator1.Release(Key.Q);
                    keyboardManipulator1.Release(Key.W);
                }
            }
        });
    }

    public void Dispose()
    {
        var manipulator = new KeyboardManipulator();
        Assert.Empty(manipulator.LockedKeys);
    }
}