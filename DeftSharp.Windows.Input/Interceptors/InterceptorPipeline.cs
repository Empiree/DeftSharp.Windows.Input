using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Shared.Exceptions;

namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Pipeline responsible for running a list of interceptors and handling the interceptor responses accordingly.
/// </summary>
internal sealed class InterceptorPipeline
{
    /// <summary>
    /// Runs the interceptors and determines if the pipeline can be processed based on interceptor responses.
    /// </summary>
    /// <param name="interceptors">The list of interceptor responses.</param>
    /// <returns>True if the pipeline can be processed; otherwise, false.</returns>
    public bool Run(List<InterceptorResponse> interceptors)
    {
        var isPipelineAllowed = interceptors.All(i => i.IsAllowed);

        if (isPipelineAllowed)
        {
            foreach (var action in interceptors.Select(i => i.OnPipelineSuccess))
                action?.Invoke();
            return true;
        }

        var failedInterceptors = interceptors
            .Where(i => !i.IsAllowed)
            .Select(i => i.Interceptor)
            .ToArray();

        if (failedInterceptors.Any(i => i.Type is InterceptorType.Observable))
            throw new InterceptorPipelineException(InterceptorType.Observable);

        foreach (var action in interceptors.Select(i => i.OnPipelineFailed))
            action?.Invoke(failedInterceptors);

        return false;
    }
}