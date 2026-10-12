using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Tests.Fakes;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardManipulatorInterceptorTests
{
    [Fact]
    public void KeyboardManipulator_PreventWhileKeyIsDown_ReleaseGoesThrough()
    {
        var keyboard = new FakeKeyboard();
        using var manipulator = new KeyboardManipulatorInterceptor(keyboard);

        // The key goes down before it gets locked
        keyboard.Send(Key.A, KeyboardInputEvent.KeyDown);
        manipulator.Prevent(Key.A, () => true);

        Assert.True(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));
    }

    [Fact]
    public void KeyboardManipulator_PreventDuringAutoRepeat_ReleaseGoesThrough()
    {
        var keyboard = new FakeKeyboard();
        using var manipulator = new KeyboardManipulatorInterceptor(keyboard);

        // The system already has the key down, so the next KeyDown is an auto-repeat
        keyboard.SetPressed(Key.A);
        manipulator.Prevent(Key.A, () => true);

        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.True(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));
    }

    [Fact]
    public void KeyboardManipulator_ReleaseLockWhileKeyIsDown_NextPressGoesThrough()
    {
        var keyboard = new FakeKeyboard();
        using var manipulator = new KeyboardManipulatorInterceptor(keyboard);

        manipulator.Prevent(Key.A, () => true);
        keyboard.Send(Key.A, KeyboardInputEvent.KeyDown);

        // The lock is removed while the key is down, so the manipulator does not see its KeyUp
        manipulator.Release(Key.A);
        keyboard.Send(Key.A, KeyboardInputEvent.KeyUp);

        manipulator.Prevent(Key.B, () => true);

        Assert.True(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.True(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));
    }

    [Fact]
    public void KeyboardManipulator_PreventedPress_ReleaseIsBlocked()
    {
        var keyboard = new FakeKeyboard();
        using var manipulator = new KeyboardManipulatorInterceptor(keyboard);

        manipulator.Prevent(Key.A, () => true);

        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyDown));
        Assert.False(keyboard.Send(Key.A, KeyboardInputEvent.KeyUp));
    }
}
