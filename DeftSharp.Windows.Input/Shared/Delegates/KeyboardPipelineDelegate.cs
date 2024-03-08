using DeftSharp.Windows.Input.InteropServices.Keyboard;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Delegates;

internal delegate InterceptorResponse KeyboardPipelineDelegate(KeyPressedArgs args);