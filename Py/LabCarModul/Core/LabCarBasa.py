import win32com.client
import pprint
import time
import os

from LabCarModul.Core.ManagerFile import ManagerFiles
from LabCarModul.Core.PathProgect import CPath
from LabCarModul.Core import *
from LabCarModul.Core.CDan import *


class LabCarBasa:
  def __init__(self, path: dict, dan: CDan):
    self._pathLabCar = path
    self._danLabCar = dan

  def ConnectLabCar(self):

    _s = self._pathLabCar["PathWorkspace"]
    print(f"Запускаем LabCar -  {_s} ")
    self.Application = win32com.client.dynamic.Dispatch("ExperimentEnvironment.Application")  # startup
    self.ExperimentEnvironment = self.Application.Scripting  # get root object
    self.Workspace = self.ExperimentEnvironment.OpenWorkspace(_s)  # open workspace

    _e = self._pathLabCar["PathExperiment"]
    print("Грузим эксперемент  {e}")
    self.Experiment = self.Workspace.OpenExperiment(_e)  # open experiment
    self.SignalSources = self.Experiment.SignalSources  # get all signal sources

    if self.SignalSources.HardwareDetected():
      print(" LabCar запущен")
    else:
      print(" Запускаем LabCar ")
      self.SignalSources.Download  # download the model to the target

    self.SignalSources.StartSimulation  # start simulation on the target
    self.SignalSources.StartMeasurement  # start measurement

  #  Выключение  Labcara
  def DisconnectLabCar(self):
    self.SignalSources.StopMeasurement  # stop measurement
    self.SignalSources.StopSimulation  # stop the simulation on the target
    self.SignalSources.Disconnect  # disconnect the target

    self.Experiment.Close  # close the Experiment
    self.Workspace.Close  # close the workspace
