

using Newtonsoft.Json.Linq;

namespace LabCarContext20.Core.Ari;

public class CVariableBase : ICVariableBase
{
  public CVariableBase(string name, string strCommand) => Instal(name, strCommand);

  public CVariableBase()
  {
    Name = "";
    StrCommand = "";
  }

  public void Instal(string name, string strCommand)
  {
    Name = name;
    StrCommand = strCommand;
  }

  public virtual void Run()
  {
    
  }

  public string Name { get; set; }
  public string StrCommand { get; set; }
//  public dynamic? Value { get; set; }

  public bool IsValue {get; set;}
}

public class CVariable : CVariableBase, ICVariable
{
  private dynamic? _value;

  public dynamic? Value
  {
    get => _value;
    set
    {
      if (value == null)
      {
        IsValue=false;
        _value = null;
      }
      else
      {
        _value =  value;
        IsValue = true;
      }
    }
  }

  public string SValue {
    get => Value == null?"":Value.ToString();
    set { }
  }
  

  public CVariable(string name, string strCommand):base(name, strCommand) 
  {
    Value = null;
    SValue = "";
  }
  public CVariable(string  strCommand)
  {
    var ss = strCommand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Неправильный формат переменных в разборе арефметической строки -> {strCommand}", -5);

    Instal(ss[0], ss[1]);

    Value = null;
    SValue = "";
  }

  public CVariable() : base() 
  {
    Value = null;
    SValue = "";
  }

  public override void Run()
  {
  }
}

public class CVariableLogic : CVariable, ICVariableLogic
{
  public bool? IsLogic { get; set; }

  public CVariableLogic(string name, string strCommand) : base(name, strCommand)
  {
    IsLogic = null;
  }

  public CVariableLogic():base() 
  {
    IsLogic = null;

  }
}