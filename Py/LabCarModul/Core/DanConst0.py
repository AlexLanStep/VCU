from LabCarModul.Core.CDan import CDan
from LabCarModul.Core.PathProgect import CPath

PathBasa = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
ModelName = "TEST/Low_Beam_Test"


def inicialPath():
  pp = CPath(PathBasa)
  pp.AddFileTest("PathWorkspace", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew")
  pp.AddFileTest("PathExperiment",
                 "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex")

  pp.AddDirTestOrInc("Str0ControlDan", "D:\\ControlDan\\Str0")  # +\\ParameterFile.dcm"
  pp.Add("Str0Param0", "D:\\ControlDan\\Str0\\Param0.dcm")
  # pp.__str__(True)
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
  cdan.AddTask("VCU_DesInvMode", "TEST/Low_Beam_Test/VCU_DesInvMode", "Acquisition")  # , "Acquisition"
  cdan.AddTask("Low_Beam_Req", "TEST/Low_Beam_Test/Low_Beam_Req", "Acquisition")
  cdan.AddTask("Low_Beam_State", "TEST/Low_Beam_Test/Low_Beam_State", "Acquisition")
  return cdan.TaskDan


def inicialInput():
  cdan = CDan(ModelName)
  lsParams = ["BatteryIsOn", "Ignition", "Butt_Drive_State"]
  _ = [cdan.AddNameParamsIn(it) for it in lsParams]

  # for it in lsParams:
  #   cdan.AddNameParamsIn(it)
  # cdan.AddNameParamsModelIm("BatteryIsOn1", "xxTEST/xxx/Low_Beam_Test", "twst param")
  return (lsParams, cdan.Dan)

def inicialOut():
  cdan = CDan(ModelName)
  lsParams = ["VCU_DesInvMode", "Low_Beam_Req", "Low_Beam_State"]
  _ = [cdan.AddNameParamsOut(it) for it in  lsParams]

  # for it in lsParams:
  #   cdan.AddNameParamsOut(it)
  #cdan.AddNameParamsModelOut("BatteryIsOn1", "xxTEST/xxx/Low_Beam_Test", "twst param") PRIMMER!!!!!!
  return (lsParams, cdan.Dan)

def inicialStrateg():
  name = "Swet"
  _wait0 = 1.0
  _wait1 = 1.5

  ls = [{"sampl":0.2, "loadfile":["Str0Param0"], "activfile":["Str0Param0"], "maxwait": 10},
        {'t': _wait0,
         'set': {"Butt_Drive_State": 1.0, "BatteryIsOn": 1.0, "Ignition": 1.0}},
        {'t': _wait1,
         'get': ["VCU_DesInvMode", "Low_Beam_Req", "Low_Beam_State"],
         'rez': ["VCU_DesInvMode==1.0", "Low_Beam_Req==1.0", "Low_Beam_State==1.0"]},
        ]
  return  (name, ls)

def IniciaAll():
  _inicialStrateg = inicialStrateg()

  rez =  {"name": _inicialStrateg[0],
          "path": inicialPath(),
          "festwertl": inicialDanFestWert(),
          "task": inicialTask(),
          "input": inicialInput(),
          "output": inicialOut(),
          "strateg": _inicialStrateg[1]
         }

  return rez

'''

def inicialStrateg():
  name = "Swet"
  _wait0 = 1.0
  _wait1 = 1.5
  ls = [{"sampl":0.1, "loadfile":["Str0Param0"], "activfile":["Str0Param0"]},
        {'t': _wait0,
         'set': {"Butt_Drive_State": 1.0, "BatteryIsOn": 1.0}},
        {'t': _wait0,
         'get': ["BatteryIsOn",  "Butt_Drive_State"],
         'set': {"Ignition": 1.0, "BatteryIsOn": 0.0}},
        {'t': _wait0,
         'get': ["BatteryIsOn",  "Butt_Drive_State"],
         'set': {"Butt_Drive_State": 0.0, "BatteryIsOn": 1.0}},
        {'t': _wait0,
         'get': ["BatteryIsOn",  "Butt_Drive_State"],
         'set': {"Butt_Drive_State": 1.0, }},
        {'t': _wait1,
         'get': ["VCU_DesInvMode", "Low_Beam_Req", "Low_Beam_State"],
         'rez': ["VCU_DesInvMode==1.0", "Low_Beam_Req==1.0", "Low_Beam_State==1.0"]},
        ]
  return  (name, ls)

'''
