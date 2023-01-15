
using LabCarContext20.Core.Ari.Interface;

namespace LabCarContext20.Core.Ari;

public class CVariableBase : ICVariableBase
{
  #region MyRegion
  public string Name { get; set; } = "";
  public string StrCommand { get; set; } = "";
  public bool IsValue { get; set; }

  #endregion

  #region Constructor
  public CVariableBase(string name, string strCommand) => Instal(name, strCommand);
  public CVariableBase()
  {
  }
  #endregion
  public void Instal(string name, string strCommand)
  {
    Name = name;
    StrCommand = strCommand;
  }
  public virtual void Run()
  {
    
  }
}

public class CVariable : CVariableBase, ICVariable
{
  #region Data
  private dynamic? _value;
  public dynamic? Value
  {
    get => _value;
    set
    {
      if (value == null)
      {
        IsValue = false;
        _value = null;
      }
      else
      {
        _value = value;
        IsValue = true;
      }
    }
  }
  public string SValue
  {
    get => Value == null ? "" : Value.ToString();
    set { }
  }
  #endregion

  #region Constuctor
  public CVariable(string name, string strCommand) : base(name, strCommand)
  {
    Value = null;
    SValue = "";
  }
  public CVariable(string strCommand)
  {
    var ss = strCommand.Split('=');
    if (ss.Length != 2)
      throw new MyException($"Неправильный формат переменных в разборе арефметической строки -> {strCommand}", -5);

    Instal(ss[0], ss[1]);

    Value = null;
    SValue = "";
  }
  public CVariable()
  {
    Value = null;
    SValue = "";
  }
  #endregion

  public override void Run()
  {
  }
}

public class CVariableLogic : CVariable, ICVariableLogic
{
  #region Data
  public bool? IsLogic { get; set; }
  #endregion

  #region Constructur
  public CVariableLogic(string name, string strCommand) : base(name, strCommand)=>IsLogic = null;
  public CVariableLogic() => IsLogic = null;

  #endregion
}