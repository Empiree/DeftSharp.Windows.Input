namespace DeftSharp.Windows.Input.Mouse;

/// <summary>
/// Specifies types of system input mouse events.
/// </summary>
public enum MouseInputEvent : ushort
{
    Move = 512,
    LeftButtonDown = 513,
    LeftButtonUp = 514,
    RightButtonDown = 516,
    RightButtonUp = 517,
    MiddleButtonDown = 519,
    MiddleButtonUp = 520,
    Scroll = 522
}