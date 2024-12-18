namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents the different types of keyboard layouts.
/// </summary>
public enum KeyboardLayoutType
{
    /// <summary>
    /// The QWERTY keyboard layout, the most common layout used in English-speaking countries.
    /// </summary>
    Qwerty,

    /// <summary>
    /// The AZERTY keyboard layout, primarily used in French-speaking regions, particularly in France and Belgium.
    /// </summary>
    Azerty,

    /// <summary>
    /// The Dvorak keyboard layout, designed to increase typing speed by placing the most common letters under the strongest fingers.
    /// </summary>
    Dvorak,

    /// <summary>
    /// The Colemak keyboard layout, designed as a middle ground between QWERTY and Dvorak, with fewer key changes.
    /// </summary>
    Colemak,

    /// <summary>
    /// The Workman keyboard layout, designed to minimize finger movement and improve typing efficiency.
    /// </summary>
    Workman,

    /// <summary>
    /// The Norman keyboard layout, created as an alternative to QWERTY for increased typing speed.
    /// </summary>
    Norman
}