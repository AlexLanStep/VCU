
namespace LabCarContext20.Core.Ari;

public interface IArithmetic
{
  Arithmetic Initialization(string str);
}

public class Arithmetic : IArithmetic
{
  private string _str;
  private bool? _isCase = null;
  public Arithmetic Initialization(string str)
  {
    _str = str;
    return this;
  }
}

