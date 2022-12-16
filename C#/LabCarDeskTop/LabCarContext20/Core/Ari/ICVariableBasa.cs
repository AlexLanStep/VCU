namespace LabCarContext20.Core.Ari
{
  public interface ICVariableBasa
  {
    bool IsValue { get; set; }
    string Name { get; set; }
    string StrComand { get; set; }
    void Instal(string name, string strComand);
  }
}