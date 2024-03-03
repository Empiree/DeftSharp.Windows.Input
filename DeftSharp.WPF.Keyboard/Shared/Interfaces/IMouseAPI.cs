using System;
using DeftSharp.Windows.Keyboard.InteropServices.Mouse;

namespace DeftSharp.Windows.Keyboard.Shared.Interfaces;

public interface IMouseAPI
{
    void Hook();
    void Unhook();
    
    event EventHandler<MouseInputArgs>? MouseInput;
}