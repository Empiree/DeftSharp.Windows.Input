using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

internal sealed class InterceptorResponse
{
    public bool IsAllowed { get; }
    public Action? OnPipelineSuccess { get; }
    public Action? OnPipelineFailed { get; }

    public InterceptorResponse(bool isAllowed, Action? onPipelineSuccess = null, Action? onPipelineFailed = null)
    {
        IsAllowed = isAllowed;
        OnPipelineSuccess = onPipelineSuccess;
        OnPipelineFailed = onPipelineFailed;
    }
}