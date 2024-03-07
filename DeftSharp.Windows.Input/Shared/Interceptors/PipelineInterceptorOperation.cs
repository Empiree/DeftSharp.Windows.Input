using System;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal sealed class PipelineInterceptorOperation
{
    public bool IsSuccess { get; }
    public Action? Callback { get; }

    public PipelineInterceptorOperation(bool isSuccess, Action callback)
    {
        IsSuccess = isSuccess;
        
        if(isSuccess)
            Callback = callback;
    }
}