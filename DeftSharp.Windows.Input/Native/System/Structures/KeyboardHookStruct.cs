using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.Native.System;

/// <summary>
/// Contains information about a low-level keyboard input event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct KeyboardHookStruct
{
    /// <summary>
    /// The virtual-key code of the key.
    /// </summary>
    public uint vkCode;

    /// <summary>
    /// The hardware scan code of the key.
    /// </summary>
    public uint scanCode;

    /// <summary>
    /// The extended-key flag, event-injected flags, context code, and transition-state flag.
    /// </summary>
    public uint flags;

    /// <summary>
    /// The time stamp for this message.
    /// </summary>
    public uint time;

    /// <summary>
    /// Additional information associated with the message.
    /// </summary>
    public nint dwExtraInfo;
}
