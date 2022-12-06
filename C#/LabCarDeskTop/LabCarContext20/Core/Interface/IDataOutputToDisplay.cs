namespace LabCarContext20.Core.Interface;

public interface IDataOutputToDisplay
{
    void InitializationConsole();
    void InitializationWindows();
    void Write(string str);
    event AsyncEventHandler? AsyncEvent;
}