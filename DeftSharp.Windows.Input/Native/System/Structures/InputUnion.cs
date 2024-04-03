using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.Native.System;

/// <summary>
/// Represents a union of input event types.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
internal struct InputUnion
{
    /// <summary>
    /// Represents mouse input.
    /// </summary>
    [FieldOffset(0)]
    public MouseInput mi;

    /// <summary>
    /// Represents keyboard input.
    /// </summary>
    [FieldOffset(0)]
    public KeyBdInput ki;

    /// <summary>
    /// Represents hardware input.
    /// </summary>
    [FieldOffset(0)]
    public HardwareInput hi;
}