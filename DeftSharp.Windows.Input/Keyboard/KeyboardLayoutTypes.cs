using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Keyboard;

/// <summary>
/// Provides predefined keyboard layouts.
/// </summary>
public static class KeyboardLayoutTypes
{
    public static readonly Dictionary<KeyboardLayoutType, LetterButton[]> Layouts = new()
    {
        [KeyboardLayoutType.Qwerty] =
        [
            new LetterButton(Key.A, "A"),
            new LetterButton(Key.B, "B"),
            new LetterButton(Key.C, "C"),
            new LetterButton(Key.D, "D"),
            new LetterButton(Key.E, "E"),
            new LetterButton(Key.F, "F"),
            new LetterButton(Key.G, "G"),
            new LetterButton(Key.H, "H"),
            new LetterButton(Key.I, "I"),
            new LetterButton(Key.J, "J"),
            new LetterButton(Key.K, "K"),
            new LetterButton(Key.L, "L"),
            new LetterButton(Key.M, "M"),
            new LetterButton(Key.N, "N"),
            new LetterButton(Key.O, "O"),
            new LetterButton(Key.P, "P"),
            new LetterButton(Key.Q, "Q"),
            new LetterButton(Key.R, "R"),
            new LetterButton(Key.S, "S"),
            new LetterButton(Key.T, "T"),
            new LetterButton(Key.U, "U"),
            new LetterButton(Key.V, "V"),
            new LetterButton(Key.W, "W"),
            new LetterButton(Key.X, "X"),
            new LetterButton(Key.Y, "Y"),
            new LetterButton(Key.Z, "Z")
        ],
        [KeyboardLayoutType.Azerty] = [
            new LetterButton(Key.Q, "A"),
            new LetterButton(Key.B, "B"),
            new LetterButton(Key.C, "C"),
            new LetterButton(Key.D, "D"),
            new LetterButton(Key.E, "E"),
            new LetterButton(Key.F, "F"),
            new LetterButton(Key.G, "G"),
            new LetterButton(Key.H, "H"),
            new LetterButton(Key.I, "I"),
            new LetterButton(Key.J, "J"),
            new LetterButton(Key.K, "K"),
            new LetterButton(Key.L, "L"),
            new LetterButton(Key.OemSemicolon, "M"),
            new LetterButton(Key.OemComma, "N"),
            new LetterButton(Key.O, "O"),
            new LetterButton(Key.P, "P"),
            new LetterButton(Key.A, "Q"),
            new LetterButton(Key.R, "R"),
            new LetterButton(Key.S, "S"),
            new LetterButton(Key.T, "T"),
            new LetterButton(Key.U, "U"),
            new LetterButton(Key.V, "V"),
            new LetterButton(Key.Z, "W"),
            new LetterButton(Key.X, "X"),
            new LetterButton(Key.Y, "Y"),
            new LetterButton(Key.W, "Z")
        ],
        [KeyboardLayoutType.Dvorak] = [
            new LetterButton(Key.A, "A"),
            new LetterButton(Key.X, "B"),
            new LetterButton(Key.J, "C"),
            new LetterButton(Key.E, "D"),
            new LetterButton(Key.OemPeriod, "E"),
            new LetterButton(Key.U, "F"),
            new LetterButton(Key.I, "G"),
            new LetterButton(Key.D, "H"),
            new LetterButton(Key.C, "I"),
            new LetterButton(Key.H, "J"),
            new LetterButton(Key.T, "K"),
            new LetterButton(Key.N, "L"),
            new LetterButton(Key.M, "M"),
            new LetterButton(Key.B, "N"),
            new LetterButton(Key.R, "O"),
            new LetterButton(Key.L, "P"),
            new LetterButton(Key.OemQuotes, "Q"),
            new LetterButton(Key.P, "R"),
            new LetterButton(Key.O, "S"),
            new LetterButton(Key.Y, "T"),
            new LetterButton(Key.G, "U"),
            new LetterButton(Key.K, "V"),
            new LetterButton(Key.OemComma, "W"),
            new LetterButton(Key.Q, "X"),
            new LetterButton(Key.F, "Y"),
            new LetterButton(Key.OemSemicolon, "Z")
        ],
        [KeyboardLayoutType.Colemak] = [
            new LetterButton(Key.A, "A"),
            new LetterButton(Key.B, "B"),
            new LetterButton(Key.I, "C"),
            new LetterButton(Key.D, "D"),
            new LetterButton(Key.R, "E"),
            new LetterButton(Key.F, "F"),
            new LetterButton(Key.P, "G"),
            new LetterButton(Key.H, "H"),
            new LetterButton(Key.J, "J"),
            new LetterButton(Key.K, "K"),
            new LetterButton(Key.L, "L"),
            new LetterButton(Key.Y, "M"),
            new LetterButton(Key.N, "N"),
            new LetterButton(Key.O, "O"),
            new LetterButton(Key.S, "P"),
            new LetterButton(Key.Q, "Q"),
            new LetterButton(Key.U, "R"),
            new LetterButton(Key.T, "S"),
            new LetterButton(Key.V, "T"),
            new LetterButton(Key.E, "U"),
            new LetterButton(Key.C, "V"),
            new LetterButton(Key.W, "W"),
            new LetterButton(Key.X, "X"),
            new LetterButton(Key.M, "Y"),
            new LetterButton(Key.Z, "Z")
        ],
        [KeyboardLayoutType.Workman] = [
            new LetterButton(Key.A, "A"),
            new LetterButton(Key.B, "B"),
            new LetterButton(Key.I, "C"),
            new LetterButton(Key.H, "D"),
            new LetterButton(Key.D, "E"),
            new LetterButton(Key.U, "F"),
            new LetterButton(Key.N, "G"),
            new LetterButton(Key.T, "H"),
            new LetterButton(Key.R, "I"),
            new LetterButton(Key.S, "J"),
            new LetterButton(Key.O, "K"),
            new LetterButton(Key.Y, "L"),
            new LetterButton(Key.M, "M"),
            new LetterButton(Key.E, "N"),
            new LetterButton(Key.P, "O"),
            new LetterButton(Key.Q, "P"),
            new LetterButton(Key.X, "Q"),
            new LetterButton(Key.W, "R"),
            new LetterButton(Key.K, "S"),
            new LetterButton(Key.M, "T"),
            new LetterButton(Key.G, "U"),
            new LetterButton(Key.V, "V"),
            new LetterButton(Key.F, "W"),
            new LetterButton(Key.J, "X"),
            new LetterButton(Key.L, "Y"),
            new LetterButton(Key.Z, "Z")
        ],
        [KeyboardLayoutType.Norman] = [
            new LetterButton(Key.A, "A"),
            new LetterButton(Key.B, "B"),
            new LetterButton(Key.I, "C"),
            new LetterButton(Key.R, "D"),
            new LetterButton(Key.D, "E"),
            new LetterButton(Key.U, "F"),
            new LetterButton(Key.J, "G"),
            new LetterButton(Key.H, "H"),
            new LetterButton(Key.E, "I"),
            new LetterButton(Key.S, "J"),
            new LetterButton(Key.K, "K"),
            new LetterButton(Key.N, "L"),
            new LetterButton(Key.M, "M"),
            new LetterButton(Key.T, "N"),
            new LetterButton(Key.O, "O"),
            new LetterButton(Key.P, "P"),
            new LetterButton(Key.Q, "Q"),
            new LetterButton(Key.W, "R"),
            new LetterButton(Key.L, "S"),
            new LetterButton(Key.Y, "T"),
            new LetterButton(Key.G, "U"),
            new LetterButton(Key.V, "V"),
            new LetterButton(Key.F, "W"),
            new LetterButton(Key.X, "X"),
            new LetterButton(Key.C, "Y"),
            new LetterButton(Key.Z, "Z")
        ]
    };
}
