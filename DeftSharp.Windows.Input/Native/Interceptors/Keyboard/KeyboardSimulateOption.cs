namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Specifies options for simulating keyboard button events.
/// </summary>
public enum KeyboardSimulateOption : ushort
{
    /// <summary>
    /// Simulates a key down event.
    /// </summary>
    KeyDown = 0x0000,
    
    /// <summary>
    /// Simulates a key up event.
    /// </summary>
    KeyUp = 0x0002
}