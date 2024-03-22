using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Delegates;

public delegate InterceptorResponse KeyboardInputDelegate(KeyPressedArgs args);