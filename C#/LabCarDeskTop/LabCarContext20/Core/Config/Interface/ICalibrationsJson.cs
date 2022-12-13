namespace LabCarContext20.Core.Config.Interface;

public interface ICalibrationsJson
{
    string Signal { get; }
    dynamic Val { get; }
    string Comment { get; }
    string Text { get; }
}