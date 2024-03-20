using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardManipulatorInterceptor : KeyboardInterceptor, IKeyboardManipulator
{
    private static readonly Lazy<KeyboardManipulatorInterceptor> LazyInstance =
        new(() => new KeyboardManipulatorInterceptor());

    public static KeyboardManipulatorInterceptor Instance => LazyInstance.Value;

    private readonly HashSet<Key> _lockedKeys;
    private readonly object _lock = new();
    public event Action<KeyPressedArgs>? KeyPrevented;

    public IEnumerable<Key> LockedKeys => _lockedKeys;

    private KeyboardManipulatorInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _lockedKeys = new HashSet<Key>();
    }

    ~KeyboardManipulatorInterceptor() => Dispose();

    public void Press(Key key) => Keyboard.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => Keyboard.PressCombination(combination);

    public void Prevent(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.Any(k => k == key))
                return;

            Hook();
            _lockedKeys.Add(key);
        }
    }

    public void Release(Key key)
    {
        lock (_lock)
        {
            if (_lockedKeys.All(k => k != key))
                return;

            _lockedKeys.Remove(key);

            if (!_lockedKeys.Any())
                Unhook();
        }
    }

    public void ReleaseAll()
    {
        if (!_lockedKeys.Any()) 
            return;
        
        _lockedKeys.Clear();
        Unhook();
    }

    public bool IsKeyLocked(Key key) => _lockedKeys.Any(k => k == key);

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !_lockedKeys.Any();

    protected override InterceptorResponse OnKeyboardInput(KeyPressedArgs args) =>
        new(!IsKeyLocked(args.KeyPressed),
            InterceptorType.Manipulator,
            onPipelineFailed: failedInterceptors =>
            {
                if (failedInterceptors.Contains(InterceptorType.Manipulator))
                    KeyPrevented?.Invoke(args);
            });
}