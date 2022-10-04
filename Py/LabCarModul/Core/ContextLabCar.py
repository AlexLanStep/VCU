from AStrateg import AStrateg
from LabCarBasa import LabCarBasa
from LabCarModul.Core.CDan import *
from TransferControlDataToLabCar import TransferDataToLabCar
import time


# class TransferDataToLabCar(LabCarBasa):
#   def __init__(self, *args, **kwargs):
#     LabCarBasa.__init__(self, *args)


class ContextLabCar(LabCarBasa):
  def __init__(self, *args, **kwargs):  # TransferDataToLabCar.__init__(self, *args)
    LabCarBasa.__init__(self, *args)
    self.DStrateg = {}

  def InicialStrateg(self, *args, **kwargs):
    _name = kwargs.get("name", "strateg0")
    self.DStrateg[_name] = AStrateg(self, **kwargs)
    kk = 1

  def Run(self, name):
    self.DStrateg[name].Run()
    k=1

  def UniTest(self, sc: list):
    _ls = sc
    if len(_ls) < 2:
      return
    _bRez = True
    _dGet = dict()
    _config = _ls[0]
    for it in range(1, len(_ls)):
      d = dict(_ls[it])
      print(d)
      k = 1
      if 't' in d.keys():
        time.sleep(d['t'])

      if 'get' in d.keys():
        _get = list(d['get'])
        for it in _get:
          print(it)
          v = self.GetMeasurement(it)
          _dGet[it] = v

      if 'set' in d.keys():
        _set = dict(d['set'])
        for key, val in _set.items():
          print(key, val)
          self.SetDan(key, val)

      if 'rez' in d.keys():
        _bRez = True
        _rez = list(d['rez'])
        for it in _rez:
          print(it)
          _ls0 = it.strip()
          if '==' in _ls0:
            ss = _ls0.split('==')
            name = ss[0]
            _b = _dGet[name] == float(ss[1])
            if _b:
              pass
            else:
              print(f"  ERROR  {name}")
            _bRez = _bRez & _b
    if _bRez:
      print(" Test GUD  !!!!")
    else:
      print(" - ERROR - Test  !!!!")

    k = 1
#    LabCarBasa.__init__(self, *args)
# self.transferLabCar = TransferControlDataToLabCar.TransferDataToLabCar(self)
# self.transferLabCar.InitContext(self)
