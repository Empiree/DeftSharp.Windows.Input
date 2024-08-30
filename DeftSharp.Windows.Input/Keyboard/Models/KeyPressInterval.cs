using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

internal sealed class KeyPressInterval(Key key, TimeSpan interval)
{
    public Key Key { get; } = key;
    public TimeSpan Interval { get; } = interval;
    public DateTime? LastPressed { get; set; }
    public bool IsBlocked => LastPressed?.Add(Interval) > DateTime.Now;
}