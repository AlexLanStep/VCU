from LabCarModul.Core.CDan import CDan
from LabCarModul.Core.PathProgect import CPath

PathBasa = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
ModelName = "TEST/Low_Beam_Test"


def inicialPath():
  pp = CPath(PathBasa)
  pp.AddFileTest("PathWorkspace", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew")
  pp.AddFileTest("PathExperiment",
                 "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex")
  pp.AddDirTest("Str0ControlDan", "D:\\ControlDan\\Str0")  # +\\ParameterFile.dcm"
  pp.Add("Str0Param0", pp.Get("Str0ControlDan") + "\\Str0Param0.dcm")
  pp.__str__(True)
  return pp.D


def inicialDanFestWert():
  nameFile="Str0Param0"
  cdan = CDan(ModelName)
  cdan.AddFestWert("BatteryIsOn", 0.0, "Кнопка баттареи")
  cdan.AddFestWert("Ignition", 0.0, "Зажигание")
  cdan.AddFestWert("Butt_Drive_State", 0.0, "Кнопка устройства")
  # cdan.ReplaceFestWert("BatteryIsOn", 1999.0)
  cdan.ConvertFestWertTxt(CPath.D[nameFile])
  return {nameFile: cdan.DanForModel}


def inicialTask():
  cdan = CDan()
  cdan.AddTask("VCU_DesInvMode", "TEST/Low_Beam_Test/VCU_DesInvMode/Value", "Task001", "")  # , "Acquisition"
  cdan.AddTask("Low_Beam_Req", "TEST/Low_Beam_Test/Low_Beam_Req/Value", "Task01")  # , "Acquisition"
  cdan.AddTask("Low_Beam_State", "TEST/Low_Beam_Test/Low_Beam_State/Value", "Task01")  # , "Acquisition"
  return cdan.TaskDan


def inicialInput():
  cdan = CDan(ModelName)
  lsParams = ["BatteryIsOn", "Ignition", "Butt_Drive_State"]
  for it in lsParams:
    cdan.AddNameParams(it)
  cdan.AddNameParamsModel("BatteryIsOn1", "xxTEST/xxx/Low_Beam_Test", "twst param")
  return (lsParams, cdan.Dan)

def inicialOut():
  cdan = CDan(ModelName)
  lsParams = ["BatteryIsOn", "Ignition", "Butt_Drive_State"]
  for it in lsParams:
    cdan.AddNameParams(it)
  cdan.AddNameParamsModel("BatteryIsOn1", "xxTEST/xxx/Low_Beam_Test", "twst param")
  return (lsParams, cdan.Dan)

def inicialStrateg():
  name = "Swet"
  _wait = 0.2
  ls = [{"sampl":0.1, "loadfile":["Str0Param0"], "activfile":["Str0Param0"]},
        {'t': _wait,
         'set': {"Butt_Drive_State": 1.0, "BatteryIsOn": 1.0}},
        {'t': _wait,
         'get': ["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
         'set': {"Ignition": 1.0, "BatteryIsOn": 0.0}},
        {'t': _wait,
         'get': ["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
         'set': {"Butt_Drive_State": 0.0, "BatteryIsOn": 1.0}},
        {'t': _wait,
         'get': ["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
         'set': {"Butt_Drive_State": 1.0, }},
        {'t': _wait,
         'get': ["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
         'rez': ["BatteryIsOn==1.0", "Low_Beam_Req==1.0", "Butt_Drive_State==1.0"]},
        ]
  return  (name, ls)

