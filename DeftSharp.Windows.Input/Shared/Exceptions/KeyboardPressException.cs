using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyboardPressException : Exception
{
    public KeyboardPressException(Key key)
    : base($"Failed to simulate pressing the '{key}' button") { }
}