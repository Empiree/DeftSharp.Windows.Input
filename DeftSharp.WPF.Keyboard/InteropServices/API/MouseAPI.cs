namespace DeftSharp.Windows.Keyboard.InteropServices.API;

public static class MouseAPI
{
    public static CursorPosition GetPosition()
    {
        WinAPI.GetCursorPos(out var position);

        return position;
    }

    public static void SetPosition(int x, int y)
    {
        WinAPI.SetCursorPos(x, y);
    }

    public static void Click(int x, int y)
    {
        SetPosition(x, y);
        WinAPI.mouse_event(0x02, x, y, 0, 0);
        WinAPI.mouse_event(0x04, x, y, 0, 0);
    }
}