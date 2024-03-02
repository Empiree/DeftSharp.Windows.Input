using System.Windows.Input;

namespace DeftSharp.Windows.Keyboard.Shared.Models;

public class NumpadButton
{
    public Key Key { get; }
    public int Number { get; }

    public NumpadButton(Key key, int number)
    {
        Key = key;
        Number = number;
    }
}