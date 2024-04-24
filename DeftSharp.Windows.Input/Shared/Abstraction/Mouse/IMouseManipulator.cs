using System;
using System.Collections.Generic;

namespace DeftSharp.Windows.Input.Mouse;

public interface IMouseManipulator : IDisposable
{
    IEnumerable<MouseInputEvent> LockedKeys { get;}
    
    event Action<MouseInputEvent> InputPrevented;

    bool IsKeyLocked(PreventMouseOption mouseEvent);
    
    void SetPosition(int x, int y);
    void SetMouseSpeed(int speed);
    void Click(int x, int y, MouseButton button = MouseButton.Left);
    void Click(MouseButton button = MouseButton.Left);

    void Simulate(int x, int y, MouseSimulateOption simulateOption);
    void Simulate(MouseSimulateOption simulateOption);

    void DoubleClick(int x, int y);
    void DoubleClick();

    void Scroll(int rotation);

    void Prevent(PreventMouseOption preventOption, Func<bool>? predicate = null);
    void Release(PreventMouseOption preventOption);
    void Release();
}