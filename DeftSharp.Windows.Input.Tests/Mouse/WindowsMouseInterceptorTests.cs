using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Interceptors;

namespace DeftSharp.Windows.Input.Tests.Mouse;

public sealed class WindowsMouseInterceptorTests
{
    private const int WmMouseMove = 0x0200;
    private const int WmLButtonDown = 0x0201;

    [Fact]
    public void MouseHook_RunsCallbacksAfterTheHookReturns()
    {
        RunOnStaThread(() =>
        {
            var mouse = new WindowsMouseInterceptor();
            var invoked = false;
            mouse.MouseInput += _ => new InterceptorResponse(true, new InterceptorInfo("Test", InterceptorType.Custom), () => invoked = true);

            SendHookEvent(mouse, WmMouseMove);

            // A slow callback must not hold the mouse of the whole system, so it runs later on the dispatcher
            Assert.False(invoked);

            Dispatcher.CurrentDispatcher.Invoke(() => { }, DispatcherPriority.ApplicationIdle);
            Assert.True(invoked);
        });
    }

    [Fact]
    public void MouseHook_BlocksInputRightAway()
    {
        RunOnStaThread(() =>
        {
            var mouse = new WindowsMouseInterceptor();
            mouse.MouseInput += _ => new InterceptorResponse(false, new InterceptorInfo("Test", InterceptorType.Custom));

            Assert.Equal(1, SendHookEvent(mouse, WmLButtonDown));
        });
    }

    private static nint SendHookEvent(WindowsMouseInterceptor mouse, int message)
    {
        // The hook data is not used for these events, an empty MSLLHOOKSTRUCT is enough
        var data = Marshal.AllocHGlobal(32);
        try
        {
            Marshal.Copy(new byte[32], 0, data, 32);
            return mouse.HookCallback(0, message, data);
        }
        finally
        {
            Marshal.FreeHGlobal(data);
        }
    }

    private static void RunOnStaThread(Action test)
    {
        Exception? failure = null;

        var thread = new Thread(() =>
        {
            try
            {
                test();
            }
            catch (Exception exception)
            {
                failure = exception;
            }
            finally
            {
                Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        });

        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();

        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
    }
}
