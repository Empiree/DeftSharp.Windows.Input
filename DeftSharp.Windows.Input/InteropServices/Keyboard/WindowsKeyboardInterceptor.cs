using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Delegates;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.InteropServices.Keyboard;

/// <summary>
/// Listens for system keyboard events and raises an event when a key is pressed.
/// </summary>
internal sealed class WindowsKeyboardInterceptor : WindowsInterceptor, IKeyboardInterceptor
{
    private static readonly Lazy<WindowsKeyboardInterceptor> LazyInstance = new(() => new WindowsKeyboardInterceptor());
    public static WindowsKeyboardInterceptor Instance => LazyInstance.Value;

    public event KeyboardInputDelegate? KeyboardInput;

    private WindowsKeyboardInterceptor()
        : base(InputMessages.WhKeyboardLl)
    {
    }

    public void Press(Key key) => KeyboardAPI.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => 
        KeyboardAPI.PressSynchronously(combination.Distinct());

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
        var keyPressedArgs = new KeyPressedArgs(key, keyEvent);

        return StartInterceptorPipeline(keyPressedArgs)
            ? WinAPI.CallNextHookEx(HookId, nCode, wParam, lParam)
            : 1;
    }

    /// <summary>
    /// Checks whether the provided key press event can be processed by the registered middleware.
    /// </summary>
    /// <param name="args">The <see cref="KeyPressedArgs"/> representing the key press event.</param>
    /// <returns>True if the event can be processed; otherwise, false.</returns>
    private bool StartInterceptorPipeline(KeyPressedArgs args)
    {
        if (KeyboardInput is null)
        {
            Unhook();
            return true;
        }

        var interceptors = new List<InterceptorResponse>();

        foreach (var next in KeyboardInput.GetInvocationList())
        {
            var interceptor = ((KeyboardInputDelegate)next).Invoke(args);
            interceptors.Add(interceptor);
        }

        return InterceptorPipeline.Run(interceptors);
    }
}