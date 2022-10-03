# import COM module for Windows
import re
import win32com.client
import pprint
import time
import os

import CDan
from ContextLabCar import ContextLabCar
from LabCarBasa import LabCarBasa
from LabCarModul.Core.ManagerFile import ManagerFiles
from LabCarModul.Core.PathProgect import CPath
from LabCarModul.Core import *
from LabCarModul.Core.CDan import *


def inicialLabCar():
  cdan = CDan(DanConst0.ModelName)

  pp = CPath(DanConst0.PathBasa)
  pp.AddFileTest("PathWorkspace", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew")
  pp.AddFileTest("PathExperiment", "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex")
  pp.AddDirTest("ControlDan", "D:\\ControlDan") #+\\ParameterFile.dcm"
  pp.Add("Param1", pp.Get("ControlDan") + "\\ParameterFile1.dcm")

  pp.__str__(True)
  mFile = ManagerFiles(pp.D)
  # _x1 = cdan.Get("Out1")  _x2 = cdan.Get("Gain_Signal")  _x3 = cdan.Get("Gain_Signal", 's') _x4 = cdan.Get("Gain_Signal", 'v')  _x5 = cdan.Get("Gain_Signal1", 'vs')

  return cdan, pp

def inicialDan0(*dan):
  match dan.__len__():
    case 0:
         cdan = CDan("")
    case 1:
         if isinstance(dan[0], str):
           cdan = CDan(dan[0])
         else:
           cdan = dan[0]
    case 2:
        cdan = dan[0]
        cdan.model =  dan[1]

  cdan.AddFestWert("BatteryIsOn", 0.0, "Кнопка баттареи")
  cdan.AddFestWert("Ignition", 0.0, "Зажигание")
  cdan.AddFestWert("Butt_Drive_State", 0.0, "Кнопка устройства")
  # cdan.ReplaceFestWert("BatteryIsOn", 1999.0)
  cdan.ConvertFestWertTxt(CPath.D["Param1"])

#  cdan.AddTask(self, name, signal, task, comment=""):
  cdan.AddTask("VCU_DesInvMode", "TEST/Low_Beam_Test/VCU_DesInvMode/Value", "Task001", "" )   # , "Acquisition"
  cdan.AddTask("Low_Beam_Req", "TEST/Low_Beam_Test/Low_Beam_Req/Value", "Task01")             # , "Acquisition"
  cdan.AddTask("Low_Beam_State", "TEST/Low_Beam_Test/Low_Beam_State/Value", "Task01")         # , "Acquisition"

  lsParams = ["BatteryIsOn", "Ignition", "Butt_Drive_State"]
  for it in lsParams:
    cdan.AddNameParams(it)

  cdan.AddNameParamsModel("BatteryIsOn1", "xxTEST/xxx/Low_Beam_Test", "twst param")

  return  cdan, lsParams

def InicialScenario0():
  _wait=0.2
  ls=[("sampl=0.1"),
      {'t':_wait,
            'set':{"Butt_Drive_State":1.0, "BatteryIsOn":1.0}},
      {'t':_wait,
            'get':["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
            'set':{"Ignition":1.0, "BatteryIsOn":0.0 }},
      {'t':_wait,
            'get':["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
            'set':{"Butt_Drive_State":0.0, "BatteryIsOn":1.0}},
      {'t':_wait,
            'get':["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
            'set':{"Butt_Drive_State":1.0, }},
      {'t':_wait,
            'get':["BatteryIsOn", "Low_Beam_Req", "Butt_Drive_State"],
            'rez':["BatteryIsOn==1.0", "Low_Beam_Req==1.0", "Butt_Drive_State==1.0"]},
     ]
  return ls

if __name__ == '__main__':

  # pEquals = re.compile('==')

  print(" Старт программы ")


  cdan, pp = inicialLabCar()
  _contextLabCar = ContextLabCar(pp.D, cdan)

  Scenario0 = InicialScenario0()
  pprint.pp(Scenario0)

  d0, lsParams = inicialDan0("TEST/Low_Beam_Test")

  _contextLabCar.InstalParam(d0.Dan, lsParams)
  _contextLabCar.UniTest(Scenario0)


#  _contextLabCar.transferLabCar.IniIControlDan({"Param1":d0.DanForModel})
#  _contextLabCar.InitTask(d0.TaskDan)


  # _labCarBasa = LabCarBasa(pp.D, cdan)
  # _labCarBasa.ConnectLabCar()
#  _contextLabCar.ConnectLabCar()

  _contextLabCar.CreateFileParam("Param1", d0)

#  _contextLabCar.DisconnectLabCar()

