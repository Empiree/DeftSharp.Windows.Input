using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Shared.Delegates;
using MouseButton = DeftSharp.Windows.Input.Mouse.MouseButton;

namespace DeftSharp.Windows.Input.Tests.Fakes;

/// <summary>
/// Replaces the system mouse hook, so tests can send input through interceptors without touching the real mouse.
/// </summary>
internal sealed class FakeMouse : INativeMouseInterceptor
{
    public event MouseInputDelegate? MouseInput;

    public event Func<bool>? UnhookRequested
    {
        add { }
        remove { }
    }

    /// <summary>
    /// Sends the input through the hooked interceptors the same way the system hook does.
    /// </summary>
    /// <returns>True if the input is allowed; false if it is blocked.</returns>
    public bool Send(MouseInputEvent inputEvent)
    {
        var args = new MouseInputArgs(inputEvent);

        var responses = MouseInput?.GetInvocationList()
            .Cast<MouseInputDelegate>()
            .Select(handler => handler(args))
            .ToList() ?? new List<InterceptorResponse>();

        var pipeline = new InterceptorPipeline(responses);
        pipeline.Run();

        return pipeline.IsAllowed;
    }

    public bool IsKeyPressed(MouseButton button) => false;

    public Point GetPosition() => new(0, 0);

    public void Hook()
    {
    }

    public void Unhook()
    {
    }

    public void Dispose()
    {
    }

    public void SetPosition(int x, int y) => throw new NotSupportedException();

    public void SetMouseSpeed(int speed) => throw new NotSupportedException();

    public void Click(int x, int y, MouseButton button) => throw new NotSupportedException();

    public void Simulate(int x, int y, MouseSimulateOption option) => throw new NotSupportedException();

    public void Scroll(int rotation) => throw new NotSupportedException();
}
