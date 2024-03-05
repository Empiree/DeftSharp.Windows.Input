using System;
using System.Collections.Generic;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IMouseInterceptor : IRequestedInterceptor
{
    IEnumerable<MouseEvent> LockedKeys { get;}
    
    Coordinates GetPosition();

    void Prevent(MouseEvent mouseEvent);
    void Release(MouseEvent mouseEvent);
    void ReleaseAll();

    event Action<MouseEvent> ClickPrevented; 
    event Action<MouseEvent> ClickReleased;
    event EventHandler<MouseInputArgs>? MouseInput;
}