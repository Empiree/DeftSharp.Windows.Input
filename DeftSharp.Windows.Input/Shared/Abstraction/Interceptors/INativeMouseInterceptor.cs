using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

internal interface INativeMouseInterceptor : IRequestedInterceptor
{
    event MouseInputDelegate? MouseInput;
    int GetSpeed();
    Point GetPosition();
    void SetPosition(int x, int y);
    void Click(MouseButton button, int x, int y);
    void Click(MouseButton button);
    void Scroll(int scrollAmount);
}