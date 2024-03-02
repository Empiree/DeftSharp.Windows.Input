using System;
using System.Windows.Input;

namespace DeftSharp.Windows.Keyboard.Shared.Models;

public sealed class KeyboardButton
{
    private readonly Action<Key> _onClick;

    public Guid Id { get; }
    public Key Key { get; }
    public TimeSpan IntervalOfClick { get; set; }
    public DateTime? LastInvoked { get; private set; }
    public bool SingleUse { get; }
    
    public KeyboardButton(Key key, Action<Key> onClick, bool singleUse = false)
    {
        _onClick = onClick;

        Key = key;
        SingleUse = singleUse;
        IntervalOfClick = TimeSpan.Zero;
        
        Id = Guid.NewGuid();
    }

    public void Invoke()
    {
        if (LastInvoked.HasValue && SingleUse)
            return;

        if (LastInvoked is null || LastInvoked.Value.Add(IntervalOfClick) < DateTime.Now)
        {
            LastInvoked = DateTime.Now;
            _onClick(Key);
        }
    }
}