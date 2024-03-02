using System;

namespace DeftSharp.Windows.Keyboard.Shared.Exceptions;

public class KeyboardListenerException : Exception
{
    public KeyboardListenerException(string message)
    : base(message)
    {
    }
}