namespace DeftSharp.Windows.Input.Shared.Interceptors;

/// <summary>
/// Enum representing types of middleware interceptors.
/// </summary>
public enum InterceptorType : byte
{
    Custom = 0,
    Listener = 1,
    Manipulator = 2,
    Binder = 3
}