using DeftSharp.Windows.Input.Native;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the information about the keyboard.
/// </summary>
public sealed class KeyboardInfo : IKeyboardInfo
{
    /// <summary>
    /// Gets the current keyboard layout.
    /// </summary>
    public KeyboardLayout GetLayout() => KeyboardAPI.GetLayout();

    /// <summary>
    /// Gets the current keyboard type.
    /// </summary>
    public KeyboardType GetKeyboardType() => KeyboardAPI.GetKeyboardType();
}