
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
//    DateTime _dt0 = DateTime.Now;
//    Thread.Sleep(2100); 
//    DateTime _dt1 = DateTime.Now;
//    TimeSpan duration = (DateTime.Now - _dt0);
//    var xx = (DateTime.Now - _dt0).Seconds;

    _container = ContainerManager.GetInstance();
    var _connect = _container.LabCar.Resolve<IConnectLabCar>();
    var _jsonConfig = _container.LabCar.Resolve<IStrategDanJson>();
    var _testIlab = _container.LabCar.Resolve<IStrategiesBasa>();
    _testIlab.RunInit(@"D:\TestSystem\Moto\Strategies\St0");


    //var tast0 = Task.Factory.StartNew(() =>
    //{
    //  var _testIlab = _container.LabCar.Resolve<IStrategiesBasa>();
    //  _testIlab.RunInit(@"D:\TestSystem\Moto\Strategies\St0");

    //});
    //var tast1 = Task.Factory.StartNew(() =>
    //{
    //  var _testIlab = _container.LabCar.Resolve<IStrategiesBasa>();
    //  _testIlab.RunInit(@"D:\TestSystem\Moto\Strategies\St1");

    //});

    //Task.WaitAll(tast0, tast1);
    Console.WriteLine("Hello World!  \n  Test dll  ContextLabCar");
  }

}