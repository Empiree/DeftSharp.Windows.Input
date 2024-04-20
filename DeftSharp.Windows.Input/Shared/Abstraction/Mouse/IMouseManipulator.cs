using System;
using System.Collections.Generic;

namespace DeftSharp.Windows.Input.Mouse;

public interface IMouseManipulator : IDisposable
{
    IEnumerable<MouseInputEvent> LockedKeys { get;}
    
    event Action<MouseInputEvent> InputPrevented;

    bool IsKeyLocked(PreventMouseEvent mouseEvent);
    
    void SetPosition(int x, int y);
    void SetMouseSpeed(int speed);
    void Click(int x, int y, MouseButton button = MouseButton.Left);
    void Click(MouseButton button = MouseButton.Left);

    void DoubleClick(int x, int y);
    void DoubleClick();

    void Scroll(int rotation);

    void Prevent(PreventMouseEvent mouseEvent, Func<bool>? predicate = null);
    void Release(PreventMouseEvent mouseEvent);
    void Release();
}