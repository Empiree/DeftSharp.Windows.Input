using System;

namespace DeftSharp.Windows.Keyboard.Shared.Exceptions;

public class MainModuleException : Exception
{
    public MainModuleException()
    : base("Main module is not exist.")
    {
        
    }
}