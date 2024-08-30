using System;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyNoneException() : Exception("You can't subscribe to the Key.None event");