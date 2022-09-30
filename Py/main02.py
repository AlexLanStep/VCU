# import COM module for Windows
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
           k=11
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

  return  cdan

  # pprint.pprint(cdan.DanForModel)
  # print(cdan.DanForModel["Gain_Signal"]["s"])



if __name__ == '__main__':
  print(" Старт программы ")
  cdan, pp = inicialLabCar()
  _contextLabCar = ContextLabCar(pp.D, cdan)
  # _labCarBasa = LabCarBasa(pp.D, cdan)
  # _labCarBasa.ConnectLabCar()
#  _contextLabCar.ConnectLabCar()
  dParam = {}
  d0 = inicialDan0("TEST/Low_Beam_Test")

  _contextLabCar.CreateFileParam("Param1", d0)

  _contextLabCar.DisconnectLabCar()

'''
  # get value of measurement variable
  Measurement = SignalSources.CreateMeasurement(cdan.Get("Out1"), "Acquisition")
  time.sleep(3)  # wait some time until value is updated from the model execution target
  ValueObject = Measurement.GetValueObject
  Value = ValueObject.GetValue

  # set value of calibration variable (parameter)
  Parameter = SignalSources.CreateParameter(cdan.Get("Out1"))
  ValueObject = Parameter.GetValueObject
  ValueObject.SetValue(30.000)
  Parameter.SetValueObject(ValueObject)

  # download parameters directly from parameter file
  Experiment.CalibrationController.LoadParameters(pp.Get("ControlDan") +\\ParameterFile.dcm")
  # add parameter file to experiment explorer
  Experiment.AddFile(
    "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm")
  Experiment.ActivateFile(
    "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm",
    True)

  # create a new data logger
  Datalogger = Experiment.DataLoggers.CreateDatalogger("MyDataLogger")
  Datalogger.AddScalarRecordingSignal("TEST/Result", "")
  # get existing data logger
  Datalogger = Experiment.DataLoggers.GetDataloggerByName("MyDataLogger")
  Datalogger.StartTriggerPreTriggerTime = 5
  # this call is needed to apply all configuration setting (trigger, file settings)
  Datalogger.ApplyConfiguration
  Datalogger.Activate
  # start data logger for manual start trigger type
  Datalogger.Start
  time.sleep(10)  # wait
  # stop data logger for manual stop trigger type
  Datalogger.Stop
  time.sleep(3)  # wait some time until datalogger post processing is complete

  SignalGeneratorConfiguration = Experiment.SignalGeneratorConfiguration
  # create a signal generator based on signal description set (LCO use case)
  # see interface ISignalGeneratorWithSet in API reference
  SignalGenerator = SignalGeneratorConfiguration.SignalGenerators.CreateSignalGenerator(0, "MySignalGenerator")
  # or get an existing generator by name
  SignalGenerator = SignalGeneratorConfiguration.SignalGenerators.GetSignalGeneratorByName("MySignalGenerator")
  SignalGenerator.StartTimeInSeconds = 5
  # create signal description set by import of measure file or lcs file
  SignalGeneratorConfiguration.SignalDescriptionSets.Import(
    "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\SignalGenerator_Files\\TEST_Signal_generator.dat")
  # create a sine signal description
  SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.CreateSignalDescriptionSet(
    "MySignalDescription")
  SignalDescription = SignalDescriptionSet.CreateSignalDescription("My Sine Signal")
  SignalSegment = SignalDescription.CreateSegment(7)
  SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.GetSignalDescriptionSetByName(
    "MySignalDescription")
  SignalGenerator.SignalDescriptionSet = SignalDescriptionSet
  SignalGenerator.Device = "RTPC"
  SignalGenerator.Task = "Acquisition"
  SignalGenerator.Start
  # for application of the signal generator to your LABCAR ports, please refer to ISignalFlowManager



def smart_divide(func):
  def inner(a, b):
    print("I am going to divide", a, "and", b)
    if b == 0:
      print("Whoops! cannot divide")
      return

    return func(a, b)

  return inner


@smart_divide
def divide(a, b):
  return a / b


'''
