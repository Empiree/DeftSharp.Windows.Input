using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Extensions;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseManipulatorInterceptor : MouseInterceptor, IMouseManipulator
{
    private static readonly Lazy<MouseManipulatorInterceptor> LazyInstance =
        new(() => new MouseManipulatorInterceptor());

    public static MouseManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly HashSet<MouseEvent> _lockedKeys;
    private readonly object _lock = new();

    public event Action<MouseEvent>? ClickPrevented;

    public IEnumerable<MouseEvent> LockedKeys => _lockedKeys;

    public MouseManipulatorInterceptor()
        : base(WindowsMouseInterceptor.Instance)
    {
        _lockedKeys = new HashSet<MouseEvent>();
    }

    ~MouseManipulatorInterceptor()
    {
        Dispose();
    }

    public bool IsKeyLocked(MouseEvent mouseEvent) => _lockedKeys.Any(e => e == mouseEvent);
    public void SetPosition(int x, int y) => Mouse.SetPosition(x, y);
    public void Click(int x, int y, MouseButton button) => Mouse.Click(x, y, button);

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
        var preventEvents =  preventOption.ToMouseEvents();

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

    protected override bool OnInterceptorUnhookRequested() => !_lockedKeys.Any();

    protected override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(!IsKeyLocked(args.Event),
            InterceptorType.Manipulator,
            onPipelineFailed: failedInterceptors =>
            {
                if (failedInterceptors.Contains(InterceptorType.Manipulator))
                    ClickPrevented?.Invoke(args.Event);
            });
}