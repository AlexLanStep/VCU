
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
/*
    LoadJsonParams _jsonParams = new LoadJsonParams();
    _jsonParams.ParamsStrategy.Add("NameStrateg", "StrateyBasa");
    _jsonParams.ParamsStrategy.Add("TimeWait", 1000);
    _jsonParams.ParamsStrategy.Add("Variable0", 10.2);
    _jsonParams.Parameters.Add("BatteryIsOn", new ParameterNew(signal: "TEST/Low_Beam_Test/BatteryIsOn/Value",
                                                              comment: "Test 001"));
    _jsonParams.Parameters.Add("Ignition", new ParameterNew(signal: "TEST/Low_Beam_Test/Ignition/Value"));
    _jsonParams.Parameters.Add("Butt_Drive_State", new ParameterNew(signal: "TEST/Low_Beam_Test/Butt_Drive_State/Value",
                                                              comment: "Comment 1"));
    _jsonParams.Output.Add("VCU_DesInvMode", new DanOutputNew(signal: "TEST/Low_Beam_Test/VCU_DesInvMode",
                                                              comment: " xxxxxxx"));
    _jsonParams.Output.Add("Low_Beam_Req", new DanOutputNew(signal: "TEST/Low_Beam_Test/Low_Beam_Req"));
    _jsonParams.Output.Add("Low_Beam_State", new DanOutputNew(signal: "TEST/Low_Beam_Test/Low_Beam_State",
                                                              comment: " zzzzzz"));
    Dictionary<string, CalibratNew> _cal0 = new Dictionary<string, CalibratNew>();
    _cal0.Add("BatteryIsOn", new CalibratNew("TEST/Low_Beam_Test/BatteryIsOn/Value", 0.0, "Кнопка баттареи"));
    _cal0.Add("Ignition", new CalibratNew("TEST/Low_Beam_Test/Ignition/Value", 0.0, "Зажигание"));
    _cal0.Add("Butt_Drive_State", new CalibratNew("TEST/Low_Beam_Test/Butt_Drive_State/Value", 0.0, "Кнопка устройства"));
    _jsonParams.Calibration.Add("Params0", new CalibrationParams()
    {
      PathFiles = "D:\\TestSystem\\Moto\\Strategies\\St0\\Params0.dcm",
      Parameter = _cal0
    });

    Dictionary<string, CalibratNew> _cal1 = new Dictionary<string, CalibratNew>();
    _cal1.Add("BatteryIsOn_qqq", new CalibratNew("TEST/Low_Beam_Test/BatteryIsOn/Value", 110.0, "Кнопка !!! баттареи"));
    _cal1.Add("Ignition_qqq", new CalibratNew("TEST/Low_Beam_Test/Ignition/Value", 220.0, "Зажигание!!! "));
    _cal1.Add("Butt_Drive_State_qqq", new CalibratNew("TEST/Low_Beam_Test/Butt_Drive_State/Value", 220.0, "!!! Кнопка устройства"));
    _jsonParams.Calibration.Add("Params1", new CalibrationParams()
    {
      PathFiles = "D:\\TestSystem\\Moto\\Strategies\\St0\\Params1.dcm",
      Parameter = _cal1
    });

    string output = JsonConvert.SerializeObject(_jsonParams);
    string _pathConfig = @"D:\TestSystem\Moto\Params.json";
    File.WriteAllText(_pathConfig, output);
    int kk = 1;
*/    
//    string _txt = File.ReadAllText(_pathConfig);
//    var _paramsMoto = JsonConvert.DeserializeObject<LoadJsonParams>(_txt);

    int k = 1;
    //    DateTime _dt0 = DateTime.Now;
    //    Thread.Sleep(2100); 
    //    DateTime _dt1 = DateTime.Now;
    //    TimeSpan duration = (DateTime.Now - _dt0);
    //    var xx = (DateTime.Now - _dt0).Seconds;
    /*
       "Path": {
        "Workspace": "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew",
        "Experiment": "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex",
        "Params0": "D:\\TestSystem\\Moto\\Strategies\\St0\\Params0.dcm",
        "Params1": "D:\\TestSystem\\Moto\\Strategies\\St0\\Params1.dcm"
      },

      "Task": {
        "VCU_DesInvMode": [ "TEST/Low_Beam_Test/VCU_DesInvMode", "Acquisition", "Test 001" ],
        "Low_Beam_Req": [ "TEST/Low_Beam_Test/Low_Beam_Req", "Acquisition" ],
        "Low_Beam_State": [ "TEST/Low_Beam_Test/Low_Beam_State", "Acquisition", "Test 1" ]
      },



     */
    /*
        //GlobalConfigLabCar _config = new GlobalConfigLabCar();
        //_config.PathLabCar.Add("Workspace", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew");
        //_config.PathLabCar.Add("Experiment", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex");
        //_config.LabCarTask.Add("VCU_DesInvMode", new LabCarTaskName()
        //                    {NameInLabCar = "TEST/Low_Beam_Test/VCU_DesInvMode", PathTask = "Acquisition", Comment = "Test 001" });
        //_config.LabCarTask.Add("Low_Beam_Req", new LabCarTaskName()
        //  { NameInLabCar = "TEST/Low_Beam_Test/Low_Beam_Req", PathTask = "Acquisition" });
        //_config.LabCarTask.Add("Low_Beam_State", new LabCarTaskName()
        //  { NameInLabCar = "TEST/Low_Beam_Test/Low_Beam_State", PathTask = "Acquisition", Comment = "Test 1" });

        //string output = JsonConvert.SerializeObject(_config);
        string _pathConfig = @"D:\TestSystem\Moto\Config.json";
    //    File.WriteAllText(_pathConfig, output);
        string _txt = File.ReadAllText(_pathConfig);
        var config = JsonConvert.DeserializeObject<GlobalConfigLabCar>(_txt);
    */


    //using (StreamWriter sw = new StreamWriter(_pathConfig))
    //using (JsonWriter writer = new JsonTextWriter(sw))
    //{
    //  serializer.Serialize(writer, product);
    //  // {"ExpiryDate":new Date(1230375600000),"Price":0}
    //}


    _container = ContainerManager.GetInstance();
    var _connect = _container.LabCar.Resolve<IConnectLabCar>();
    var _jsonConfig = _container.LabCar.Resolve<IStrategyDanJson>();
        Console.WriteLine("=========   Stratedy 1   =======");
        var _testIlab1 = _container.LabCar.Resolve<IStrategiesBasa>();
        _testIlab1.RunInit(@"D:\TestSystem\Moto\Strategies\St1");
        _testIlab1.RunTest();
        Console.WriteLine("=========   Stratedy 2   =======");
        var _testIlab2 = _container.LabCar.Resolve<IStrategiesBasa>();
        _testIlab2.RunInit(@"D:\TestSystem\Moto\Strategies\St2");
        _testIlab2.RunTest();

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
        Console.WriteLine("==== END === !!!! ");
  }

}