using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interceptors.Mouse;

internal interface IMouseManipulatorInterceptor : IRequestedInterceptor
{
    IEnumerable<MouseEvent> LockedKeys { get;}
    
    void SetPosition(int x, int y);
    void Click(int x, int y, MouseButton button);
    void Prevent(MouseEvent mouseEvent);
    void Release(MouseEvent mouseEvent);
    void ReleaseAll();
    
    event Action<MouseEvent> ClickPrevented; 
    event Action<MouseEvent> ClickReleased;
}