//// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using System;

using ContextLabCar.Core;

namespace TestContextLabCar;  // Note: actual namespace depends on the project name.
internal class Program
{
  static void Main(string[] args)
  {
    Console.WriteLine("Hello World!  \n  Test dll  ContextLabCar");
    var _connectLabCar =  new ConnectLabCar(@"c:\1.txt", @"c:\2.txt");
  }
}