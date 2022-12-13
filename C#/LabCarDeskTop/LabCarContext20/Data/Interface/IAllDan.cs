namespace LabCarContext20.Data.Interface;

public interface IAllDan
{
  void Add<T>(string name, T dan);
  object? GetT<T>(string name);
  dynamic? Get(string name);
}