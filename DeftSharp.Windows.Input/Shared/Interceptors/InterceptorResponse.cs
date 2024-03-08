using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal sealed class InterceptorResponse
{
    public bool IsSuccess { get; }
    public Action? OnPipelineSuccess { get; }
    public Action? OnPipelineFailed { get; }

    public InterceptorResponse(bool isSuccess, Action? onPipelineSuccess = null, Action? onPipelineFailed = null)
    {
        IsSuccess = isSuccess;

        OnPipelineSuccess = onPipelineSuccess;
        OnPipelineFailed = onPipelineFailed;
    }
}