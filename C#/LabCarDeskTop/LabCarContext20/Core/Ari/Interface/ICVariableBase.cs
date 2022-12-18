namespace LabCarContext20.Core.Ari
{
  public interface ICVariableBase
  {
    bool IsValue { get; set; }
    string Name { get; set; }
    string StrCommand { get; set; }
//    public dynamic? Value { get; set; }
    void Instal(string name, string strCommand);
    void Run();
  }
}