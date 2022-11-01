#define DataEmulation


using System;

using ContextLabCar.Core;
using ContextLabCar.Core.Config;
using ContextLabCar.Core.Strategies;
using ContextLabCar.Static;
using DryIoc;
using Newtonsoft.Json;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
internal class Program
{
  static ContainerManager _container;
  static void Main(string[] args)
  {
#if DataEmulation
    Console.WriteLine(" ---  Включен режим эмуляция данных   --");
#else 
    Console.WriteLine(" Real dan");
#endif

    List<string> listDir = new List<string>() 
      { @"D:\TestSystem\Moto\Strategies\St0",
        @"D:\TestSystem\Moto\Strategies\St1",
        @"D:\TestSystem\Moto\Strategies\St2",
        @"D:\TestSystem\Moto\Strategies\St3",
        @"D:\TestSystem\Moto\Strategies\St4",
        @"D:\TestSystem\Moto\Strategies\St5",
        @"D:\TestSystem\Moto\Strategies\St6",
        @"D:\TestSystem\Moto\Strategies\St7"
      };

    _container = ContainerManager.GetInstance();
    var _connect = _container.LabCar.Resolve<IConnectLabCar>();
    var _jsonConfig = _container.LabCar.Resolve<IStrategyDanJson>();
    bool _isRezulta = true;

    int NStartStrateg = 0;
    int NEndStrateg = 1;
    int i = NStartStrateg;
    while (_isRezulta && i< NEndStrateg)
    {
      Console.WriteLine($"=========   Stratedy {i}   =======");
      var _testIlab = _container.LabCar.Resolve<IBaseContext>();
      _testIlab.Initialization(listDir.ElementAt(i));

      //var _testIlab = _container.LabCar.Resolve<IStrategiesBasa>();
      //_testIlab.RunInit(listDir.ElementAt(i));
      //_testIlab.RunTest();
      
      _isRezulta = _testIlab.IsResult;
      i += 1;
    }


    Console.WriteLine("==== END === !!!! ");
  }

}