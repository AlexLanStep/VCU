
using System.Reactive.Joins;

namespace LabCarContext20.Core.Ari;

public interface IArithmetic
{
  Arithmetic Initialization(string str);
  dynamic? Result { get; set; }

}

public class Arithmetic : IArithmetic
{

//  public readonly AriPattern APattern  = new AriPattern();
  public readonly AriLogicCall ALogicCall;

  private CollapseBrackets collapseBrackets;
  private string _str;
//  private bool? _isCase = null;
  private readonly ILoggerDisplay _iDisplay;
  private readonly IAllDan _iallDan;
  public dynamic? Result { get; set; }
  public Arithmetic(ILoggerDisplay iDisplay, IAllDan iallDan)
  {
    _iallDan = iallDan;
    _iDisplay = iDisplay;
//    APattern = new AriPattern();
    collapseBrackets = new CollapseBrackets();
    ALogicCall = new AriLogicCall(iallDan);
    Result = null;

  }


  public Arithmetic? Initialization(string str)
  {
    Result = null;
    _str = str.Replace(" ", "");
    var _isSymbol = ALogicCall.TestInputStr(_str);
    if (_isSymbol == null)
    {
//      _iDisplay.Write(s);
      throw new MyException(" Проблема в строке (в стратегии)! ", -10);
    }

    if (_isSymbol.Value)
    {
      _iDisplay.Write("Строка вычислений ");
      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariable>(_str);
      foreach (var it in _collapseBrakets.LBrakets)
      {
        string _key = it;
        string sval = _collapseBrakets.DBrakets[it].StrCommand;
        Result = ALogicCall.InputStrArifmet(sval, _collapseBrakets.DBrakets);
        if (Result == null)
        {
          throw new MyException($" Error in string, no variable {sval} ", -20);
        }
        else
        {
          var _z0 = _collapseBrakets.DBrakets[it];
          _z0.Value = Result;
          _collapseBrakets.DBrakets[it] = _z0;
        }
      }
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