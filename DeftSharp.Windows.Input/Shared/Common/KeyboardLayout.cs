namespace DeftSharp.Windows.Input.Keyboard;

public sealed class KeyboardLayout
{
    public int Id { get; }

    public int LocaleId { get; }

    public string DisplayName { get; }

    public string Name { get; }

    public KeyboardLayout(int id, int localeId, string name, string displayName)
    {
        Id = id;
        LocaleId = localeId;
        Name = name;
        DisplayName = displayName;
    }
}