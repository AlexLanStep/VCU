
using System.Reactive.Joins;

namespace LabCarContext20.Core.Ari;

public interface IArithmetic
{
  Arithmetic Initialization(string str);
}

public class Arithmetic : IArithmetic
{

  public AriPattern APattern { get; set; }
  private CollapseBrackets collapseBrackets;
  private string _str;
  private bool? _isCase = null;
  private readonly ILoggerDisplay _iDisplay;

  public Arithmetic(ILoggerDisplay iDisplay)
  {
    _iDisplay = iDisplay;
    APattern = new AriPattern();
    collapseBrackets = new CollapseBrackets();
  }

  public Arithmetic? Initialization(string str)
  {
    _str = str.Replace(" ", "");
    var _isSymbol = APattern.TestInputStr(_str);
    if (_isSymbol == null)
    {
//      _iDisplay.Write(s);
      throw new MyException(" Проблема в строке (в стратегии)! ", -10);
    }

//    DanCollapseBrakets? _collapseBrakets;
    if (_isSymbol.Value)
    {
      _iDisplay.Write("Строка вычислений ");
      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariable>(_str);
    }
    else
    {
      _iDisplay.Write("Строка условий ");
      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariableLogic>(_str);
      if (_collapseBrakets == null) return null;
    }

    return this;
  }
}

/*
 
     BasaCommanda = scobki;
    while (StArithmetic.IsScobki(scobki).Item1)
    {
      var st0 = scobki;
      var xScop = StArithmetic.ScobkiX(scobki);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);

      var ssx = scobki.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      var nameTreeX = _nameTree + _indexCom;
      st0 = st0.Replace(ssx, nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");

      var cv = new CVariable(nameTreeX + "=" + ssx);
      if (!cv.IsValue)
        StArithmetic.DVarCommand.AddOrUpdate(nameTreeX, cv, (_, _) => cv);

      xScop.RemoveAt(0);
      scobki = st0;
      _indexCom++;
    }
    BasaCommanda = scobki;

 
 
 */