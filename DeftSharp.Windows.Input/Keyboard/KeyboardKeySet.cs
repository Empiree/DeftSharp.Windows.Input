using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides collections of specific keyboard keys.
/// </summary>
public static class KeyboardKeySet
{
    public static IEnumerable<Key> NumpadKeys => new[]
    {
        Key.NumPad7, Key.NumPad8, Key.NumPad9,
        Key.NumPad4, Key.NumPad5, Key.NumPad6,
        Key.NumPad1, Key.NumPad2, Key.NumPad3,
        Key.NumPad0
    };

    public static IEnumerable<Key> NumberKeys => new[]
    {
        Key.D1, Key.D2, Key.D3, Key.D4, Key.D5, Key.D6, Key.D7, Key.D8, Key.D9, Key.D0,
        Key.NumPad7, Key.NumPad8, Key.NumPad9,
        Key.NumPad4, Key.NumPad5, Key.NumPad6,
        Key.NumPad1, Key.NumPad2, Key.NumPad3,
        Key.NumPad0
    };

    public static IEnumerable<Key> FunctionKeys => new[]
    {
        Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6, Key.F7, Key.F8, Key.F9, Key.F10, Key.F11, Key.F12
    };

    public static IEnumerable<Key> ArrowKeys => new[]
    {
        Key.Up, Key.Right, Key.Down, Key.Left
    };

    public static IEnumerable<Key> NavigationKeys => new[]
    {
        Key.Home, Key.End, Key.PageUp, Key.PageDown
    };

    public static IEnumerable<Key> MultimediaKeys => new[]
    {
        Key.MediaPlayPause, Key.MediaStop, Key.MediaPreviousTrack, Key.MediaNextTrack,
        Key.VolumeUp, Key.VolumeDown, Key.VolumeMute
    };

    public static IEnumerable<Key> EditingKeys => new[]
    {
        Key.Back, Key.Enter, Key.LeftShift, Key.RightShift, Key.CapsLock, Key.Insert, Key.Delete
    };

    public static IEnumerable<Key> SpecialKeys => new[]
    {
        Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt, Key.Tab, Key.Escape
    };
}