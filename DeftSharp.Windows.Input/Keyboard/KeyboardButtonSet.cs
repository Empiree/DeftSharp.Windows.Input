using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides collections of specific keyboard buttons.
/// </summary>
internal static class KeyboardButtonSet
{
    public static IEnumerable<NumButton> NumpadButtons => new NumButton[]
    {
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };

    public static IEnumerable<NumButton> NumberButtons => new NumButton[]
    {
        new(Key.D1, 1), new(Key.D2, 2), new(Key.D3, 3), new(Key.D4, 4), new(Key.D5, 5),
        new(Key.D6, 6), new(Key.D7, 7), new(Key.D8, 8), new(Key.D9, 9), new(Key.D0, 0),
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };
}