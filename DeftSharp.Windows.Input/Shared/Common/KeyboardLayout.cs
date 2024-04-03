namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Represents a keyboard layout.
/// </summary>
public sealed class KeyboardLayout
{
    /// <summary>
    /// Gets the identifier of the keyboard layout.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Gets the locale identifier (LCID) of the keyboard layout.
    /// </summary>
    public int LocaleId { get; }

    /// <summary>
    /// Gets the display name of the keyboard layout.
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// Gets the name of the keyboard layout.
    /// </summary>
    public string Name { get; }

    public KeyboardLayout(int id, int localeId, string name, string displayName)
    {
        Id = id;
        LocaleId = localeId;
        Name = name;
        DisplayName = displayName;
    }
}