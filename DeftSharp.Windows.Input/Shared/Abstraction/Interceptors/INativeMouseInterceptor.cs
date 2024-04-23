using DeftSharp.Windows.Input.Shared.Abstraction.Interceptors;
using DeftSharp.Windows.Input.Shared.Delegates;

namespace DeftSharp.Windows.Input.Mouse;

internal interface INativeMouseInterceptor : IRequestedInterceptor
{
    event MouseInputDelegate? MouseInput;
    bool IsKeyPressed(MouseButton button);
    Point GetPosition();
    void SetPosition(int x, int y);
    void SetMouseSpeed(int speed);
    void Click(int x, int y, MouseButton button);
    void Simulate(int x, int y, MouseSimulateOption option);
    void Scroll(int rotation);
}