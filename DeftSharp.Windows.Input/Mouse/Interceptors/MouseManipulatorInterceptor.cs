using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseManipulatorInterceptor : MouseInterceptor
{
    private static readonly Lazy<MouseManipulatorInterceptor> LazyInstance =
        new(() => new MouseManipulatorInterceptor());

    public static MouseManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly ConcurrentDictionary<MouseInputEvent, Func<bool>> _lockedKeys;

    // Button presses blocked here. Only their releases are blocked as well,
    // so a button that was already down when it got locked does not stay pressed in the system.
    private readonly HashSet<MouseInputEvent> _blockedPresses = new();

    public event Action<MouseInputEvent>? InputPrevented;

    public IEnumerable<MouseInputEvent> LockedKeys => _lockedKeys.Keys;

    public MouseManipulatorInterceptor()
        : this(WindowsMouseInterceptor.Instance)
    {
    }

    internal MouseManipulatorInterceptor(INativeMouseInterceptor mouse)
        : base(InterceptorType.Prohibitive, mouse) =>
        _lockedKeys = new ConcurrentDictionary<MouseInputEvent, Func<bool>>();

    public void SetPosition(int x, int y) => Mouse.SetPosition(x, y);
    public void SetMouseSpeed(int speed) => Mouse.SetMouseSpeed(speed);
    public void Click(int x, int y, MouseButton button) => Mouse.Click(x, y, button);

    public void Click(MouseButton button)
    {
        var position = Mouse.GetPosition();
        Click(position.X, position.Y, button);
    }

    public void Simulate(int x, int y, MouseSimulateOption simulateOption) => Mouse.Simulate(x, y, simulateOption);

    public void Simulate(MouseSimulateOption simulateOption)
    {
        var position = Mouse.GetPosition();
        Simulate(position.X, position.Y, simulateOption);
    }

    public void Scroll(int rotation) => Mouse.Scroll(rotation);

    public void Prevent(MousePreventOption preventOption, Func<bool> predicate)
    {
        var preventEvents = preventOption.ToMouseEvents();

        Hook();

        foreach (var inputEvent in preventEvents)
        {
            _lockedKeys.AddOrUpdate(inputEvent, predicate,
                (_, _) => predicate);
        }
    }

    public void Release(MousePreventOption preventOption)
    {
        var preventEvents = preventOption.ToMouseEvents();

        foreach (var inputEvent in preventEvents)
            _lockedKeys.TryRemove(inputEvent, out _);

        TryUnhook();
    }

    public void Release()
    {
        if (_lockedKeys.IsEmpty)
            return;

        _lockedKeys.Clear();
        Unhook();
    }

    public bool IsKeyLocked(MouseInputEvent mouseEvent)
    {
        _lockedKeys.TryGetValue(mouseEvent, out var predicate);

        return predicate is not null && predicate.Invoke();
    }

    public bool IsKeyLocked(MousePreventOption option) 
        => option.ToMouseEvents().Any(IsKeyLocked);

    public override void Dispose()
    {
        Release();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => _lockedKeys.IsEmpty;

    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        var inputEvent = args.Event;

        switch (inputEvent)
        {
            case MouseInputEvent.LeftButtonUp:
                return !_blockedPresses.Remove(MouseInputEvent.LeftButtonDown);
            case MouseInputEvent.RightButtonUp:
                return !_blockedPresses.Remove(MouseInputEvent.RightButtonDown);
            case MouseInputEvent.MiddleButtonUp:
                return !_blockedPresses.Remove(MouseInputEvent.MiddleButtonDown);
        }

        if (!IsKeyLocked(inputEvent))
        {
            _blockedPresses.Remove(inputEvent);
            return true;
        }

        if (inputEvent is MouseInputEvent.LeftButtonDown or MouseInputEvent.RightButtonDown or MouseInputEvent.MiddleButtonDown)
            _blockedPresses.Add(inputEvent);

        return false;
    }

    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            InputPrevented?.Invoke(args.Event);
    }

    private void TryUnhook()
    {
        if (_lockedKeys.IsEmpty)
            Unhook();
    }
}