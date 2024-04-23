using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

internal sealed class KeyPressInterval
{
    public Key Key { get; }
    public TimeSpan Interval { get; }
    public DateTime? LastPressed { get; set; }
    public bool IsBlocked => LastPressed?.Add(Interval) > DateTime.Now;

    public KeyPressInterval(Key key, TimeSpan interval)
    {
        Key = key;
        Interval = interval;
    }
}