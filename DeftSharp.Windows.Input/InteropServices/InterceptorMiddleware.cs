using System.Collections.Generic;
using System.Linq;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.InteropServices;

/// <summary>
/// Middleware responsible for running a list of interceptors and handling the pipeline accordingly.
/// </summary>
internal sealed class InterceptorMiddleware
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

        foreach (var action in interceptors.Select(i => i.OnPipelineFailed))
            action?.Invoke(failedInterceptors);

        return false;
    }
}