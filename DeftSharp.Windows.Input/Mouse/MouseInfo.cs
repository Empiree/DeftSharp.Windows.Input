using static DeftSharp.Windows.Input.Native.MouseAPI;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseInfo : IMouseInfo
{
    /// <summary>
    /// Gets the current mouse speed level.
    /// </summary>
    public int Speed => GetMouseSpeed();
}