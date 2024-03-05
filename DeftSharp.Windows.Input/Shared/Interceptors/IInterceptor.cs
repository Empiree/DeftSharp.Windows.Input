namespace DeftSharp.Windows.Input.Shared.Interceptors;

public interface IInterceptor
{
    void Hook();
    void Unhook();
}