using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Mouse;

internal interface IMouseManipulator : IDisposable
{
    IEnumerable<MouseEvent> LockedKeys { get;}
    
    event Action<MouseEvent> ClickPrevented;
    
    bool IsKeyLocked(MouseEvent mouseEvent);
    
    void SetPosition(int x, int y);
    void Click(int x, int y, MouseButton button = MouseButton.Left);
    void Click(MouseButton button = MouseButton.Left);

    void DoubleClick(int x, int y);
    void DoubleClick();
    
    void Prevent(PreventMouseOption mouseEvent);
    void Release(PreventMouseOption mouseEvent);
    void ReleaseAll();
}