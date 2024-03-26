using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardManipulatorInterceptor : KeyboardInterceptor
{
    private static readonly Lazy<KeyboardManipulatorInterceptor> LazyInstance =
        new(() => new KeyboardManipulatorInterceptor());

    public static KeyboardManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly ConcurrentDictionary<Key, Func<bool>> _lockedKeys;
    public event Action<KeyPressedArgs>? KeyPrevented;

    public IReadOnlyDictionary<Key, Func<bool>> LockedKeys => _lockedKeys;

    private KeyboardManipulatorInterceptor()
        : base(InterceptorType.Manipulator) =>
        _lockedKeys = new ConcurrentDictionary<Key, Func<bool>>();

    public void Press(Key key) => Keyboard.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => Keyboard.PressCombination(combination);

    public void Prevent(Key key, Func<bool> predicate)
    {
        if (_lockedKeys.ContainsKey(key))
            return;

        Hook();
        _lockedKeys.TryAdd(key, predicate);
    }

    public void Release(Key key)
    {
        if (!_lockedKeys.ContainsKey(key))
            return;

        _lockedKeys.TryRemove(key, out _);

        if (!_lockedKeys.Any())
            Unhook();
    }

    public void ReleaseAll()
    {
        if (!_lockedKeys.Any())
            return;

        _lockedKeys.Clear();
        Unhook();
    }

    public bool IsKeyLocked(Key key)
    {
        _lockedKeys.TryGetValue(key, out var predicate);

        return predicate is not null && predicate.Invoke();
    } 

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_lockedKeys.Any();
    protected override bool IsInputAllowed(KeyPressedArgs args) => !IsKeyLocked(args.KeyPressed);

    protected override void OnInputFailure(KeyPressedArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            KeyPrevented?.Invoke(args);
    }
}