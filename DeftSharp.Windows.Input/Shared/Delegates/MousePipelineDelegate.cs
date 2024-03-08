using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Shared.Interceptors.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Delegates;

internal delegate InterceptorResponse MousePipelineDelegate(MouseInputArgs args);