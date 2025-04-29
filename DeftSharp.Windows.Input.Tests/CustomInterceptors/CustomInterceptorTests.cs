using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Mouse.Interceptors;


namespace DeftSharp.Windows.Input.Tests.CustomInterceptors;

public sealed class CustomInterceptorTests
{
    private readonly MouseManipulator _mouseManipulator = new();


    [Fact]
    public async void CustomInterceptor_CatchEvent()
    {
        ScrollDisabler _scrollDisable = new();
        MouseLogger _mouseLog = new();

        _scrollDisable.Hook();
        _mouseLog.Hook();


        //Simulate a mouse click
        _mouseManipulator.Click();

        //Assert
        var exception = await _mouseLog.ExceptionThrown.Task.WaitAsync(TimeSpan.FromSeconds(1));
        
        Assert.NotNull(exception);
        Assert.IsType<EventCatchedException>(exception);


        _scrollDisable.Unhook();
        _mouseLog.Unhook();


    }

    [Fact]
    public async void CustomInterceptor_BlockEvent()
    {
        ScrollDisabler _scrollDisable = new();
        MouseLogger _mouseLog = new();

        _scrollDisable.Hook();
        _mouseLog.Hook();


        //Simulate a mouse scroll
        _mouseManipulator.Scroll(400);

        //Assert
        var exception = await _mouseLog.ExceptionThrown.Task.WaitAsync(TimeSpan.FromSeconds(1));

        Assert.NotNull(exception);
        Assert.IsType<EventBlockedException>(exception);


        _scrollDisable.Unhook();
        _mouseLog.Unhook();


    }


}

//Helper Custom Interceptor Classes
internal class ScrollDisabler : MouseInterceptor
{
    protected override bool IsInputAllowed(MouseInputArgs args)
    {
        if (args.Event is MouseInputEvent.Scroll)
            return false; 

        return true; 
    }
}

internal class MouseLogger : MouseInterceptor
{
    internal TaskCompletionSource<Exception> ExceptionThrown { get; } = new();

    protected override bool IsInputAllowed(MouseInputArgs args) => true;

    protected override void OnInputSuccess(MouseInputArgs args)
    {
        ExceptionThrown.TrySetResult(new EventCatchedException("Click Catched"));
    }

    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        ExceptionThrown.TrySetResult(new EventBlockedException("Scroll Blocked"));
    }
}

public class EventCatchedException : Exception
{
    public EventCatchedException(string message) : base(message)
    {
    }
}

public class EventBlockedException : Exception
{
    public EventBlockedException(string message) : base(message)
    {
    }
}

