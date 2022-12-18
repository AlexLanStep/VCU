using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace LabCarContext20.Core.Ari.Interfacexxxx11111;


public class xxxxCVariableBase : ICVariableBase
{
    public xxxxCVariableBase(string name, string strComand) => Instal(name, strComand);

    public xxxxCVariableBase()
    {
        Name = "";
        StrCommand = "";
        IsValue = false;
    }

    public void Instal(string name, string strComand)
    {
        Name = name;
        StrCommand = strComand;
        IsValue = false;
    }
    public string Name { get; set; }
    public string StrCommand { get; set; }
    public bool IsValue { get; set; }

}

public interface ICVariable : ICVariableBase
{
    dynamic? Value { get; set; }
    string SValue { get; set; }
}

public class IcVariable : xxxxCVariableBase, ICVariable
{
    public dynamic? Value { get; set; }
    public string SValue
    {
        get => Value == null ? "" : Value.ToString();
        set { }
    }

    public IcVariable(string name, string strCommand) : base(name, strCommand)
    {
        Value = null;
        SValue = "";
    }
    public IcVariable(string strComand)
    {
        var ss = strComand.Split('=');
        if (ss.Length != 2)
            throw new MyException($"Неправильный формат переменных в разборе арефметической строки -> {strComand}", -5);

        Instal(ss[0], ss[1]);

        Value = null;
        SValue = "";
    }

    public IcVariable() : base()
    {
        Value = null;
        SValue = "";
    }
}


public interface ICVariableLogic : ICVariableBase, ICVariable
{
    bool? IsLogic { get; set; }
}

public interface ICVariableBase
{
}

public class IcVariableLogic : IcVariable, ICVariableLogic
{
    public bool? IsLogic { get; set; }

    public IcVariableLogic(string name, string strCommand) : base(name, strCommand)
    {
        IsLogic = null;
    }

    public IcVariableLogic() : base()
    {
        IsLogic = null;

    }
}