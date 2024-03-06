using static DeftSharp.Windows.Input.InteropServices.API.WinAPI;

namespace DeftSharp.Windows.Input.InteropServices.API;

internal static class MouseAPI
{
    internal static void SetPosition(int x, int y)
    {
        SetCursorPos(x, y);
    }

    internal static void Click(int x, int y)
    {
        SetPosition(x, y);
        mouse_event(0x02, x, y, 0, 0);
        mouse_event(0x04, x, y, 0, 0);
    }
}