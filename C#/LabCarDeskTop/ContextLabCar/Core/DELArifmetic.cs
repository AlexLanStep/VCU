//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ContextLabCar.Core;

//public interface IArifmetic
//{
//  void LoadStr(string str);
//  string NameValume { get; set; }
//}
//public class Arifmetic : IArifmetic
//{
//  public string NameValume { get; set; }
//  private string _strValume;
//  private string _valumeCommand;
//  private const string _patternLifet = @"\(";
//  private const string _patternRite = @"\)";

//  public Arifmetic()
//  {
//  }

//  private void allScobci()
//  {
//    var x0Lcount = Regex.Matches(_valumeCommand, _patternLifet, RegexOptions.IgnoreCase).Count;
//    var x0Rcount = Regex.Matches(_valumeCommand, _patternRite, RegexOptions.IgnoreCase).Count;
//    var xxxl = Regex.Matches(_valumeCommand, _patternLifet, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
//    var xxxr = Regex.Matches(_valumeCommand, _patternRite, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();

//  }
//  public void LoadStr(string str)
//  {
//    _strValume = str;
//    var _s= _strValume.Split("=");
//    NameValume = _s[0];
//    _valumeCommand = _s[1];
//    allScobci();

//  }
//}

///*
//       List<string> s0 = new List<string>()
//      {
//        "sas = Butt_Drive_State + 3", "ss11 = VCU_DesInvMode - 2 + sas", "ee =(5+ ss11)*2.0 - 66 + (4* (sas + (ss11 + 101)*2 ) + sas) / ee + (10+8)/7"
//      };
//      foreach (var item in s0)
//      {
//        Console.WriteLine(item.Replace(" ",""));
//        var id = Guid.NewGuid().ToString();
//        //        Console.WriteLine(id);
//        var ur = item.Split('=');

//        var i0 = ur[1].IndexOf("(");
//        if(i0 == -1)
//        {
//          Console.WriteLine("Нет скобок");
//        } 
//        else
//        {
//          var _patternLifet = @"\(";
//          var _patternRite = @"\)";
//          Console.WriteLine("Есть скобки");

//          var _newSig = ur[1];
//          var x0Lcount = Regex.Matches(_newSig, _patternLifet, RegexOptions.IgnoreCase).Count;
//          var x0Rcount = Regex.Matches(_newSig, _patternRite, RegexOptions.IgnoreCase).Count;
//          var xxxl = Regex.Matches(_newSig, _patternLifet, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();
//          var xxxr = Regex.Matches(_newSig, _patternRite, RegexOptions.IgnoreCase).Select(x => x.Index).ToList();


//          var countx = _newSig.Length;

//          List<(int, int)> xScop = new();
//          int _valR = 0;
//          int _valL = 0;

//          while (xxxr.Count()>0)
//          {
//            _valR = xxxr.ElementAt(0);
//            bool _is = true;
//            int kL = 0;
//            int kLMax = xxxl.Count();
//            while (_is & xxxl.Count() > 0)
//            {
//              _valL = xxxl.ElementAt(kL);

//              if ((_valR > _valL) & (_valR < xxxl.ElementAt(Math.Min(kL + 1, kLMax-1))) 
//                        || (xxxl.Count == 1 && xxxr.Count == 1))
//              {
//                xScop.Add((_valL, _valR));
//                _is= false;
//                xxxl.RemoveAt(kL);
//                xxxr.RemoveAt(0);
//                continue;
//              }
//              kL++;
//            }
//          }


//        }

//      }


// */