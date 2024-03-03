namespace DeftSharp.WPF.Keyboard.Tests;

// ReSharper disable once InconsistentNaming
internal sealed class WPFEmulator
{
    public bool Run(Action onAction)
    {
        var threadFinished = new ManualResetEvent(false);
        
        new Thread(() =>
        {
            onAction();
            threadFinished.Set();
        }).Start();
        
        return threadFinished.WaitOne(3000);
    }
}