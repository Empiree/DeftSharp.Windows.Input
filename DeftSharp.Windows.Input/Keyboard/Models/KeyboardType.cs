namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the type of keyboard hardware.
/// </summary>
public sealed class KeyboardType(int value, string name)
{
    /// <summary>
    /// Gets the numeric value representing the keyboard type.
    /// </summary>
    public int Value { get; } = value;

    /// <summary>
    /// Gets the name of the keyboard type.
    /// </summary>
    public string Name { get; } = name;
}