using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Abstraction.Mouse;

internal interface IMouseManipulator : IDisposable
{
    IEnumerable<MouseEvent> LockedKeys { get;}
    
    bool IsKeyLocked(MouseEvent mouseEvent);
    
    void SetPosition(int x, int y);
    void Click(MouseButton button, int x, int y);
    void Click(MouseButton button);
    void Prevent(PreventMouseOption mouseEvent);
    void Release(PreventMouseOption mouseEvent);
    void ReleaseAll();
    
    event Action<MouseEvent> ClickPrevented;
}