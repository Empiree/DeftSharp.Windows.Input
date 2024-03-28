using System;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

internal class InterceptorPipelineException : Exception
{
    public InterceptorPipelineException(InterceptorType type) : base(
        $"An interceptor with type {type} cannot affect input events.") { }
}