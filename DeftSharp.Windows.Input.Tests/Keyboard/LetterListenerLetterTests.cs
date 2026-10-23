using System.Globalization;
using DeftSharp.Windows.Input.Native;

namespace DeftSharp.Windows.Input.Tests.Keyboard;

public sealed class LetterListenerLetterTests
{
    [Fact]
    public void LetterListener_KeyTypesLetter_ReportsIt()
    {
        using var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener, _ => "Ф");
        var letters = new List<string>();

        letterListener.OnKeyPressed(Key.A, letters.Add);

        Assert.Equal(new[] { "Ф" }, letters);
    }

    [Fact]
    public void LetterListener_KeyDoesNotTypeLetter_IsIgnored()
    {
        using var keyboardListener = new KeyboardListener();
        var letterListener = new LetterListener(keyboardListener, _ => null);
        var letters = new List<string>();

        letterListener.OnKeyPressed(Key.D1, letters.Add);

        Assert.Empty(letters);
    }

    [Fact]
    public void LetterListener_ListensToKeysWhereLayoutsPutLetters()
    {
        using var keyboardListener = new KeyboardListener();
        var keys = new LetterListener(keyboardListener).Keys.ToArray();

        Assert.Contains(Key.A, keys);
        Assert.Contains(Key.Z, keys);
        Assert.Contains(Key.D2, keys);           // é in the French layout
        Assert.Contains(Key.OemSemicolon, keys); // ж in the Russian layout
        Assert.DoesNotContain(Key.NumPad2, keys);
        Assert.Equal(keys.Length, keys.Distinct().Count());
    }

    [Theory]
    [InlineData('a', "en-US", "A")]
    [InlineData('ф', "ru-RU", "Ф")]
    [InlineData('i', "tr-TR", "İ")]
    [InlineData('ı', "tr-TR", "I")]
    [InlineData('1', "en-US", null)]
    [InlineData(';', "en-US", null)]
    public void KeyboardAPI_ToLetter(char character, string culture, string? expected)
    {
        Assert.Equal(expected, KeyboardAPI.ToLetter(character, CultureInfo.GetCultureInfo(culture)));
    }
}
