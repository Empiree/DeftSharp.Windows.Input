using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardManipulatorInterceptor : KeyboardInterceptor
{
    private static readonly Lazy<KeyboardManipulatorInterceptor> LazyInstance =
        new(() => new KeyboardManipulatorInterceptor());

    public static KeyboardManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly ConcurrentDictionary<Key, Func<bool>> _lockedKeys;

    // Keys whose KeyDown was blocked here. Only their KeyUp is blocked as well,
    // so a key that was already down when it got locked does not stay pressed in the system.
    private readonly HashSet<Key> _blockedPresses = new();

    public event Action<KeyboardInputArgs>? KeyPrevented;

    public IEnumerable<Key> LockedKeys => _lockedKeys.Keys;

    private KeyboardManipulatorInterceptor()
        : this(WindowsKeyboardInterceptor.Instance)
    {
    }

    internal KeyboardManipulatorInterceptor(INativeKeyboardInterceptor keyboard)
        : base(InterceptorType.Prohibitive, keyboard) =>
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

    internal override bool OnPipelineUnhookRequested() => _lockedKeys.IsEmpty;

    protected override bool IsInputAllowed(KeyboardInputArgs args)
    {
        var key = args.KeyPressed;

        if (args.Event is KeyboardInputEvent.KeyUp)
            return !_blockedPresses.Remove(key);

        if (!IsKeyLocked(key))
        {
            _blockedPresses.Remove(key);
            return true;
        }

        // If the system already has the key down, this KeyDown is an auto-repeat
        // of a press that went through, so its KeyUp has to go through too.
        if (!Keyboard.IsKeyPressed(key))
            _blockedPresses.Add(key);

        return false;
    }

    protected override void OnInputFailure(KeyboardInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            KeyPrevented?.Invoke(args);
    }

    private void TryUnhook()
    {
        if (_lockedKeys.IsEmpty)
            Unhook();
    }
}