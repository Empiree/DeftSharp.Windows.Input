using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

public interface IMouseInterceptor : IRequestedInterceptor
{
    event MouseInputDelegate? MouseInput;

    Coordinates GetPosition();
    void SetPosition(int x, int y);
    void Click(MouseButton button, int x, int y);
    void Click(MouseButton button);
}