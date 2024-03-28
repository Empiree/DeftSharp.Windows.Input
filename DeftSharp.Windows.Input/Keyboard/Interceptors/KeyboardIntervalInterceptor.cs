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

    private readonly ConcurrentDictionary<Key, KeyClickInterval> _keyClickIntervals;

    private KeyboardIntervalInterceptor()
        : base(InterceptorType.Prohibitive) =>
        _keyClickIntervals = new ConcurrentDictionary<Key, KeyClickInterval>();

    public void SetInterval(Key key, TimeSpan interval)
    {
        if (interval.Equals(TimeSpan.Zero))
        {
            _keyClickIntervals.TryRemove(key, out _);

            TryUnhook();
            return;
        }

        var keyClickInterval = new KeyClickInterval(key, interval);

        _keyClickIntervals.AddOrUpdate(key, keyClickInterval, (_, _) => keyClickInterval);

        Hook();
    }

    public override void Dispose()
    {
        _keyClickIntervals.Clear();

        Unhook();
        base.Dispose();
    }

    internal override bool OnPipelineUnhookRequested() => !_keyClickIntervals.Any();

    protected override void OnInputSuccess(KeyPressedArgs args)
    {
        if (args.Event is KeyboardInputEvent.KeyUp)
            return;

        _keyClickIntervals.TryGetValue(args.KeyPressed, out var clickInterval);

        if (clickInterval is not null)
            clickInterval.LastClicked = DateTime.Now;
    }

    protected override bool IsInputAllowed(KeyPressedArgs args)
    {
        if (args.Event is KeyboardInputEvent.KeyUp)
            return true;

        _keyClickIntervals.TryGetValue(args.KeyPressed, out var keyClickInterval);

        return !(keyClickInterval is not null && keyClickInterval.IsBlocked);
    }

    private void TryUnhook()
    {
        if (!_keyClickIntervals.Any())
            Unhook();
    }
}