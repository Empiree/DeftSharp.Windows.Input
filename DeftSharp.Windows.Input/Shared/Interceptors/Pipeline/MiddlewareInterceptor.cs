namespace DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

/// <summary>
/// Enum representing types of middleware interceptors.
/// </summary>
public enum MiddlewareInterceptor : byte
{
    Listener = 0,
    Manipulator = 1,
    Binder = 2
}