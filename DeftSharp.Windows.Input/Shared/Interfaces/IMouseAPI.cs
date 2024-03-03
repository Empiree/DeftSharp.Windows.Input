using System;
using DeftSharp.Windows.Input.InteropServices.Mouse;

namespace DeftSharp.Windows.Input.Shared.Interfaces;

public interface IMouseAPI
{
    void Hook();
    void Unhook();

    event EventHandler<MouseInputArgs>? MouseInput;
}