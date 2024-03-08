using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;
using DeftSharp.Windows.Input.Shared.Interceptors.Mouse;

namespace DeftSharp.Windows.Input.Mouse.Interceptors;

internal class MouseManipulatorInterceptor : IMouseManipulatorInterceptor
{
    #region Singleton

    private static readonly Lazy<MouseManipulatorInterceptor> LazyInstance =
        new(() => new MouseManipulatorInterceptor());

    public static MouseManipulatorInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly IMouseInterceptor _mouseInterceptor;
    private readonly HashSet<MouseEvent> _lockedKeys;
    private readonly object _lock = new();

    public event Action<MouseEvent>? ClickPrevented;
    public event Action<MouseEvent>? ClickReleased;
    public event Func<bool>? UnhookRequested;

    public IEnumerable<MouseEvent> LockedKeys => _lockedKeys;

    public MouseManipulatorInterceptor()
    {
        _lockedKeys = new HashSet<MouseEvent>();
        _mouseInterceptor = WindowsMouseInterceptor.Instance;
        _mouseInterceptor.MouseProcessing += OnKeyProcessing;
        _mouseInterceptor.UnhookRequested += OnInterceptorUnhookRequested;
    }

    ~MouseManipulatorInterceptor()
    {
        Dispose();
    }

    public bool IsKeyLocked(MouseEvent mouseEvent) => _lockedKeys.Any(e => e == mouseEvent);
    public void SetPosition(int x, int y) => _mouseInterceptor.SetPosition(x, y);
    public void Click(int x, int y, MouseButton button) => _mouseInterceptor.Click(x, y, button);
    public void Hook() => _mouseInterceptor.Hook();
    public void Unhook() => _mouseInterceptor.Unhook();

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

    public void Dispose()
    {
        ReleaseAll();
        _mouseInterceptor.MouseProcessing -= OnKeyProcessing;
        _mouseInterceptor.UnhookRequested -= OnInterceptorUnhookRequested;
    }

    private bool OnInterceptorUnhookRequested() => (UnhookRequested?.Invoke() ?? true) && !_lockedKeys.Any();

    private InterceptorResponse OnKeyProcessing(MouseInputArgs args) =>
        new(!IsKeyLocked(args.Event), () => { });
}