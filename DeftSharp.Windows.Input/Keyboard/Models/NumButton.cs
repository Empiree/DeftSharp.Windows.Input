using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

internal sealed class NumButton(Key key, int number)
{
    public Key Key { get; } = key;
    public int Number { get; } = number;
}