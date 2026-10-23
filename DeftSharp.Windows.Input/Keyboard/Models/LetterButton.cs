using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

[Obsolete("Only used by the obsolete KeyboardLayouts tables.")]
public sealed class LetterButton(Key key, string letter)
{
    public Key Key { get; } = key;
    public string Letter { get; } = letter;
}