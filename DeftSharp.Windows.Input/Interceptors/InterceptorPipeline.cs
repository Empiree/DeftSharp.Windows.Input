using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DeftSharp.Windows.Input.Shared.Exceptions;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Pipeline responsible for running a list of interceptors and handling the interceptor responses accordingly.
/// </summary>
internal sealed class InterceptorPipeline
{
    private readonly IEnumerable<InterceptorResponse> _interceptors;

    /// <summary>
    /// Gets a value indicating whether the pipeline has passed through all interceptors.
    /// </summary>
    public bool IsPassed { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the system is allowed to proceed input event.
    /// </summary>
    public bool IsAllowed => _interceptors.All(i => i.IsAllowed);

    public InterceptorPipeline(IEnumerable<InterceptorResponse> interceptors)
        => _interceptors = interceptors;

    /// <summary>
    /// Runs the interceptors callbacks.
    /// </summary>               
    public void Run()
    {
        var dispatcher = Application.Current is null
            ? Dispatcher.CurrentDispatcher
            : Application.Current.Dispatcher;

        dispatcher.BeginInvoke(RunCallbacks);
    }

    private void RunCallbacks()
    {
        if (IsAllowed)
            RunSuccessful();
        else
            RunFailed();
    }

    private void RunFailed()
    {
        if (IsPassed || IsAllowed)
            return;

        var failedInterceptors = _interceptors
            .Where(i => !i.IsAllowed)
            .Select(i => i.Interceptor)
            .ToArray();

        if (failedInterceptors.Any(i => i.Type is InterceptorType.Observable))
            throw new InterceptorPipelineException(InterceptorType.Observable);

        foreach (var callback in _interceptors.Select(i => i.OnPipelineFailed))
            callback?.Invoke(failedInterceptors);

        IsPassed = true;
    }

    private void RunSuccessful()
    {
        if (IsPassed || !IsAllowed)
            return;

        foreach (var callback in _interceptors.Select(i => i.OnPipelineSuccess))
            callback?.Invoke();

        IsPassed = true;
    }
}