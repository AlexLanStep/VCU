namespace LabCarContext20.Data.Interface;

public interface IDanBase<T> where T : class
{
    void Run();
    void Add(string name, T tDan);
    T? GetT(string name);
    dynamic ? Get(string name);
}