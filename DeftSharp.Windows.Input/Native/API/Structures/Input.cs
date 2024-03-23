using System.Runtime.InteropServices;

namespace DeftSharp.Windows.Input.Native.API.Structures;

/// <summary>
/// Contains information about an input event.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct Input
{
    /// <summary>
    /// The type of the input event. This can be one of the following values: INPUT_MOUSE, INPUT_KEYBOARD, or INPUT_HARDWARE.
    /// </summary>
    public uint type;

    /// <summary>
    /// A union of the input event. This contains the data for the input event, such as keyboard, mouse, or hardware input.
    /// </summary>
    public InputUnion u;

    /// <summary>
    /// Initializes a new instance of the Input structure with the specified type.
    /// </summary>
    /// <param name="type">The type of the input event.</param>
    public Input(uint type)
    {
        this.type = type;
    }
}