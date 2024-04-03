using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Extensions;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseManipulatorInterceptor : MouseInterceptor
{
    private static readonly Lazy<MouseManipulatorInterceptor> LazyInstance =
        new(() => new MouseManipulatorInterceptor());

    public static MouseManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly HashSet<MouseInputEvent> _lockedKeys;
    private readonly object _lock = new();

    public event Action<MouseInputEvent>? InputPrevented;

    public IEnumerable<MouseInputEvent> LockedKeys => _lockedKeys;

    public MouseManipulatorInterceptor()
        : base(InterceptorType.Prohibitive)
    {
        _lockedKeys = new HashSet<MouseInputEvent>();
    }

    public bool IsKeyLocked(MouseInputEvent mouseEvent) => _lockedKeys.Any(e => e == mouseEvent);
    public void SetPosition(int x, int y) => Mouse.SetPosition(x, y);
    public void Click(MouseButton button, int x, int y) => Mouse.Click(x, y, button);

    public void Click(MouseButton button)
    {
        var position = Mouse.GetPosition();
        Click(button, position.X, position.Y);
    }

    public void Scroll(int scrollAmount) => Mouse.Scroll(scrollAmount);

    public void Prevent(PreventMouseOption preventOption)
    {
        var preventEvents = preventOption.ToMouseEvents();

        lock (_lock)
        {
            Hook();

            foreach (var preventEvent in preventEvents)
            {
                if (_lockedKeys.Any(e => e == preventEvent))
                    continue;

                _lockedKeys.Add(preventEvent);
            }
        }
    }

    public void Release(PreventMouseOption preventOption)
    {
        var preventEvents = preventOption.ToMouseEvents();

        lock (_lock)
        {
            foreach (var preventEvent in preventEvents)
            {
                if (_lockedKeys.All(e => e != preventEvent))
                    continue;

                _lockedKeys.Remove(preventEvent);
            }

            if (!_lockedKeys.Any())
                Unhook();
        }
    }

    public void ReleaseAll()
    {
        if (!_lockedKeys.Any())
            return;

        _lockedKeys.Clear();
        Unhook();
    }

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_lockedKeys.Any();
    protected override bool IsInputAllowed(MouseInputArgs args) => !IsKeyLocked(args.Event);

    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            InputPrevented?.Invoke(args.Event);
    }
}