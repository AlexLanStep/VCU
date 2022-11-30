using ContextLabCar.Static;
using DryIoc.ImTools;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ContextLabCar.Core;

public interface ICalcElement
{
}
public class CalcElement: ICalcElement
{
  public string BasaCommanda { get; set; }
//  public Dictionary<string, string> DComands { get; set; }
//  public OneElement oneElement { get; set; }

  private readonly string _nameTree = "__#";
  private int _indexCom = 0;

  public CalcElement(string name)
  {
    _nameTree = _nameTree+name;

//    DComands = new Dictionary<string, string>();
  }

  public void CaclScobki(string scobki)
  {
    BasaCommanda = scobki;
//    DComands.Clear();
    while (StArithmetic.IsScobki(scobki).Item1)
    {
      var _st0 = scobki;
      var xScop = StArithmetic.ScobkiX(scobki);

      if (xScop.Count <= 0)
        continue;

      var x0 = xScop.ElementAt(0);
  
      string ssx = scobki.Substring(x0.Item1, x0.Item2 - x0.Item1 + 1);
      var _nameTreeX = _nameTree + _indexCom;
      _st0 = _st0.Replace(ssx, _nameTreeX);
      ssx = ssx.Replace("(", "").Replace(")", "");
//      DComands.Add(_nameTreeX, ssx);
      
      var _s00 = _nameTreeX + "=" + ssx;
      var cv = new CVariable(_s00);
      if(!cv.IsValue)
        StArithmetic.DVarCommand.AddOrUpdate(_nameTreeX, cv, (_, _)=>cv);

      xScop.RemoveAt(0);
      scobki = _st0;
      _indexCom++;
    }
    BasaCommanda = scobki;
  }

  public void ReplaseSimvolDan()
  {
    var _keyVal = StArithmetic.DVarCommand.Where(_z=>_z.Value.IsValue==true)
          .Select(x=>new {x.Key, x.Value.Value }).ToList();

    Dictionary<string, string> _d= new();
    foreach (var item in _keyVal)
      _d.Add(item.Key, ((string)Convert.ToString(item.Value)).Replace(',', '.'));

    foreach (var it in _d)
    {
      bool b = BasaCommanda.Contains(it.Key);
      
        BasaCommanda.FirstOrDefault()
    }    

  }

}
//OneElement
