using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Abstraction.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal sealed class MouseManipulatorInterceptor : MouseInterceptor, IMouseManipulator
{
    private static readonly Lazy<MouseManipulatorInterceptor> LazyInstance =
        new(() => new MouseManipulatorInterceptor());
    public static MouseManipulatorInterceptor Instance => LazyInstance.Value;
    
    private readonly HashSet<MouseEvent> _lockedKeys;
    private readonly object _lock = new();

    public event Action<MouseEvent>? ClickPrevented;
    public event Action<MouseEvent>? ClickReleased;

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

    public void Prevent(MouseEvent mouseEvent)
    {
        lock (_lock)
        {
            if (_lockedKeys.Any(e => e == mouseEvent))
                return;

            Hook();
            _lockedKeys.Add(mouseEvent);
            ClickPrevented?.Invoke(mouseEvent);
        }
    }

    public void Release(MouseEvent mouseEvent)
    {
        lock (_lock)
        {
            if (_lockedKeys.All(e => e != mouseEvent))
                return;

            _lockedKeys.Remove(mouseEvent);
            ClickReleased?.Invoke(mouseEvent);

            if (!_lockedKeys.Any())
                Unhook();
        }
    }

    public void ReleaseAll()
    {
        var events = _lockedKeys.ToArray();

        foreach (var mouseEvent in events)
            Release(mouseEvent);
    }

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !_lockedKeys.Any();

    protected override InterceptorResponse OnMouseInput(MouseInputArgs args) =>
        new(!IsKeyLocked(args.Event), MiddlewareInterceptor.Manipulator,() => { });
}