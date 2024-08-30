namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents a keyboard layout.
/// </summary>
public sealed class KeyboardLayout(int id, int localeId, string name, string displayName)
{
    /// <summary>
    /// Gets the identifier of the keyboard layout.
    /// </summary>
    public int Id { get; } = id;

    /// <summary>
    /// Gets the locale identifier (LCID) of the keyboard layout.
    /// </summary>
    public int LocaleId { get; } = localeId;

    /// <summary>
    /// Gets the display name of the keyboard layout.
    /// </summary>
    public string DisplayName { get; } = displayName;

    /// <summary>
    /// Gets the name of the keyboard layout.
    /// </summary>
    public string Name { get; } = name;
}