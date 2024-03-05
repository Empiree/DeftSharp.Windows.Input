using System;
using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.InteropServices.Mouse;

public sealed class WindowsMouseInterceptor : WindowsInterceptor, IMouseInterceptor, IDisposable
{
    #region Singleton

    private static readonly Lazy<WindowsMouseInterceptor> LazyInstance = new(() => new WindowsMouseInterceptor());
    public static WindowsMouseInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly HashSet<MouseEvent> _lockedKeys;

    public IEnumerable<MouseEvent> LockedKeys => _lockedKeys;

    public event Action<MouseEvent>? ClickPrevented;
    public event Action<MouseEvent>? ClickReleased;
    public event EventHandler<MouseInputArgs>? MouseInput;

    private WindowsMouseInterceptor()
        : base(InputMessages.WhMouseLl)
    {
        _lockedKeys = new HashSet<MouseEvent>();
    }

    public Coordinates GetPosition()
    {
        WinAPI.GetCursorPos(out var position);
        return position;
    }

    public void Prevent(MouseEvent mouseEvent)
    {
        if (_lockedKeys.Any(e => e == mouseEvent))
            return;

        _lockedKeys.Add(mouseEvent);
        ClickPrevented?.Invoke(mouseEvent);
    }

    public void Release(MouseEvent mouseEvent)
    {
        if (_lockedKeys.All(e => e != mouseEvent))
            return;

        _lockedKeys.Remove(mouseEvent);
        ClickReleased?.Invoke(mouseEvent);
    }

    public void ReleaseAll()
    {
        var events = _lockedKeys.ToArray();

        foreach (var mouseEvent in events)
            Release(mouseEvent);
    }
    
    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        ReleaseAll();
        Unhook();
    }

    /// <summary>
    /// Callback method for the mouse hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected override nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if (nCode < 0 || !InputMessages.IsMouseEvent(wParam))
            return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);

        var mouseEvent = (MouseEvent)wParam;

        if (IsClickLocked(mouseEvent))
            return 1;

        var args = new MouseInputArgs(mouseEvent);

        MouseInput?.Invoke(this, args);

        return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);
    }

    private bool IsClickLocked(MouseEvent mouseEvent) => _lockedKeys.Any(e => e == mouseEvent);
}