namespace DeftSharp.Windows.Input.Mouse;

public readonly struct Coordinates
{
    public readonly int X;
    public readonly int Y;

    public Coordinates(int x, int y)
    {
        X = x;
        Y = y;
    }
}