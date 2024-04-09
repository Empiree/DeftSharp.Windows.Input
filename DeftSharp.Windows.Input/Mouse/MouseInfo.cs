using static DeftSharp.Windows.Input.Native.MouseAPI;

namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Represents the information about the mouse.
/// </summary>
public sealed class MouseInfo : IMouseInfo
{
    /// <summary>
    /// Gets the current mouse speed level.
    /// </summary>
    public int GetSpeed() => GetMouseSpeed();
}