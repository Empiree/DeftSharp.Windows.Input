namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Enum representing types of middleware interceptors.
/// </summary>
public enum InterceptorType : byte
{
    Custom = 0,
    Observable = 1,
    Prohibitive = 2,
}