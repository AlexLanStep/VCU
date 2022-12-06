#define TestDanLabCar
#define DataEmulation

using LabCarContext20.Core;
using LabCarContext20.Core.Interface;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using DryIoc;
using LabCarContext20.Core.Test;
using LabCarContext20.Static;
namespace LabCar20;

public class Program
{
  private static ContainerManager _container = null!;

  static void Main(string[] args)
  {

    double[] d0 = new double[5] { 0.0, 1.0, 2.0, 3.0, 4.0 };
    double[,] d1 = new double[5, 1] { { 0.0}, { 1.0 }, { 2.0 }, { 3.0 }, { 4.0 } };
    double[,] d2 = new double[7, 2] { { 0.0, 1.1 }, { 1.0, 2.1 }, { 2.0, 2.1 }, 
                                      { 3.0, 3.1 }, { 4.0, 4.1 }, { 5.0, 5.1 }, { 6.0, 6.1 } };

    ConcurrentDictionary<string, double[,]> MDan = new ();
    MDan.AddOrUpdate("VDan", d1, ((s, doubles) => d1));
    MDan.AddOrUpdate("MDan", d2, ((s, doubles) => d2));



    Console.WriteLine(d0.GetType().Name);
    Console.WriteLine(d1.GetType().Name);

    Console.WriteLine(d1.Rank);
    Console.WriteLine(d1.Length);
    Console.WriteLine(d1.GetLength(0));
    Console.WriteLine(d1.GetLength(1));

    //    double[][] d1 = new double[5][1] { 0.0, 1.0, 2.0, 3.0, 4.0 };

    _container = ContainerManager.GetInstance();
    var iDisplay = _container.LabCar.Resolve<IDataOutputToDisplay>();
    iDisplay.InitializationConsole();
//    var _iWriteWin = _container.LabCar.Resolve<ITWriteWin>();
//    _iWriteWin.WriteYes();
    iDisplay.Write("__ Start program LabCar ver - 2.0 ");



    iDisplay.Write(" ==--  End program  == ");

    Thread.Sleep(500);
  }
}

