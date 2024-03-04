using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interfaces;

public interface IMouseAPI
{
    void Hook();
    void Unhook();

    Coordinates GetPosition();
    
    event EventHandler<MouseInputArgs>? MouseInput;
}