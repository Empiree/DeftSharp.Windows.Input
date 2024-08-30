using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeySequenceLengthException(string message) : Exception(message);