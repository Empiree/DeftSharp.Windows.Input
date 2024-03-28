using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

internal sealed class KeyClickInterval
{
    public Key Key { get; }
    public TimeSpan Interval { get; }
    public DateTime? LastClicked { get; set; }
    public bool IsBlocked => LastClicked?.Add(Interval) > DateTime.Now;

    public KeyClickInterval(Key key, TimeSpan interval)
    {
        Key = key;
        Interval = interval;
    }
}