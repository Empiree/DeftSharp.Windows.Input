using System.Collections.Generic;
using System.Windows.Input;

namespace DeftSharp.Windows.Input.Shared.Buttons;

public sealed class ButtonSequence
{
    public IEnumerable<Key> Sequence { get; }
    
    public ButtonSequence(IEnumerable<Key> sequence)
    {
        Sequence = sequence;
    }
}