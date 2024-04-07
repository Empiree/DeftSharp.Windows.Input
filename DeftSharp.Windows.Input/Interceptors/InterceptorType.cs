namespace DeftSharp.Windows.Input.Interceptors;

/// <summary>
/// Enum representing types of middleware interceptors.
/// </summary>
public enum InterceptorType : byte
{
    /// <summary>
    /// User defined type of interceptors.
    /// </summary>
    Custom = 0,
    /// <summary>
    /// Type of interceptors that do not affect the input result.
    /// </summary>
    Observable = 1,
    /// <summary>
    /// An affecting type of interceptor that prohibits any action.
    /// </summary>
    Prohibitive = 2,
}