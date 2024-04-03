using DeftSharp.Windows.Input.Mouse;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;

internal interface INativeMouseInterceptor : IRequestedInterceptor
{
    event MouseInputDelegate? MouseInput;
    int GetSpeed();
    Point GetPosition();
    void SetPosition(int x, int y);
    void Click(int x, int y, MouseButton button);
    void Scroll(int scrollAmount);
}