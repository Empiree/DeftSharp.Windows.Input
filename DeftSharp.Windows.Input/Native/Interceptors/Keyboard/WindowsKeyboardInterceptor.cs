using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Native.Interceptors;

/// <summary>
/// Listens for system keyboard events and raises an event when a key is pressed.
/// </summary>
internal sealed class WindowsKeyboardInterceptor : WindowsInterceptor, INativeKeyboardInterceptor
{
    // Specifies a low-level keyboard input event.
    private const int WhKeyboardLl = 13;

    private static readonly Lazy<WindowsKeyboardInterceptor> LazyInstance = new(() => new WindowsKeyboardInterceptor());
    public static WindowsKeyboardInterceptor Instance => LazyInstance.Value;

    public event KeyboardInputDelegate? KeyboardInput;

    private WindowsKeyboardInterceptor()
        : base(WhKeyboardLl)
    {
    }

    public KeyboardLayout GetLayout() => KeyboardAPI.GetLayout();
    public KeyboardType GetKeyboardType() => KeyboardAPI.GetKeyboardType();

    public void Press(Key key) => KeyboardAPI.Press(key);

    public void Press(IEnumerable<Key> keys) =>
        KeyboardAPI.PressSynchronously(keys);

    public bool IsKeyActive(Key key) => KeyboardAPI.IsKeyActive(key);
    public bool IsKeyPressed(Key key) => KeyboardAPI.IsKeyPressed(key);

    /// <summary>
    /// Callback method for the keyboard hook.
    /// </summary>
    /// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
    /// <param name="wParam">Specifies additional information about the message.</param>
    /// <param name="lParam">Specifies additional information about the message.</param>
    /// <returns>The return value of the next hook procedure in the chain.</returns>
    protected override nint HookCallback(int nCode, nint wParam, nint lParam)
    {
        if ((nCode < 0 || !SystemEvents.IsKeyboardEvent(wParam)) && wParam != SystemEvents.WmSystemKeyDown)
            return User32.CallNextHookEx(HookId, nCode, wParam, lParam);

        var virtualKeyCode = Marshal.ReadInt32(lParam);
        var key = KeyInterop.KeyFromVirtualKey(virtualKeyCode);
        var keyEvent = (KeyboardInputEvent)wParam;
        var keyPressedArgs = new KeyPressedArgs(key, keyEvent);

        var pipeline = CreatePipeline(keyPressedArgs);

        if (pipeline is null)
            return User32.CallNextHookEx(HookId, nCode, wParam, lParam);

        pipeline.RunAsync();

        return pipeline.IsAllowed
            ? User32.CallNextHookEx(HookId, nCode, wParam, lParam)
            : 1;
    }

    /// <summary>
    /// Checks whether the provided key press event can be processed by the registered middleware.
    /// </summary>
    /// <param name="args">The <see cref="KeyPressedArgs"/> representing the key press event.</param>
    /// <returns>True if the event can be processed; otherwise, false.</returns>
    private InterceptorPipeline? CreatePipeline(KeyPressedArgs args)
    {
        if (KeyboardInput is null)
        {
            Unhook();
            return null;
        }

        var interceptors = new List<InterceptorResponse>();

        foreach (var next in KeyboardInput.GetInvocationList())
        {
            var interceptor = ((KeyboardInputDelegate)next).Invoke(args);
            interceptors.Add(interceptor);
        }

        return new InterceptorPipeline(interceptors);
    }
}