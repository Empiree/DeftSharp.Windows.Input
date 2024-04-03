using System.Text;
using System.Windows.Input;
using DeftSharp.Windows.Input.Native;

namespace DeftSharp.Windows.Input.Keyboard;

public static class KeyboardExtensions
{
    /// <summary>
    /// Converts the specified Key value to its corresponding Unicode representation.
    /// </summary>
    public static string ToUnicode(this Key key)
    {
        var virtualKeyCode = (uint)KeyInterop.VirtualKeyFromKey(key);
        var handle = KeyboardAPI.GetLayoutHandle();
        var scanCode = User32.MapVirtualKey(virtualKeyCode, 0);
        
        var buffer = new StringBuilder(5);
        var keyboardState = new byte[256];
        
        var result = User32.ToUnicodeEx(virtualKeyCode, scanCode, keyboardState, buffer, buffer.Capacity, 0, handle);

        return result > 0 ? buffer.ToString() : string.Empty;
    }
}