
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace LabCarContext20.Core.Ari;


public class CVariableBasa : ICVariableBasa
{
  public CVariableBasa(string name, string strComand) => Instal(name, strComand);

  public CVariableBasa()
  {
    Name = "";
    StrComand = "";
    IsValue = false;
  }

  public void Instal(string name, string strComand)
  {
    Name = name;
    StrComand = strComand;
    IsValue = false;
  }
  public string Name { get; set; }
  public string StrComand { get; set; }
  public bool IsValue { get; set; }

}

public interface ICVariable : ICVariableBasa
{
  dynamic? Value { get; set; }
  string SValue { get; set; }
}

public class CVariable : CVariableBasa, ICVariable
{
  public dynamic? Value { get; set; }
  public string SValue {
    get => Value == null?"":Value.ToString();
    set { }
  }

  public CVariable(string name, string strComand):base(name, strComand) 
  {
    Value = null;
    SValue = "";
  }
  public CVariable(string  strComand)
  {
    var ss = strComand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Неправильный формат переменных в разборе арефметической строки -> {strComand}", -5);

    Instal(ss[0], ss[1]);

    Value = null;
    SValue = "";
  }

  public CVariable() : base() 
  {
    Value = null;
    SValue = "";
  }
}


public interface ICVariableLogic : ICVariableBasa, ICVariable
{
  bool? IsLogic { get; set; }
}

public class CVariableLogic : CVariable, ICVariableLogic
{
  public bool? IsLogic { get; set; }

  public CVariableLogic(string name, string strComand) : base(name, strComand)
  {
    IsLogic = null;
  }

  public CVariableLogic():base() 
  {
    IsLogic = null;

  }
}