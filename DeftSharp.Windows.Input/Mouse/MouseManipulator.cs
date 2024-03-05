using System;
using DeftSharp.Windows.Input.InteropServices.API;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors;

namespace DeftSharp.Windows.Input.Mouse;

public sealed class MouseManipulator
{
    private readonly IMouseInterceptor _mouseInterceptor;

    public MouseManipulator()
    {
        _mouseInterceptor = WindowsMouseInterceptor.Instance;
    }

    // TODO implement 
    public void Prevent(MouseEvent mouseEvent)
    {
        throw new NotImplementedException();
    }

    public void SetPosition(int x, int y) => MouseAPI.SetPosition(x, y);
    public void Click(int x, int y) => MouseAPI.Click(x, y);
}