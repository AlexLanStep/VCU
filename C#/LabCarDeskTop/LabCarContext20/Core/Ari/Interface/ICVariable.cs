namespace LabCarContext20.Core.Ari;

public interface ICVariable : ICVariableBase
{
  dynamic? Value { get; set; }
  string SValue { get; set; }
}