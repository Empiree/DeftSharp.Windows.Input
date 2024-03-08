using System;
using System.Collections.Generic;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

internal sealed class InterceptorResponse
{
    public bool IsAllowed { get; }
    public PipelineInterceptor Interceptor { get; set; }
    public Action? OnPipelineSuccess { get; }
    public Action<IEnumerable<PipelineInterceptor>>? OnPipelineFailed { get; }

    public InterceptorResponse(
        bool isAllowed, 
        PipelineInterceptor interceptor,
        Action? onPipelineSuccess = null, Action<IEnumerable<PipelineInterceptor>>? onPipelineFailed = null)
    {
        IsAllowed = isAllowed;
        Interceptor = interceptor;
        OnPipelineSuccess = onPipelineSuccess;
        OnPipelineFailed = onPipelineFailed;
    }
}