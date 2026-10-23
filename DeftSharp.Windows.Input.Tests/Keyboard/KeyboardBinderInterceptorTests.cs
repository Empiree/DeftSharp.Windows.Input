using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Tests.Fakes;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardBinderInterceptorTests
{
    [Fact]
    public void KeyboardBinder_HoldBoundKey_TargetKeyIsHeldToo()
    {
        var keyboard = new FakeKeyboard();
        using var binder = new KeyboardBinderInterceptor(keyboard);
        binder.Bind(Key.A, Key.B);

        // The second KeyDown is an auto-repeat while A is held
        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));

        Assert.Equal(new[]
        {
            (Key.B, KeyboardSimulateOption.KeyDown),
            (Key.B, KeyboardSimulateOption.KeyDown),
            (Key.B, KeyboardSimulateOption.KeyUp)
        }, keyboard.SimulatedInputs);
    }

    [Fact]
    public void KeyboardBinder_SwappedKeys_SentKeysAreNotBlocked()
    {
        var keyboard = new FakeKeyboard();
        using var binder = new KeyboardBinderInterceptor(keyboard);
        binder.Bind(Key.A, Key.B);
        binder.Bind(Key.B, Key.A);

        // A is blocked and B is sent instead, the sent B must reach the system
        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.True(keyboard.Send(Key.B, KeyboardInputEvent.KeyDown, isBinderInput: true));

        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));
        Assert.True(keyboard.Send(Key.B, KeyboardInputEvent.KeyUp, isBinderInput: true));
    }

    [Fact]
    public void KeyboardBinder_RebindWhileKeyIsHeld_ReleasesTheKeyThatWasPressed()
    {
        var keyboard = new FakeKeyboard();
        using var binder = new KeyboardBinderInterceptor(keyboard);
        binder.Bind(Key.A, Key.B);

        keyboard.Send(Key.A, KeyboardInputEvent.KeyDown);
        binder.Bind(Key.A, Key.C);
        keyboard.Send(Key.A, KeyboardInputEvent.KeyUp);

        Assert.Equal(new[]
        {
            (Key.B, KeyboardSimulateOption.KeyDown),
            (Key.B, KeyboardSimulateOption.KeyUp)
        }, keyboard.SimulatedInputs);
    }

    [Fact]
    public void KeyboardBinder_UnbindWhileKeyIsHeld_ReleasesTheKeyThatWasPressed()
    {
        var keyboard = new FakeKeyboard();
        using var binder = new KeyboardBinderInterceptor(keyboard);
        binder.Bind(Key.A, Key.B);

        keyboard.Send(Key.A, KeyboardInputEvent.KeyDown);
        binder.Unbind(Key.A);

        Assert.Equal(new[]
        {
            (Key.B, KeyboardSimulateOption.KeyDown),
            (Key.B, KeyboardSimulateOption.KeyUp)
        }, keyboard.SimulatedInputs);
    }

    [Fact]
    public void KeyboardBinder_KeyThatIsNotBound_GoesThrough()
    {
        var keyboard = new FakeKeyboard();
        using var binder = new KeyboardBinderInterceptor(keyboard);
        binder.Bind(Key.A, Key.B);

        Assert.True(keyboard.Send(Key.C, KeyboardInputEvent.KeyDown));
        Assert.True(keyboard.Send(Key.C, KeyboardInputEvent.KeyUp));
        Assert.Empty(keyboard.SimulatedInputs);
    }
}
