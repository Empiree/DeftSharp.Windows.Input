namespace DeftSharp.Windows.Input.Tests;

/// <summary>
/// A class to start a new thread. It is needed because if you run WinAPI hooks in the main thread, the system may hang a lot.
/// </summary>
internal sealed class ThreadRunner
{
    public async Task Run(Action onAction)
    {
        var threadFinished = new ManualResetEvent(false);

        await Task.Run(() =>
        {
            onAction();
            threadFinished.Set();
        });

        threadFinished.WaitOne(3000);
    }
}