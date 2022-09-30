import win32com.client
import pprint
import time
import os

from LabCarModul.Core.ManagerFile import ManagerFiles
from LabCarModul.Core.PathProgect import CPath
from LabCarModul.Core import *
from LabCarModul.Core.CDan import *
from NegValException import NegValException


class LabCarBasa:
  def __init__(self, *args):
    if args.__len__()<1:
      NegValException(' Error not Path ', -3)

    self._pathLabCar = args[0]
    self.Application = None
    self.ExperimentEnvironment = None
    self.Workspace = None
    self.Experiment = None
    self.SignalSources = None

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

  def TestBasePeremen(self):

    if self.Application == None:
      NegValException(" Error value Application ", -30)  # NegValException(" Не загружена переменная Experiment ", -30)

    if self.ExperimentEnvironment == None:
      NegValException(" Error value ExperimentEnvironment ", -31)

    if self.Workspace == None:
      NegValException(" Error value Workspace ", -32)

    if self.Experiment == None:
      NegValException(" Error value Experiment ", -33)

    if self.SignalSources == None:
      NegValException(" Error value SignalSources ", -34)

