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

        await Task.Run(() =>
        {
            _scrollDisable.Hook();
            _mouseLog.Hook();

            _mouseManipulator.Click();
            _mouseManipulator.Click();

            //Assert.False(_mouseLog.WasEventBlocked);
            Assert.True(_mouseLog.WasEventCatched);
            
            
            _scrollDisable.Unhook();
            _mouseLog.Unhook();
        });

        
    }

    [Fact]
    public async void CustomInterceptor_BlockEvent()
    {
        ScrollDisabler _scrollDisable = new();
        MouseLogger _mouseLog = new();

        await Task.Run(() =>
        {
            _scrollDisable.Hook();
            _mouseLog.Hook();

            _mouseManipulator.Scroll(400);
            _mouseManipulator.Scroll(-800);

            //Assert.False(_mouseLog.WasEventCatched);
            Assert.True(_mouseLog.WasEventBlocked);


            _scrollDisable.Unhook();
            _mouseLog.Unhook();
        });


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
    internal bool WasEventCatched { get; private set; } = false;
    internal bool WasEventBlocked { get; private set; } = false;

    protected override bool IsInputAllowed(MouseInputArgs args) => true;

    protected override void OnInputSuccess(MouseInputArgs args)
    {
        WasEventCatched = true;
    }

    protected override void OnInputFailure(MouseInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
       WasEventBlocked = true;
    }
}

