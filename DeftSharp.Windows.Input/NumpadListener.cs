using System;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Shared.Models;

namespace DeftSharp.Windows.Input;

public sealed class NumpadListener
{
    private readonly KeyboardListener _keyboardListener;

    private readonly NumpadButton[] _numKeys =
    {
        new(Key.NumPad7, 7), new(Key.NumPad8, 8), new(Key.NumPad9, 9),
        new(Key.NumPad4, 4), new(Key.NumPad5, 5), new(Key.NumPad6, 6),
        new(Key.NumPad1, 1), new(Key.NumPad2, 2), new(Key.NumPad3, 3),
        new(Key.NumPad0, 0)
    };

    public NumpadListener(KeyboardListener keyboardListener)
    {
        _keyboardListener = keyboardListener;
    }

    public void Subscribe(Action<int> onNumClick)
    {
        var keys = _numKeys.Select(n => n.Key);

        _keyboardListener.Subscribe(keys, key =>
        {
            var numKey = _numKeys.FirstOrDefault(n => n.Key == key);

            if (numKey is null)
                throw new NotImplementedException($"{key} was not implemented!");

            onNumClick(numKey.Number);
        });
    }

    public void Unsubscribe() => _keyboardListener.UnsubscribeAll(_numKeys.Select(n => n.Key));
}