using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

public class KeyboardPressException(Key key) : Exception($"Failed to simulate pressing the '{key}' button");