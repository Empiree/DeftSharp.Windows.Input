using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardManipulatorInterceptor : KeyboardInterceptor, IKeyboardManipulator
{
    #region Singleton

    private static readonly Lazy<KeyboardManipulatorInterceptor> LazyInstance =
        new(() => new KeyboardManipulatorInterceptor());

    public static KeyboardManipulatorInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly HashSet<Key> _lockedKeys;
    private readonly object _lock = new();
    public event Action<Key>? KeyPrevented;
    public event Action<Key>? KeyReleased;

    public IEnumerable<Key> LockedKeys => _lockedKeys;

    private KeyboardManipulatorInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _lockedKeys = new HashSet<Key>();
    }

    ~KeyboardManipulatorInterceptor()
    {
        Dispose();
    }

    public void Press(Key key) => Keyboard.Press(key);

    public void Prevent(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.Any(k => k == key))
                return;

            Hook();
            _lockedKeys.Add(key);
            KeyPrevented?.Invoke(key);
        }
    }

    public void Release(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.All(k => k != key))
                return;

            _lockedKeys.Remove(key);
            KeyReleased?.Invoke(key);

            if (!_lockedKeys.Any())
                Unhook();
        }
    }

    public void ReleaseAll()
    {
        var keys = _lockedKeys.ToArray();

        foreach (var key in keys)
            Release(key);
    }

    public bool IsKeyLocked(Key key) => _lockedKeys.Any(k => k == key);

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !_lockedKeys.Any();

    protected override InterceptorResponse OnInterceptorPipelineRequested(KeyPressedArgs args) =>
        new(!IsKeyLocked(args.KeyPressed), PipelineInterceptor.Manipulator);
}