namespace DeftSharp.Windows.Input.Keyboard;

public interface IKeyboardInfo
{
    KeyboardLayout GetLayout();
    KeyboardType GetKeyboardType();
}