
//namespace ContextLabCar.Core;

//public interface IArAction
//{
//  string? Name { get; set; }
//  void SetStr(string str);
//  string NameValue { get; set; }

//}
//public class ArAction : IArAction
//{

//  public string? Name { get; set; }
//  public string NameValue { get; set; }

//  private string _str0;
//  public string CommandRoot { get; set; }
//  public List<string> CommandList { get; set; }
//  private readonly string _nameTree = "__#";
//  private int _indexCom = 0;
//  public ArElement? arElement { get; set; }
//  private Dictionary<string, ArElement> arElements = new();

//  public ArAction(string name)
//  {
//    var strBasa = name.Replace(" ", "");
//    var s0 = strBasa.Split("=");
//    NameValue = s0[0];
//    CommandRoot = s0[1];
//    arElement = new ArElement("root", s0[1], null);
//    CommandList = new() { CommandRoot };
//  }

//  public void SetStr(string str)
//  {
//    _str0 = str;

//    if (!StArithmetic.IsScobki(str).Item1) return;

//    var scobkiX = StArithmetic.ScobkiX(_str0);
//    var k = 0;
//    while (k< scobkiX.Count - 1)
//    {
//      var x0 = scobkiX.ElementAt(k);
//      var x1 = scobkiX.ElementAt(k+1);
//      if(x0.Item2< x1.Item2)
//      {
//        var ssx = str.Substring(x0.Item1, x0.Item2- x0.Item1);
//        var namevolume = _nameTree + _indexCom;
//        str = str.Replace(ssx, namevolume);
//        ssx = ssx.Replace("(", "").Replace(")","");
//        arElements.Add(namevolume, new ArElement(namevolume, ssx, arElement));
//        k = scobkiX.Count;
//      }
//      k++;
//    }
//  }


//}
