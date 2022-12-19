﻿#define TestDanLabCar
#define DataEmulation

using LabCarContext20.Core;
using LabCarContext20.Core.Interface;
using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using DryIoc;
using LabCarContext20.Core.Test;
using LabCarContext20.Static;
using LabCarContext20.Core.Config;
using LabCarContext20.Data;
using LabCarContext20.Data.Interface;
using System.Xml.Linq;
using LabCarContext20.Core.Ari;
using LabCarContext20.Core.Strategies;

namespace LabCar20;

public class Program
{
  private static ContainerManager? _container = null;

  static void Main(string[] args)
  {
    string ssss = "-2,4";
    double.TryParse(ssss, out var rez);

    _container = ContainerManager.GetInstance();
    ContainerManager.Initialization();
    var iDisplay = _container.LabCar.Resolve<ILoggerDisplay>();

    iDisplay.InitializationConsole();
    iDisplay.Write("__ Start program LabCar ver - 2.0 ");

    IArithmetic _iaPattern = _container.LabCar.Resolve<IArithmetic>();
    IStSetDan _stSetDan = _container.LabCar.Resolve<IStSetDan>();
    List<string> _list = new List<string>(){ "ss11=12.2", "ds1= 9.2", "sas=2.3", "ee1=-3.3" };
    _stSetDan.SetDans(_list);
//    string s00x = "(((ww > (33.4*e1)) & (ee < ((44,3+eee)/sss))) | (3>e)) & (r~=(-t))";
// string s00x = "(r~=t)";
//    string s00x = "(r~=t)";
//    string s00x = "r=333 / t";
    string s00x = "ee =(5+ ss11 - ds1*3.1 - 21.2/3)*2.0 - 66/2.1 + (4* (sas + (ss11 + 101)*2 ) + sas) / ee1 + (10.2+8*(-2.3))/7.3";
    var ddd=     _iaPattern.Initialization(s00x);


    IConnectLabCar _connectLabCar = _container.LabCar.Resolve<IConnectLabCar>();
    //    _connectLabCar.Initialization(path1, path2);
//    IDanBase<CReadLC> danReadLc = _container.LabCar.Resolve<IDanBase<CReadLC>>();
    IDanReadLc danReadLc =(IDanReadLc) _container.LabCar.Resolve<DanDanReadLc>();
    IAllDan _iAllDan = _container.LabCar.Resolve<IAllDan>();

    //danReadLc.Run();
    //    danReadLc.Add("11", "222", "2222", "222");

    _iAllDan.Add<CReadLc>("axds", _container.LabCar.Resolve<CReadLc>().Initialization("11", "222", "333", "44444"));
    dynamic? _xxx = _iAllDan.Get("axds");

    CReadLc _cReadLc = (CReadLc) _iAllDan.GetT<CReadLc>("axds");

    _iAllDan.Add<dynamic>("sas", 334.22);
    dynamic? _xxx2 = _iAllDan.Get("sas");

    _iAllDan.Add<dynamic>("vec1", new double[5] { 0.0, 1.0, 2.0, 3.0, 4.0 });
    dynamic? vec1 = _iAllDan.Get("vec1");


    _iAllDan.Add<dynamic>("mas1", new double[7, 2] { { 0.0, 1.1 }, { 1.0, 2.1 }, { 2.0, 2.1 },
      { 3.0, 3.1 }, { 4.0, 4.1 }, { 5.0, 5.1 }, { 6.0, 6.1 } });
    dynamic? mas1 = _iAllDan.Get("mas1");

    iDisplay.Write(" ==--  End program  == ");

//    IAllDan, AllDan
    Thread.Sleep(500);
  }
}

/*

////////    Пример обработки массива

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
 
//    var _iWriteWin = _container.LabCar.Resolve<ITWriteWin>();
//    _iWriteWin.WriteYes();
 
 */
