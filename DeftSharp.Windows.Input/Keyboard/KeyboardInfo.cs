using static DeftSharp.Windows.Input.Native.KeyboardAPI;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the information about the keyboard.
/// </summary>
public sealed class KeyboardInfo : IKeyboardInfo
{
    private KeyboardLayout? _layout;
    private KeyboardType? _type;

    /// <summary>
    /// Gets the current keyboard layout.
    /// </summary>
    public KeyboardLayout Layout
    {
        get
        {
            _layout ??= GetLayout();

            return _layout;
        }
    }

    /// <summary>
    /// Gets the current keyboard type.
    /// </summary>
    public KeyboardType Type
    {
        get
        {
            _type ??= GetKeyboardType();

            return _type;
        }
    }

}