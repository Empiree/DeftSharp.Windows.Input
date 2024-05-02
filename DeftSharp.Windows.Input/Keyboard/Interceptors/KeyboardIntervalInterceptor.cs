using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Input;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Keyboard.Interceptors;

internal sealed class KeyboardIntervalInterceptor : KeyboardInterceptor
{
    private static readonly Lazy<KeyboardIntervalInterceptor> LazyInstance =
        new(() => new KeyboardIntervalInterceptor());

    public static KeyboardIntervalInterceptor Instance => LazyInstance.Value;

    private readonly ConcurrentDictionary<Key, KeyPressInterval> _keyPressIntervals;

    private KeyboardIntervalInterceptor()
        : base(InterceptorType.Prohibitive) =>
        _keyPressIntervals = new ConcurrentDictionary<Key, KeyPressInterval>();

    public void SetInterval(Key key, TimeSpan interval)
    {
        if (interval.Equals(TimeSpan.Zero))
        {
            _keyPressIntervals.TryRemove(key, out _);

            TryUnhook();
            return;
        }

        var keyPressInterval = new KeyPressInterval(key, interval);

        _keyPressIntervals.AddOrUpdate(key, keyPressInterval, (_, _) => keyPressInterval);

        Hook();
    }

    public void ResetInterval()
    {
        _keyPressIntervals.Clear();
        Unhook();
    }

    public override void Dispose()
    {
        ResetInterval();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_keyPressIntervals.Any();

    protected override void OnInputSuccess(KeyboardInputArgs args)
    {
        if (args.Event is KeyboardInputEvent.KeyUp)
            return;

        _keyPressIntervals.TryGetValue(args.KeyPressed, out var pressInterval);

        if (pressInterval is not null)
            pressInterval.LastPressed = DateTime.Now;
    }

    protected override bool IsInputAllowed(KeyboardInputArgs args)
    {
        if (args.Event is KeyboardInputEvent.KeyUp)
            return true;

        _keyPressIntervals.TryGetValue(args.KeyPressed, out var pressInterval);

        return !(pressInterval is not null && pressInterval.IsBlocked);
    }

    private void TryUnhook()
    {
        if (!_keyPressIntervals.Any())
            Unhook();
    }
}