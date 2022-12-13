
namespace LabCarContext20.Data.Interface;

public interface IDanCalibrations2 : IGetDan //: IDanBase<T> where T : class
{
    void Add(string name, Calibrations2 calidr);
    void Load(string name);
    void Action(string name);
}