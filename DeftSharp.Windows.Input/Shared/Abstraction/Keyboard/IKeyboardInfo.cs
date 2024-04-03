namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardInfo
{
    KeyboardLayout Layout { get; }
    KeyboardType Type { get; }
}