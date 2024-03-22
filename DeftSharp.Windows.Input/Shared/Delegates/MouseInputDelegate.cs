using DeftSharp.Windows.Input.InteropServices.Mouse;
using DeftSharp.Windows.Input.Pipeline;

namespace DeftSharp.Windows.Input.Shared.Delegates;

public delegate InterceptorResponse MouseInputDelegate(MouseInputArgs args);