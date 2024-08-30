using System;
using DeftSharp.Windows.Input.Interceptors;

namespace DeftSharp.Windows.Input.Shared.Exceptions;

internal class InterceptorPipelineException(InterceptorType type)
    : Exception($"An interceptor with type {type} cannot affect input events.");