
using System;

using ContextLabCar.Core;
using ContextLabCar.Core.Config;
using ContextLabCar.Core.Strategies;
using ContextLabCar.Static;
using DryIoc;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
internal class Program
{
  static ContainerManager _container;
  static void Main(string[] args)
  {
    _container = ContainerManager.GetInstance();
    var _connect = _container.LabCar.Resolve<IConnectLabCar>();
    var _jsonConfig = _container.LabCar.Resolve<IStrategDanJson>();
    _jsonConfig.InicialJson(@"D:\TestSystem\Moto\Strategies\St0");



    Console.WriteLine("Hello World!  \n  Test dll  ContextLabCar");
  }
}