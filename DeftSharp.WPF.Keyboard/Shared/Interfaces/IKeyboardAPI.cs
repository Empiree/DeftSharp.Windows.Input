using System;
using DeftSharp.Windows.Keyboard.InteropServices.Keyboard;

namespace DeftSharp.Windows.Keyboard.Shared.Interfaces;

public interface IKeyboardAPI
{
    void Hook();
    void Unhook();
    
    event EventHandler<KeyPressedArgs>? KeyPressed;
}