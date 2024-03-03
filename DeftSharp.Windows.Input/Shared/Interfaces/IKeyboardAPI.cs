using System;
using DeftSharp.Windows.Input.InteropServices.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Interfaces;

public interface IKeyboardAPI
{
    void Hook();
    void Unhook();

    event EventHandler<KeyPressedArgs>? KeyPressed;
}