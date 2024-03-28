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
    private readonly ConcurrentDictionary<Key, KeyClickInterval> _keyClickIntervals;
    public event Action<KeyPressedArgs>? KeyPrevented;

    public IEnumerable<Key> LockedKeys => _lockedKeys.Keys;

    private KeyboardManipulatorInterceptor()
        : base(InterceptorType.Manipulator)
    {
        _lockedKeys = new ConcurrentDictionary<Key, Func<bool>>();
        _keyClickIntervals = new ConcurrentDictionary<Key, KeyClickInterval>();
    }

    public void Press(Key key) => Keyboard.Press(key);
    public void PressCombination(IEnumerable<Key> combination) => Keyboard.PressCombination(combination);

    public void Prevent(Key key, Func<bool> predicate)
    {
        if (_lockedKeys.ContainsKey(key))
            return;

        Hook();
        _lockedKeys.TryAdd(key, predicate);
    }

    public void SetInterval(Key key, TimeSpan interval)
    {
        if (interval.Equals(TimeSpan.Zero))
        {
            _keyClickIntervals.TryRemove(key, out _);

            if (!_keyClickIntervals.Any())
                Unhook();
            
            return;
        }
        
        var keyClickInterval = new KeyClickInterval(key, interval);

        _keyClickIntervals.AddOrUpdate(key, keyClickInterval, (_, _) => keyClickInterval);
        
        Hook();
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

        if (predicate is not null && predicate.Invoke())
            return true;

        _keyClickIntervals.TryGetValue(key, out var keyClickInterval);

        return keyClickInterval is not null && keyClickInterval.IsBlocked;
    } 

    public override void Dispose()
    {
        ReleaseAll();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_lockedKeys.Any();
    protected override bool IsInputAllowed(KeyPressedArgs args) => !IsKeyLocked(args.KeyPressed);

    protected override void OnInputSuccess(KeyPressedArgs args)
    {
        _keyClickIntervals.TryGetValue(args.KeyPressed, out var clickInterval);

         if (clickInterval is not null)
             clickInterval.LastClicked = DateTime.Now;
    }

    protected override void OnInputFailure(KeyPressedArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (failedInterceptors.Any(i => i.Name.Equals(Name)))
            KeyPrevented?.Invoke(args);
    }
}