
using System;

using ContextLabCar.Core;
using ContextLabCar.Core.Config;
using ContextLabCar.Core.Strategies;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
internal class Program
{
  static void Main(string[] args)
  {
    //object xx = "sss";
    //Console.WriteLine(xx.GetType());
    //xx = 11.222;
    //Console.WriteLine(xx.GetType());
    //xx = 22+3;
    //Console.WriteLine(xx.GetType());
    //xx = new FestWert("TEST/Low_Beam_Test", "BatteryIsOn", "0.001", "Включение баттареи");
    //Console.WriteLine(xx.GetType());

    var _stJson = new StrategDanJson(@"e:\Strateg0.json");
    _stJson.InicialJson();

    Console.WriteLine("Hello World!  \n  Test dll  ContextLabCar");

    string s = new FestWert("TEST/Low_Beam_Test", "BatteryIsOn", "0.001", "Включение баттареи").Text;

//    var _connectLabCar =  new ConnectLabCar(@"c:\1.txt", @"c:\2.txt");
  }
}