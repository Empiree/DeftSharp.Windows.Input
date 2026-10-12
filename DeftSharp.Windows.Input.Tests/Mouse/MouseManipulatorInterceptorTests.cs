using DeftSharp.Windows.Input.Mouse.Interceptors;
using DeftSharp.Windows.Input.Tests.Fakes;

namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class MouseManipulatorInterceptorTests
{
    [Fact]
    public void MouseManipulator_PreventWhileButtonIsDown_ReleaseGoesThrough()
    {
        var mouse = new FakeMouse();
        using var manipulator = new MouseManipulatorInterceptor(mouse);

        // A drag starts before the button gets locked
        mouse.Send(MouseInputEvent.LeftButtonDown);
        manipulator.Prevent(MousePreventOption.LeftButton, () => true);

        Assert.True(mouse.Send(MouseInputEvent.LeftButtonUp));
    }

    [Fact]
    public void MouseManipulator_ReleaseLockWhileButtonIsDown_NextClickGoesThrough()
    {
        var mouse = new FakeMouse();
        using var manipulator = new MouseManipulatorInterceptor(mouse);

        manipulator.Prevent(MousePreventOption.LeftButton, () => true);
        mouse.Send(MouseInputEvent.LeftButtonDown);

        // The lock is removed while the button is down, so the manipulator does not see its release
        manipulator.Release(MousePreventOption.LeftButton);
        mouse.Send(MouseInputEvent.LeftButtonUp);

        manipulator.Prevent(MousePreventOption.RightButton, () => true);

        Assert.True(mouse.Send(MouseInputEvent.LeftButtonDown));
        Assert.True(mouse.Send(MouseInputEvent.LeftButtonUp));
    }

    [Fact]
    public void MouseManipulator_PreventedPress_ReleaseIsBlocked()
    {
        var mouse = new FakeMouse();
        using var manipulator = new MouseManipulatorInterceptor(mouse);

        manipulator.Prevent(MousePreventOption.LeftButton, () => true);

        Assert.False(mouse.Send(MouseInputEvent.LeftButtonDown));
        Assert.False(mouse.Send(MouseInputEvent.LeftButtonUp));
    }
}
