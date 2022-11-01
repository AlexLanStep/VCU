// #define DataEmulation
//#if DataEmulation
//#else
//#endif

using ContextLabCar.Core;
using ContextLabCar.Core.Strategies;
using ContextLabCar.Static;
using DryIoc;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
internal class Program
{
  private static ContainerManager _container = null!;
  static void Main(string[] args)
  {

    var listDir = new List<string>() 
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
    _container.LabCar.Resolve<IConnectLabCar>();
//    _container.LabCar.Resolve<IStrategyDanJson>();
    var isRezulta = true;

    const int nStartStrategy = 0;
    const int nEndStrategy = 1;
    var i = nStartStrategy;
    while (isRezulta && i< nEndStrategy)
    {
      Console.WriteLine($"=========   Stratedy {i}   =======");
      var testLab = _container.LabCar.Resolve<IBaseContext>();
      testLab.Initialization(listDir.ElementAt(i));
      testLab.RunTest();
      isRezulta = testLab.IsResult;
      i += 1;
    }


    Console.WriteLine("==== END === !!!! ");
  }

}