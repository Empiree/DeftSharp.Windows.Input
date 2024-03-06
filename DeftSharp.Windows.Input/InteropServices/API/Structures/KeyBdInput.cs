using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.InteropServices.API.Structures;

/// <summary>
/// Contains information about a simulated keyboard event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct KeyBdInput
{
    /// <summary>
    /// The virtual-key code of the key.
    /// </summary>
    public ushort wVk;

    /// <summary>
    /// The hardware scan code of the key.
    /// </summary>
    public ushort wScan;

    /// <summary>
    /// Flags that specify various aspects of function operation.
    /// </summary>
    public uint dwFlags;

    /// <summary>
    /// The time stamp for the event, in milliseconds.
    /// </summary>
    public uint time;

    /// <summary>
    /// An additional value associated with the keystroke.
    /// </summary>
    public nint dwExtraInfo;
}