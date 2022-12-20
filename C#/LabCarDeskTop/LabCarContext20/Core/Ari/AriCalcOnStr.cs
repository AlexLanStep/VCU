
//using System.Reactive.Joins;

//namespace LabCarContext20.Core.Ari;

////public interface IAriCalcOnStr
////{
////  AriCalcOnStr CalcStr(string str);

////}

//public class AriCalcOnStr : IAriCalcOnStr
//{

////  public readonly AriPattern APattern  = new AriPattern();
//  public readonly AriStrDisassemble ALogicCall;

//  private CollapseBrackets collapseBrackets;
//  private string _str;
////  private bool? _isCase = null;
//  private readonly ILoggerDisplay _iDisplay;
//  private readonly IAllDan _iallDan;
//  public dynamic? Result { get; set; }
//  public AriCalcOnStr(ILoggerDisplay iDisplay, IAllDan iallDan)
//  {
//    _iallDan = iallDan;
//    _iDisplay = iDisplay;
////    APattern = new AriPattern();
//    collapseBrackets = new CollapseBrackets();
//    ALogicCall = new AriStrDisassemble(iallDan);
//    Result = null;

//  }


//  public AriCalcOnStr? CalcStr(string str)
//  {
//    Result = null;
//    _str = str.Replace(" ", "");
//    var _isSymbol = ALogicCall.TestInputStr(_str);
//    if (_isSymbol == null)
//    {
//      throw new MyException($" Проблема в строке (в стратегии)! -> {_str} ", -10);
//    }

//    if (_isSymbol.Value)
//    {
//      _iDisplay.Write("Строка вычислений ");
//      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariable>(_str);
//      foreach (var it in _collapseBrakets.LBrakets)
//      {
//        string _key = it;
//        string sval = _collapseBrakets.DBrakets[it].StrCommand;
//        Result = ALogicCall.InputStrArifmet(sval, _collapseBrakets.DBrakets);
//        if (Result == null)
//        {
//          throw new MyException($" Error in string, no variable {sval} ", -20);
//        }
//        else
//        {
//          var _z0 = _collapseBrakets.DBrakets[it];
//          _z0.Value = Result;
//          _collapseBrakets.DBrakets[it] = _z0;
//        }
//      }
//    }
//    else
//    {
//      _iDisplay.Write("Строка условий ");
//      var _collapseBrakets = collapseBrackets.CalcBrakets<CVariableLogic>(_str);
//      if (_collapseBrakets == null) 
//      { 
//        throw new MyException($" Проблема в строке (в стратегии)! -> {_str} ", -10);
//      }



//    }

//    return this;
//  }
//}

///*
 
//     BasaCommanda = scobki;
//    while (StArithmetic.IsScobki(scobki).Item1)
//    {
//      var st0 = scobki;
//      var xScop = StArithmetic.ScobkiX(scobki);

//      if (xScop.Count <= 0)
//        continue;

//      var x0 = xScop.ElementAt(0);

//      var ssx = scobki.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
//      var nameTreeX = _nameTree + _indexCom;
//      st0 = st0.Replace(ssx, nameTreeX);
//      ssx = ssx.Replace("(", "").Replace(")", "");

//      var cv = new CVariable(nameTreeX + "=" + ssx);
//      if (!cv.IsValue)
//        StArithmetic.DVarCommand.AddOrUpdate(nameTreeX, cv, (_, _) => cv);

//      xScop.RemoveAt(0);
//      scobki = st0;
//      _indexCom++;
//    }
//    BasaCommanda = scobki;

 
 
// */