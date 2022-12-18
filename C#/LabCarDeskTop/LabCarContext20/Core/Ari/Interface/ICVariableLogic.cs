namespace LabCarContext20.Core.Ari;

public interface ICVariableLogic : ICVariableBase, ICVariable
{
  bool? IsLogic { get; set; }
}