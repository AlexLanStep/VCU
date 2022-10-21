namespace ContextLabCar.Core.Interface;
public interface IStOneStep
{
    Dictionary<string, dynamic> GetPoints { get; set; }
    List<string> LRezult { get; set; }
    Dictionary<string, dynamic> SetPoints { get; set; }
    int TimeWait { get; set; }

    bool RezultEq(dynamic x0, dynamic x1);
    bool RezultGe(dynamic x0, dynamic x1);
    bool RezultGt();
    bool RezultLe();
    bool RezultLt();
    bool RezultNe();
    bool TestDan(Dictionary<string, dynamic> rezul);
}