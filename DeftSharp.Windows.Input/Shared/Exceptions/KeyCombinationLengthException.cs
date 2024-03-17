using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyCombinationLengthException : Exception
{
    public KeyCombinationLengthException(string message) 
        : base(message) { }
}