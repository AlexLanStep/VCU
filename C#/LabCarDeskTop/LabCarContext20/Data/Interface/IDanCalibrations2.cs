
namespace LabCarContext20.Data.Interface;

public interface IDanCalibrations2 : IGetDan //: IDanBase<T> where T : class
{
    void Add(string name, Calibrations2? calidr);
    bool Load(List<string> name);
    bool Action(List<string> name);
}