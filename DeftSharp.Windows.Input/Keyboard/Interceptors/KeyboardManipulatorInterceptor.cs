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
    public event Action<KeyboardInputArgs>? KeyPrevented;

    public IEnumerable<Key> LockedKeys => _lockedKeys.Keys;

    private KeyboardManipulatorInterceptor()
        : base(InterceptorType.Prohibitive) =>
        _lockedKeys = new ConcurrentDictionary<Key, Func<bool>>();

    public void Press(Key key) => Keyboard.Press(key);
    public void Press(IEnumerable<Key> combination) => Keyboard.Press(combination);

    public void Simulate(Key key, KeyboardSimulateOption option) => Keyboard.Simulate(key, option);

    public void Prevent(Key key, Func<bool> predicate)
    {
        _lockedKeys.AddOrUpdate(key, predicate,
            (_, _) => predicate);
        Hook();
    }

    public void Release(Key key)
    {
        _lockedKeys.TryRemove(key, out _);
        TryUnhook();
    }

    public void Release()
    {
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
        Release();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_lockedKeys.Any();
    protected override bool IsInputAllowed(KeyboardInputArgs args) => !IsKeyLocked(args.KeyPressed);

    protected override void OnInputFailure(KeyboardInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            KeyPrevented?.Invoke(args);
    }

    private void TryUnhook()
    {
        if (!_lockedKeys.Any())
            Unhook();
    }
}