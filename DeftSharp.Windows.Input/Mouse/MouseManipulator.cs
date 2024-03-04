using DeftSharp.Windows.Input.InteropServices.API;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseManipulator
{
    public void SetPosition(int x, int y) => MouseAPI.SetPosition(x, y);
    public void Click(int x, int y) => MouseAPI.Click(x, y);
}