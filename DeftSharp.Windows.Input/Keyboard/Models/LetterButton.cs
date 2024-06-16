using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

public sealed class LetterButton
{
    public Key Key { get; }
    public string Letter { get; }

    public LetterButton(Key key, string letter)
    {
        Key = key;
        Letter = letter;
    }
}