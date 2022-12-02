
namespace ContextLabCar.Static;

public static class GetDanX
{
  public static dynamic? Get(string name)
  {
    var dLabCar = LcDan.GetTaskOld(name);
    if (dLabCar != null)
      return dLabCar;

    var dArif = StArithmetic.GetArif<dynamic>(name);
    if (dArif != null)
      return dArif;

    return null;
  }
}
