namespace DeftSharp.Windows.Input.Shared.Interceptors;

/// <summary>
/// Enum representing types of middleware interceptors.
/// </summary>
internal enum InterceptorType : byte
{
    Listener = 0,
    Manipulator = 1,
    Binder = 2
}