using System.Runtime.InteropServices;
using DeftSharp.Windows.Input.Native;
using DeftSharp.Windows.Input.Native.Interceptors;
using DeftSharp.Windows.Input.Native.System;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class KeyboardHookTests
{
    private const int WmKeyDown = 0x0100;
    private const uint InjectedFlag = 0x10;

    [Theory]
    [InlineData(InjectedFlag, true, true)]
    [InlineData(0u, true, false)]
    [InlineData(InjectedFlag, false, false)]
    public void KeyboardHook_CreateInputArgs_DetectsBinderInput(uint flags, bool withBinderMark, bool expected)
    {
        var hookData = new KeyboardHookStruct
        {
            vkCode = 0x41,
            flags = flags,
            dwExtraInfo = withBinderMark ? KeyboardAPI.BinderInputExtraInfo : 0
        };

        var pointer = Marshal.AllocHGlobal(Marshal.SizeOf<KeyboardHookStruct>());
        try
        {
            Marshal.StructureToPtr(hookData, pointer, false);

            var args = WindowsKeyboardInterceptor.CreateInputArgs(WmKeyDown, pointer);

            Assert.Equal(Key.A, args.KeyPressed);
            Assert.Equal(KeyboardInputEvent.KeyDown, args.Event);
            Assert.Equal(expected, args.IsBinderInput);
        }
        finally
        {
            Marshal.FreeHGlobal(pointer);
        }
    }
}
