using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeySequenceLengthException : Exception
{
    public KeySequenceLengthException(string message) 
        : base(message) { }
}