
using System.Reactive.Joins;

namespace LabCarContext20.Core.Ari;

public interface IArithmetic
{
  Arithmetic Initialization(string str);
}

public class Arithmetic : IArithmetic
{

  public AriPattern APattern { get; set; }
  private string _str;
  private bool? _isCase = null;
  private readonly ILoggerDisplay _iDisplay;

  public Arithmetic(ILoggerDisplay iDisplay)
  {
    _iDisplay = iDisplay;
    APattern = new AriPattern();
  }

  public Arithmetic Initialization(string str)
  {
    _str = str;
    var _isSymbol = APattern.TestInputStr(_str);
    if (_isSymbol == null)
    {
//      _iDisplay.Write(s);
      throw new MyException(" Проблема в строке (в стратегии)! ", -10);
    }

    if(_isSymbol.Value)
    {
      _iDisplay.Write("Строка вычислений ");
    }
    else
    {
      _iDisplay.Write("Строка условий ");
    }

    return this;
  }
}

