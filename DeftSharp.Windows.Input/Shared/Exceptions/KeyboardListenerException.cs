using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyboardListenerException : Exception
{
    public KeyboardListenerException(string message)
    : base(message)
    {
    }
}