
namespace MatLab;

using System;
using System.Formats.Tar;
using System.IO;
using csmatio.common;
using csmatio.io;
using csmatio.types;

public class MatLabConvert
{
  public Dictionary<string, dynamic> Dan { get; set; } = new ();
  private string _filePath;
  public MatLabConvert(string path)
  {
    _filePath = path;
    if (!File.Exists(_filePath))
    {
//      Console.WriteLine("Файл \"{0}\" не найден!", _filePath);
      return;
    }
    MatFileReader? matReader = null;
    try
    {
      matReader = new MatFileReader(_filePath);
    }
    catch (Exception)
    {
//      int ii = 1;
    }

    if (matReader == null)
      return;

    foreach (var key in matReader.Content.Keys)
    {
      var arr0 = matReader.Content[key];
      var _size = arr0.Dimensions;
      if (_size.Length != 2)
        continue;

      int iMas = _size[0];
      int jMas = _size[1];
      double[,] mm = new double[iMas, jMas];

      double[][] _arrV = ((MLDouble)matReader.Content[key]).GetArray();

      for (int i = 0; i < _arrV.Length; i++)
        for (int j = 0; j < jMas; j++)
          mm[i, j] = (_arrV[i])[j];
      Dan.Add(key, mm);
    }
  }
}