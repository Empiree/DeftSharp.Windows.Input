namespace DeftSharp.Windows.Input.InteropServices.API;

public readonly struct CursorPosition
{
    public readonly int X;
    public readonly int Y;

    public CursorPosition(int x, int y)
    {
        X = x;
        Y = y;
    }
}