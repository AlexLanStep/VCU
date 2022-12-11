namespace LabCarContext20.Core.Interface;

public interface ILoggerDisplay
{
    void InitializationConsole();
    void InitializationWindows();
    void Write(string str);
    event AsyncEventHandler? AsyncEvent;
}