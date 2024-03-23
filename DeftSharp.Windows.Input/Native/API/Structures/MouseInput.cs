using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.Native.API.Structures;

/// <summary>
/// Contains information about a simulated mouse event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct MouseInput
{
    /// <summary>
    /// The absolute position of the mouse, along the x-axis or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member.
    /// </summary>
    public int dx;

    /// <summary>
    /// The absolute position of the mouse, along the y-axis or the amount of motion since the last mouse event was generated, depending on the value of the dwFlags member.
    /// </summary>
    public int dy;

    /// <summary>
    /// If dwFlags contains MOUSEEVENTF_WHEEL, then mouseData specifies the amount of wheel movement. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user. One wheel click is defined as WHEEL_DELTA, which is 120.
    /// </summary>
    public uint mouseData;

    /// <summary>
    /// A set of bit flags that specify various aspects of mouse motion and button clicks.
    /// </summary>
    public uint dwFlags;

    /// <summary>
    /// The time stamp for the event, in milliseconds.
    /// </summary>
    public uint time;

    /// <summary>
    /// An additional value associated with the mouse event.
    /// </summary>
    public nint dwExtraInfo;
}