using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyCombinationLengthException(string message) : Exception(message);