using static DeftSharp.Windows.Input.Native.KeyboardAPI;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the information about the keyboard.
/// </summary>
public sealed class KeyboardInfo : IKeyboardInfo
{
    /// <summary>
    /// Gets the current keyboard layout.
    /// </summary>
    public KeyboardLayout Layout => GetLayout();
    
    /// <summary>
    /// Gets the current keyboard type.
    /// </summary>
    public KeyboardType Type => GetKeyboardType();
}