namespace ContextLabCar.Core.Interface;
public interface IStOneStep
{
    Dictionary<string, dynamic> GetPoints { get; set; }
    List<string> LResult { get; set; }
    Dictionary<string, dynamic> SetPoints { get; set; }
    int TimeWait { get; set; }

    bool ResultEq(dynamic x0, dynamic x1);
    bool ResultGe(dynamic x0, dynamic x1);
    bool ResultNe(dynamic x0, dynamic x1);
    bool ResultGt(dynamic x0, dynamic x1);
    bool ResultLe(dynamic x0, dynamic x1); // <= 
    bool ResultLt(dynamic x0, dynamic x1); // < 

  bool TestDan(Dictionary<string, dynamic> result);
}