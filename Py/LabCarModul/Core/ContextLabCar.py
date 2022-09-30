from LabCarBasa import LabCarBasa
from LabCarModul.Core.CDan import *
from TransferControlDataToLabCar import TransferDataToLabCar


class ContextLabCar(TransferDataToLabCar):
  def __init__(self, *args, **kwargs):
    TransferDataToLabCar.__init__(self, *args)
    pass

  def CreateFileParam(self, nameFile, param:dict):
    _nameFile = nameFile
    _param = param

#    LabCarBasa.__init__(self, *args)
    # self.transferLabCar = TransferControlDataToLabCar.TransferDataToLabCar(self)
    # self.transferLabCar.InitContext(self)

