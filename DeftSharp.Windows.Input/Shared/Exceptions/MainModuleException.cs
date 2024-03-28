using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class MainModuleException : Exception
{
    public MainModuleException()
        : base("Main module is not exist.") { }
}