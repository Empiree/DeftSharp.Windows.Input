using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyNoneException : Exception
{
    public KeyNoneException()
        : base("You can't subscribe to the Key.None event") { }
}