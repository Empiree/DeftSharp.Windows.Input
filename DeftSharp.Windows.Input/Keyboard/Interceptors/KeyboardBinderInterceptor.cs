using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Abstraction.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardBinderInterceptor : KeyboardInterceptor, IKeyboardBinder
{
    #region Singleton

    private static readonly Lazy<KeyboardBinderInterceptor> LazyInstance =
        new(() => new KeyboardBinderInterceptor());

    public static KeyboardBinderInterceptor Instance => LazyInstance.Value;

    #endregion

    private readonly ConcurrentDictionary<Key, Key> _boundedKeys;

    public IReadOnlyDictionary<Key, Key> BoundedKeys => _boundedKeys;

    private KeyboardBinderInterceptor()
        : base(WindowsKeyboardInterceptor.Instance)
    {
        _boundedKeys = new ConcurrentDictionary<Key, Key>();
    }

    ~KeyboardBinderInterceptor()
    {
        Dispose();
    }

    public void Bind(Key oldKey, Key newKey)
    {
        if (_boundedKeys.ContainsKey(oldKey))
            _boundedKeys[oldKey] = newKey;

        Hook();
        var isAdded = _boundedKeys.TryAdd(oldKey, newKey);
    }

    public void Unbind(Key key)
    {
        if (!_boundedKeys.ContainsKey(key))
            return;

        var boundedKey = _boundedKeys.FirstOrDefault();

        _boundedKeys.TryRemove(boundedKey);

        if (!_boundedKeys.Any())
            Unhook();
    }

    public void UnbindAll()
    {
        var boundedKeys = _boundedKeys.Keys.ToArray();
        foreach (var boundedKey in boundedKeys)
            Unbind(boundedKey);
    }

    public bool IsKeyBounded(Key key) => _boundedKeys.ContainsKey(key);

    public override void Dispose()
    {
        UnbindAll();
        base.Dispose();
    }

    protected override bool OnInterceptorUnhookRequested() => !_boundedKeys.Any();

    protected override InterceptorResponse OnInterceptorPipelineRequested(KeyPressedArgs args)
    {
        return new InterceptorResponse(
            !IsKeyBounded(args.KeyPressed),
            null, () =>
            {
                if (args.Event == KeyboardEvent.KeyUp)
                    return;

                var key = _boundedKeys[args.KeyPressed];
                KeyboardAPI.PressButton(key);
            });
    }
}