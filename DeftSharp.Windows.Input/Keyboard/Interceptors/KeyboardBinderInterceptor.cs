using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Native.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardBinderInterceptor : KeyboardInterceptor
{
    private static readonly Lazy<KeyboardBinderInterceptor> LazyInstance =
        new(() => new KeyboardBinderInterceptor());

    public static KeyboardBinderInterceptor Instance => LazyInstance.Value;

    private readonly ConcurrentDictionary<Key, Key> _boundedKeys;

    // Bound keys that are held right now, with the key that was pressed instead of each of them.
    private readonly ConcurrentDictionary<Key, Key> _remappedPresses = new();

    // Input to send for the events blocked here, in the same order as the events.
    private readonly ConcurrentQueue<(Key Key, KeyboardSimulateOption Option)> _pendingInputs = new();

    public IReadOnlyDictionary<Key, Key> BoundedKeys => _boundedKeys;

    private KeyboardBinderInterceptor()
        : this(WindowsKeyboardInterceptor.Instance)
    {
    }

    internal KeyboardBinderInterceptor(INativeKeyboardInterceptor keyboard)
        : base(InterceptorType.Prohibitive, keyboard) =>
        _boundedKeys = new ConcurrentDictionary<Key, Key>();

    public void Bind(Key oldKey, Key newKey)
    {
        if (oldKey == newKey)
            return;

        if (_boundedKeys.ContainsKey(oldKey))
            _boundedKeys[oldKey] = newKey;

        Hook();
        _boundedKeys.TryAdd(oldKey, newKey);
    }

    public void Unbind(Key key)
    {
        if (!_boundedKeys.TryRemove(key, out _))
            return;

        if (!_boundedKeys.IsEmpty)
            return;

        Unhook();
        ReleaseRemappedPresses();
    }

    public void Unbind()
    {
        var boundedKeys = _boundedKeys.Keys.ToArray();
        foreach (var boundedKey in boundedKeys)
            Unbind(boundedKey);
    }

    public Key GetBoundKey(Key key) => _boundedKeys.GetValueOrDefault(key, key);

    public bool IsKeyBounded(Key key) => _boundedKeys.ContainsKey(key);

    public override void Dispose()
    {
        Unbind();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_boundedKeys.Any();

    protected override bool IsInputAllowed(KeyboardInputArgs args)
    {
        if (args.IsBinderInput)
            return true;

        var key = args.KeyPressed;

        if (args.Event is KeyboardInputEvent.KeyUp)
        {
            // Release the key that was pressed instead, even if the binding has changed since then
            if (!_remappedPresses.TryRemove(key, out var releasedKey))
                return true;

            _pendingInputs.Enqueue((releasedKey, KeyboardSimulateOption.KeyUp));
            return false;
        }

        if (!_boundedKeys.TryGetValue(key, out var boundKey))
            return true;

        if (!_remappedPresses.TryGetValue(key, out var pressedKey))
        {
            // The key was already down before it got bound, so this press goes through as is
            if (Keyboard.IsKeyPressed(key))
                return true;

            pressedKey = boundKey;
            _remappedPresses[key] = pressedKey;
        }

        _pendingInputs.Enqueue((pressedKey, KeyboardSimulateOption.KeyDown));
        return false;
    }

    protected override void OnInputFailure(KeyboardInputArgs args, IEnumerable<InterceptorInfo> failedInterceptors)
    {
        if (!failedInterceptors.Any(i => i.Name.Equals(Name)))
            return;

        if (_pendingInputs.TryDequeue(out var input))
            Keyboard.SimulateBinderInput(input.Key, input.Option);
    }

    private void ReleaseRemappedPresses()
    {
        foreach (var key in _remappedPresses.Keys)
        {
            if (_remappedPresses.TryRemove(key, out var pressedKey))
                _pendingInputs.Enqueue((pressedKey, KeyboardSimulateOption.KeyUp));
        }

        while (_pendingInputs.TryDequeue(out var input))
            Keyboard.SimulateBinderInput(input.Key, input.Option);
    }
}