using DeftSharp.Windows.Input.Native;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardInputEventTests
{
    private const int WmKeyDown = 0x0100;
    private const int WmKeyUp = 0x0101;
    private const int WmSysKeyDown = 0x0104;
    private const int WmSysKeyUp = 0x0105;
    private const int WmMouseMove = 0x0200;

    // F10 and keys pressed while Alt is held down (Alt itself too) come as WM_SYSKEYDOWN and WM_SYSKEYUP.
    [Theory]
    [InlineData(WmKeyDown)]
    [InlineData(WmKeyUp)]
    [InlineData(WmSysKeyDown)]
    [InlineData(WmSysKeyUp)]
    public void KeyboardMessage_IsKeyboardEvent(int message)
    {
        Assert.True(SystemEvents.IsKeyboardEvent(message));
    }

    [Theory]
    [InlineData(WmKeyDown, KeyboardInputEvent.KeyDown)]
    [InlineData(WmKeyUp, KeyboardInputEvent.KeyUp)]
    [InlineData(WmSysKeyDown, KeyboardInputEvent.KeyDown)]
    [InlineData(WmSysKeyUp, KeyboardInputEvent.KeyUp)]
    public void KeyboardMessage_ToKeyboardInputEvent(int message, KeyboardInputEvent expected)
    {
        Assert.Equal(expected, SystemEvents.ToKeyboardInputEvent(message));
    }

    [Fact]
    public void MouseMessage_IsNotKeyboardEvent()
    {
        Assert.False(SystemEvents.IsKeyboardEvent(WmMouseMove));
    }
}
