namespace DeftSharp.Windows.Input.Native.System;

/// <summary>
/// Defines a delegate for processing windows events.
/// </summary>
/// <param name="nCode">Specifies whether the hook procedure must process the message.</param>
/// <param name="wParam">Specifies additional information about the message.</param>
/// <param name="lParam">Specifies additional information about the message.</param>
/// <returns>A handle to the next hook procedure in the chain or <c>0</c> if there's no next procedure.</returns>
internal delegate nint WindowsProcedure(int nCode, nint wParam, nint lParam);