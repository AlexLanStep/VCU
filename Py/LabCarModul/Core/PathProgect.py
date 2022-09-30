
import os

from NegValException import NegValException
from .ManagerFile import *
class CPath:
  BasaPath, LabcarProject, Workspace, Experiment = "", "", "", ""
  ParameterFile, SignalGeneratorData, LogFiles = "", "", ""
  D = dict()

  def __init__(cls, path):
    CPath.BasaPath = path  # "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
    CPath.LabcarProject = CPath.BasaPath
    CPath.Workspace = CPath.BasaPath + "\\AUTOMATION"
    CPath.Experiment = CPath.BasaPath + "\\Experiments\\DefaultExp"
    CPath.ParameterFile = CPath.Experiment + "\\Parameter_Files"
    CPath.SignalGeneratorData = CPath.Experiment + "\\SignalGenerator_Files"
    CPath.LogFiles = CPath.Experiment

    CPath.D["BasaPath"] = CPath.BasaPath
    CPath.D["LabcarProject"] = CPath.LabcarProject
    CPath.D["Workspace"] = CPath.Workspace
    CPath.D["Experiment"] = CPath.Experiment
    CPath.D["ParameterFile"] = CPath.ParameterFile
    CPath.D["SignalGeneratorData"] = CPath.SignalGeneratorData
    CPath.D["LogFiles"] = CPath.LogFiles

  def Add(cls, name, path):
    CPath.D[name] = path

  def __str__(cls, isprint=False):
    if not isprint:
      return

    _keys = CPath.D.keys()
    for item in _keys:
      print(f" {item} :  {CPath.D[item]} ")

  def Get(cls,name):
    return CPath.D[name] if name in CPath.D.keys() else None

  def AddFileTest(self, name, path):
    if os.path.isfile(path):
          CPath.D[name] = path
    else:
      NegValException(f"Не правильный путь к файлу {path}")

  def AddDirTest(self, name, path):
    if os.path.isdir(path):
          CPath.D[name] = path
    else:
      NegValException(f"Не такого каталога {path}", -20)

'''

  def TestIsFile(func):
    def inner(s0, s1):
      print(s0, s1)
      _grup = CPath.Get(s0)
      if _grup is None:
        return None

      _file = _grup + "\\" + s1
      return _file if os.path.isfile(_file) else None

    return inner

  @TestIsFile
  def TestFileFunc(grup, nameFile):
    return None



class CPath:
  BasaPath, LabcarProject, Workspace, Experiment = "", "", "", ""
  ParameterFile, SignalGeneratorData, LogFiles = "", "", ""
  D = dict()

  def __init__(cls, path):
    cls.BasaPath = path  # "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
    cls.LabcarProject = cls.BasaPath
    cls.Workspace = cls.BasaPath + "\\AUTOMATION"
    cls.Experiment = cls.BasaPath + "\\Experiments\\DefaultExp"
    cls.ParameterFile = cls.Experiment + "\\Parameter_Files"
    cls.SignalGeneratorData = cls.Experiment + "\\SignalGenerator_Files"
    cls.LogFiles = cls.Experiment

    cls.D["BasaPath"] = cls.BasaPath
    cls.D["LabcarProject"] = cls.LabcarProject
    cls.D["Workspace"] = cls.Workspace
    cls.D["Experiment"] = cls.Experiment
    cls.D["ParameterFile"] = cls.ParameterFile
    cls.D["SignalGeneratorData"] = cls.SignalGeneratorData
    cls.D["LogFiles"] = cls.LogFiles

  def __str__(cls, isprint=False):
    if not isprint:
      return

    _keys = cls.D.keys()
    for item in _keys:
      print(f" {item} :  {cls.D[item]} ")








BasaPath =  "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
Path_LabcarProject = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
Path_Workspace = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION"
Path_Experiment =  "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp"
Path_ParameterFile = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files"
Path_SignalGeneratorData = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\SignalGenerator_Files"
Path_LogFiles = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp"

'''
