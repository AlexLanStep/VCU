import threading as th

from NegValException import NegValException
import time


# class AStrateg(th.Thread):
class AStrateg():
  def __init__(self, *args, **kwargs):
    # th.Thread.__init__(self)

    self._pathLabCar = None
    self.Application = None
    self.ExperimentEnvironment = None
    self.Workspace = None
    self.Experiment = None
    self.SignalSources = None

    # self = args[0]

    self._pathLabCar = args[0]._pathLabCar
    self.Application = args[0].Application
    self.ExperimentEnvironment = args[0].ExperimentEnvironment
    self.Workspace = args[0].Workspace
    self.Experiment = args[0].Experiment
    self.SignalSources = args[0].SignalSources

    self.Measurement = {}
    self.Parameter = {}

    self._festwertl = args[1].get("festwertl", None)
    self._input = args[1].get("input", None)
    self._task = args[1].get("task", None)
    self._output = args[1].get("output", None)
    self._strateg = args[1].get("strateg", None)
    self._path = args[1].get("path", None)

    # self._festwertl = kwargs.get("festwertl", None)
    # self._input = kwargs.get("input", None)
    # self._task = kwargs.get("task", None)
    # self._output = kwargs.get("output", None)
    # self._strateg = kwargs.get("strateg", None)
    # self._path = kwargs.get("path", None)

    if (self._festwertl == None) | (self._input == None) | \
        (self._task == None) | (self._output == None) | \
        (self._strateg == None) | (self._path == None):
      NegValException(" No input in AStrateg ", -41)


  def InitFestWertlDan(self, path):
    _path = self._pathLabCar[path] # self._festwertl[path]
    try:
      # download parameters directly from parameter file
      self.Experiment.CalibrationController.LoadParameters(_path)
    except:
      print(f" - error Experiment.CalibrationController.LoadParameters  \n - File already exists in LabCar {_path}")

    try:
      # add parameter file to experiment explorer
      self.Experiment.AddFile(_path)
    except:
      print(f" - error Experiment.AddFile - \n File already exists in LabCar {_path}")

  def ActivParametrs(self, param, b=True):
    _path = self._pathLabCar[param]
    self.Experiment.ActivateFile(_path, b)

  def InitTask(self, ddan: dict):
    for _task in ddan.keys():
      dan = ddan[_task]
      # get value of measurement variable           # [name] = {"sig": signal, 'task':task,
      self.Measurement[_task] = self.SignalSources.CreateMeasurement(dan["sig"], dan["task"])

  def GetMeasurement(self, name):
    Value = -99999999
    if name in self.Measurement.keys():
      ValueObject = self.Measurement[name].GetValueObject
      Value = ValueObject.GetValue
    else:
      print(f" !! Error in Measurement - not name {name}")
    return Value

  def InicialParam(self, name, path ):
    self.Parameter[name] = self.SignalSources.CreateParameter(path)
    # self.Parameter[name] = self.SignalSources.CreateParameter("TEST/Control_Signal/Value")

  def InicialParamDict(self, arg ):
    for key in arg[0]:
      val = arg[1][key]['s']
      self.InicialParam(key, val )

  def SetDan(self, name, value):
    # self.Parameter[name] = self.SignalSources.CreateParameter("TEST/Control_Signal/Value")
    ValueObject = self.Parameter[name].GetValueObject
    ValueObject.SetValue(value)
    self.Parameter[name].SetValueObject(ValueObject)

  def Run(self):
    _ls = self._strateg
    if len(_ls) < 2:
      return
    _bRez = True
    _dGet = dict()
    _dconfig = dict(_ls[0])

    print("  -  Load data from files")
    if "loadfile" in _dconfig.keys():
      for it in _dconfig["loadfile"]:
        self.InitFestWertlDan(it)

    print("  -  Instal TASK ")
    self.InitTask(self._task)

    print("  -  connect input params ")
    self.InicialParamDict(self._input)
    print("  -  connect output params ")
    self.InicialParamDict(self._output)

    if "activfile" in _dconfig.keys():
      for it in _dconfig["activfile"]:
        self.ActivParametrs(it)

    print("  -  Start TEST  ")
    for it in range(1, len(_ls)):
      d = dict(_ls[it])
      print(d)
      k = 1
      # if 't' in d.keys():
      #   time.sleep(d['t'])

      _ = time.sleep(d['t']) if 't' in d.keys() else 1
      # if "loadfile" in d.keys():
      #   for it in _dconfig["loadfile"]:
      #     self.InitFestWertlDan(it)

#      _ = [self.InitFestWertlDan(it) for it in _dconfig["loadfile"] if "loadfile" in d.keys()]

      # if "activfile" in d.keys():
      #   for it in d["activfile"]:
      #     self.ActivParametrs(it)
#      _ = [self.ActivParametrs(it) for it in d["activfile"] if "activfile" in d.keys()]

      if 'get' in d.keys():
        for it in list(d['get']):
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
          # print(it)
          _ls0 = it.strip()
          if '==' in _ls0:
            ss = _ls0.split('==')
            name = ss[0]
            _b = _dGet[name] == float(ss[1])
            print(f" Target - {it} real = {_dGet[name]}")
            if _b:
              pass
            else:
              print(f"  ERROR  {name}")
            _bRez = _bRez & _b
        if _bRez:
          print(" Test GUD  !!!!")
          k=1
        else:
          print(" - ERROR - Test  !!!!")
          k=1

    k = 1

  def InitControlDanOld(self, ddan: dict):
    for item in ddan.keys():
      _path = self._pathLabCar[item]
      dan = ddan[item]
      try:
        # download parameters directly from parameter file
        self.Experiment.CalibrationController.LoadParameters(_path)
      except:
        print(f" - error Experiment.CalibrationController.LoadParameters  \n - File already exists in LabCar {_path}")

      try:
        # add parameter file to experiment explorer
        self.Experiment.AddFile(_path)
      except:
        print(f" - error Experiment.AddFile - \n File already exists in LabCar {_path}")
