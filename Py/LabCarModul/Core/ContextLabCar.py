from LabCarBasa import LabCarBasa
from LabCarModul.Core.CDan import *

class ContextLabCar(LabCarBasa):
  def __init__(self, path: dict, dan: CDan):
    LabCarBasa.__init__(self, path, dan)

  def CreateFileParam(self, nameFile, param:dict):
    _nameFile = nameFile
    _param = param


