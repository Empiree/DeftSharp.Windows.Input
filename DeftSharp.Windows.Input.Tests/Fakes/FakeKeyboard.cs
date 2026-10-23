using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Tests.Fakes;

/// <summary>
/// Replaces the system keyboard hook, so tests can send input through interceptors without touching the real keyboard.
/// </summary>
internal sealed class FakeKeyboard : INativeKeyboardInterceptor
{
    private readonly HashSet<Key> _pressedKeys = new();

    public event KeyboardInputDelegate? KeyboardInput;

    public event Func<bool>? UnhookRequested
    {
        add { }
        remove { }
    }

    /// <summary>
    /// Keyboard input that interceptors sent to the system, in order.
    /// </summary>
    public List<(Key Key, KeyboardSimulateOption Option)> SimulatedInputs { get; } = new();

    /// <summary>
    /// Sends the input through the hooked interceptors the same way the system hook does.
    /// </summary>
    /// <returns>True if the input is allowed; false if it is blocked.</returns>
    public bool Send(Key key, KeyboardInputEvent inputEvent, bool isBinderInput = false)
    {
        var args = new KeyboardInputArgs(key, inputEvent) { IsBinderInput = isBinderInput };

        var responses = KeyboardInput?.GetInvocationList()
            .Cast<KeyboardInputDelegate>()
            .Select(handler => handler(args))
            .ToList() ?? new List<InterceptorResponse>();

        var pipeline = new InterceptorPipeline(responses);
        pipeline.Run();

        return pipeline.IsAllowed;
    }

    /// <summary>
    /// Makes the key look held down in the system.
    /// </summary>
    public void SetPressed(Key key) => _pressedKeys.Add(key);

    public bool IsKeyPressed(Key key) => _pressedKeys.Contains(key);

    public bool IsKeyActive(Key key) => false;

    public void Hook()
    {
    }

    public void Unhook()
    {
    }

    public void Dispose()
    {
    }

    public void Press(Key key)
    {
        SimulatedInputs.Add((key, KeyboardSimulateOption.KeyDown));
        SimulatedInputs.Add((key, KeyboardSimulateOption.KeyUp));
    }

    public void Press(IEnumerable<Key> keys) => throw new NotSupportedException();

    public void Simulate(Key key, KeyboardSimulateOption option) => throw new NotSupportedException();

    public void SimulateBinderInput(Key key, KeyboardSimulateOption option) => SimulatedInputs.Add((key, option));

    public KeyboardLayout GetLayout() => throw new NotSupportedException();

    public KeyboardType GetKeyboardType() => throw new NotSupportedException();
}
