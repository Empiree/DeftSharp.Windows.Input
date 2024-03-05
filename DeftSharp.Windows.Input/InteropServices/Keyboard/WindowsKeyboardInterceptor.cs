using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.InteropServices.Keyboard;

/// <summary>
/// Listens for system keyboard events and raises an event when a key is pressed.
/// </summary>
public sealed class WindowsKeyboardInterceptor : WindowsInterceptor, IKeyboardInterceptor, IDisposable
{
    #region Singleton

    private static readonly Lazy<WindowsKeyboardInterceptor> LazyInstance = new(() => new WindowsKeyboardInterceptor());
    public static WindowsKeyboardInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly HashSet<Key> _lockedKeys;

    public IEnumerable<Key> LockedKeys => _lockedKeys;
    
    /// <summary>
    /// Occurs when a key is pressed.
    /// </summary>
    public event EventHandler<KeyPressedArgs>? KeyPressed;

    public event Action<Key>? KeyPrevented;
    public event Action<Key>? KeyReleased;

    private WindowsKeyboardInterceptor()
        : base(InputMessages.WhKeyboardLl)
    {
        _lockedKeys = new HashSet<Key>();
    }

    public void Prevent(Key key)
    {
        if (_lockedKeys.Any(k => k == key))
            return;

        _lockedKeys.Add(key);
        KeyPrevented?.Invoke(key);
    }

    public void Release(Key key)
    {
        if (_lockedKeys.All(k => k != key))
            return;

        _lockedKeys.Remove(key);
        KeyReleased?.Invoke(key);
    }

    public void ReleaseAll()
    {
        var keys = _lockedKeys.ToArray();

        foreach (var key in keys)
            Release(key);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        ReleaseAll();
        Unhook();
    }

    /// <summary>
    /// Callback method for the keyboard hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected override nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if ((nCode < 0 || !InputMessages.IsKeyboardEvent(wParam)) && wParam != InputMessages.WmSystemKeyDown)
            return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);

        var virtualKeyCode = Marshal.ReadInt32(lParam);
        var key = KeyInterop.KeyFromVirtualKey(virtualKeyCode);
        var keyEvent = (KeyboardEvent)wParam;

        if (IsKeyLocked(key))
            return 1;

        var keyPressedArgs = new KeyPressedArgs(key, keyEvent);

        KeyPressed?.Invoke(this, keyPressedArgs);

        return WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam);
    }

    private bool IsKeyLocked(Key key) => _lockedKeys.Any(k => k == key);
}