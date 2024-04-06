using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Shared.Exceptions;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Pipeline responsible for running a list of interceptors and handling the interceptor responses accordingly.
/// </summary>
internal sealed class InterceptorPipeline
{
    private readonly IEnumerable<InterceptorResponse> _interceptors;

    public InterceptorPipeline(IEnumerable<InterceptorResponse> interceptors) => _interceptors = interceptors;

    public bool IsAllowed => _interceptors.All(i => i.IsAllowed);

    /// <summary>
    /// Runs the interceptors results.
    /// </summary>
    public void Run()
    {
        if (IsAllowed)
        {
            foreach (var action in _interceptors.Select(i => i.OnPipelineSuccess))
                action?.Invoke();
            
            return;
        }
        
        var failedInterceptors = _interceptors
            .Where(i => !i.IsAllowed)
            .Select(i => i.Interceptor)
            .ToArray();

        if (failedInterceptors.Any(i => i.Type is InterceptorType.Observable))
            throw new InterceptorPipelineException(InterceptorType.Observable);
        
        foreach (var action in _interceptors.Select(i => i.OnPipelineFailed))
            action?.Invoke(failedInterceptors);
    }
}