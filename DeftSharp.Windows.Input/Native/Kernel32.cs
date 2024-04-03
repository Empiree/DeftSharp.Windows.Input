using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.Native;

/// <summary>
/// Provides methods for interacting with the Kernel32.dll library.
/// </summary>
internal static class Kernel32
{
    /// <summary>
    /// Retrieves the identifier of the current thread.
    /// </summary>
    /// <returns>
    /// The identifier of the current thread.
    /// </returns>
    [DllImport("kernel32.dll")]
    internal static extern uint GetCurrentThreadId();
    
    /// <summary>
    /// Retrieves a module handle for the specified module.
    /// </summary>
    /// <param name="lpModuleName">The name of the loaded module (either a .dll or .exe file).</param>
    /// <returns>A handle to the specified module, or <c>0</c> if the module could not be found.</returns>
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    internal static extern nint GetModuleHandle(string lpModuleName);
}