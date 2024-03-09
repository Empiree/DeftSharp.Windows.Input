using System;
using System.Collections.Generic;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

/// <summary>
/// Represents the response of an interceptor in the middleware.
/// </summary>
internal sealed class InterceptorResponse
{
    /// <summary>
    /// Indicates whether the interceptor allows the pipeline to continue.
    /// </summary>
    public bool IsAllowed { get; }

    /// <summary>
    /// The middleware interceptor associated with this response.
    /// </summary>
    public MiddlewareInterceptor Interceptor { get; }

    /// <summary>
    /// Action to be invoked upon successful execution of the pipeline.
    /// </summary>
    public Action? OnPipelineSuccess { get; }

    /// <summary>
    /// Action to be invoked upon failure of the pipeline, providing failed interceptors.
    /// </summary>
    public Action<IEnumerable<MiddlewareInterceptor>>? OnPipelineFailed { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="InterceptorResponse"/> class.
    /// </summary>
    /// <param name="isAllowed">Indicates whether the interceptor allows the pipeline to continue.</param>
    /// <param name="interceptor">The middleware interceptor associated with this response.</param>
    /// <param name="onPipelineSuccess">Action to be invoked upon successful execution of the pipeline.</param>
    /// <param name="onPipelineFailed">Action to be invoked upon failure of the pipeline, providing failed interceptors.</param>
    public InterceptorResponse(
        bool isAllowed,
        MiddlewareInterceptor interceptor,
        Action? onPipelineSuccess = null,
        Action<IEnumerable<MiddlewareInterceptor>>? onPipelineFailed = null)
    {
        IsAllowed = isAllowed;
        Interceptor = interceptor;
        OnPipelineSuccess = onPipelineSuccess;
        OnPipelineFailed = onPipelineFailed;
    }
}