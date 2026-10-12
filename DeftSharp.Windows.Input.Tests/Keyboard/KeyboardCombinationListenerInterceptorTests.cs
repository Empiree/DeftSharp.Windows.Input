using System.Runtime.CompilerServices;
using DeftSharp.Windows.Input.Keyboard.Interceptors;
using DeftSharp.Windows.Input.Tests.Fakes;
using Microsoft.Win32;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardCombinationListenerInterceptorTests
{
    [Fact]
    public void KeyboardCombination_KeysPressedTogether_SubscriptionInvoked()
    {
        var keyboard = new FakeKeyboard();
        using var interceptor = new KeyboardCombinationListenerInterceptor(keyboard);
        var invoked = 0;
        interceptor.Subscribe(new KeyCombinationSubscription(new[] { Key.LeftCtrl, Key.M }, () => invoked++));

        keyboard.Send(Key.LeftCtrl, KeyboardInputEvent.KeyDown);
        keyboard.Send(Key.M, KeyboardInputEvent.KeyDown);

        Assert.Equal(1, invoked);
    }

    [Fact]
    public void KeyboardCombination_KeyHeldWhenSessionLocks_IsNotHeldAfterUnlock()
    {
        var keyboard = new FakeKeyboard();
        using var interceptor = new KeyboardCombinationListenerInterceptor(keyboard);
        var invoked = 0;
        interceptor.Subscribe(new KeyCombinationSubscription(new[] { Key.LeftCtrl, Key.M }, () => invoked++));

        // Ctrl is down when the session gets locked, so its KeyUp is never seen
        keyboard.Send(Key.LeftCtrl, KeyboardInputEvent.KeyDown);
        interceptor.OnSessionSwitch(SessionSwitchReason.SessionLock);
        interceptor.OnSessionSwitch(SessionSwitchReason.SessionUnlock);

        keyboard.Send(Key.M, KeyboardInputEvent.KeyDown);
        Assert.Equal(0, invoked);

        // The subscription is back after the unlock
        keyboard.Send(Key.LeftCtrl, KeyboardInputEvent.KeyDown);
        Assert.Equal(1, invoked);
    }

    [Fact]
    public void KeyboardCombination_DisposedInterceptor_CanBeCollected()
    {
        var interceptor = CreateDisposedInterceptor();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        Assert.False(interceptor.IsAlive);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static WeakReference CreateDisposedInterceptor()
    {
        var interceptor = new KeyboardCombinationListenerInterceptor(new FakeKeyboard());
        interceptor.Dispose();

        return new WeakReference(interceptor);
    }
}
