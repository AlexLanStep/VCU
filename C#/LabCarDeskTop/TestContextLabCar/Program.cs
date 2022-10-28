
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

    _container = ContainerManager.GetInstance();
    var _connect = _container.LabCar.Resolve<IConnectLabCar>();
    var _jsonConfig = _container.LabCar.Resolve<IStrategyDanJson>();
    Console.WriteLine("=========   Stratedy 0   =======");
    var _testIlab0 = _container.LabCar.Resolve<IStrategiesBasa>();
    _testIlab0.RunInit(@"D:\TestSystem\Moto\Strategies\St0");
    _testIlab0.RunTest();


    Console.WriteLine("==== END === !!!! ");
  }

}