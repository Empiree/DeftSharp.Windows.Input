using DeftSharp.Windows.Input.Interceptors;
using DeftSharp.Windows.Input.Keyboard;

namespace DeftSharp.Windows.Input.Shared.Delegates;

internal delegate InterceptorResponse KeyboardInputDelegate(KeyPressedArgs args);