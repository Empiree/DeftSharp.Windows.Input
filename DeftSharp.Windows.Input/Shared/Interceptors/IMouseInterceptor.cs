using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

internal interface IMouseInterceptor : IInterceptor
{
    event MouseInputDelegate? MouseInput;

    Coordinates GetPosition();
    void SetPosition(int x, int y);
    void Click(MouseButton button, int x, int y);
    void Click(MouseButton button);
}