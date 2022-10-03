from LabCarBasa import LabCarBasa
from LabCarModul.Core.CDan import *
from TransferControlDataToLabCar import TransferDataToLabCar
import time

class ContextLabCar(TransferDataToLabCar):
  def __init__(self, *args, **kwargs):
    TransferDataToLabCar.__init__(self, *args)
    pass

  def CreateFileParam(self, nameFile, param:dict):
    _nameFile = nameFile
    _param = param
  def InstalParam(self, d:dict, ls:list):
    for it in ls:
      self.Parameter[it] = self.SignalSources.CreateParameter(d[it])


  def UniTest(self, sc:list):
    _ls = sc
    if len(_ls) < 2:
      return

    _dGet=dict()
    _config = _ls[0]
    for it in range(1, len(_ls)):
      d = dict(_ls[it])
      print(d)
      k=1
      if 't' in d.keys():
        time.sleep(d['t'])

      if 'get' in d.keys():
        _get = list(d['get'])
        for it in _get:
          print(it)
          v = self.GetMeasurement(it)
          _dGet[it]=v

      if 'set' in d.keys():
        _set = dict(d['set'])
        for key,val in _set.items():
          print(key,val)
          self.SetDan(key, val)

      if 'rez' in d.keys():
        _bRez = True
        _rez = list(d['rez'])
        for it in _rez:
          print(it)
          _ls0 = it.strip()
          if '==' in _ls0:
            ss = _ls0.split('==')
            name=ss[0]
            _bRez = _bRez & (_dGet[name] == float(ss[1]))

    k=1
#    LabCarBasa.__init__(self, *args)
    # self.transferLabCar = TransferControlDataToLabCar.TransferDataToLabCar(self)
    # self.transferLabCar.InitContext(self)

