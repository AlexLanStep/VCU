import DanConst0


class CDan:
  def __init__(self, model):
    self.model = model
    self.DanForModel = dict()
    self.Dan = dict()
    self.TaskDan = dict()

  def AddFestWert(self, name, val, coment=""):
    self.DanForModel[name] = {"s": f"FESTWERT {self.model}/{name}/Value \n  WERT {str(val)} \nEND\n",
                              "v": val,
                              "c":coment}
    jj=1

  def AddFestWertModel(self, model, name, val, coment=""):
    self.DanForModel[name] = {"s": f"FESTWERT {model}/{name}/Value \n  WERT {str(val)} \nEND\n",
                              "v": val,
                              "c":coment}

  def ReplaceFestWert(self, name, val):
    _sd = self.DanForModel[name]
    self.DanForModel[name] = {"s": str(_sd['s']).replace(str(_sd['v']), str(val)), 'v': val, "c":_sd['c']}

  def ConvertFestWertTxt(self, pathfiles):
    s = []
    _ = [s.append(item['s']) for item in self.DanForModel.values() ]
    self.WriteTxt(pathfiles, s)

  def WriteTxt(self, pathfiles:str, ls:list):
    with open(pathfiles, 'w') as filehandle:
        for listitem in ls:
            filehandle.write('%s\n' % listitem)

  def DeleteFestWertName(self, name):
    if name in self.DanForModel.keys():
      self.DanForModel.pop(name)

  def Add(self, name, representation):
    self.Dan[name] = representation

  def AddTask(self, name, signal, task, comment=""):
    self.TaskDan[name] = {"sig": signal, 'task':task, 'com':comment}

  def GetTask(self, name):
    return self.TaskDan[name]

  def GetFestWert(self, name, key=None):
    try:
      _s = self.DanForModel[name]
      if (isinstance(_s, str)):
        return _s
      elif (isinstance(_s, dict)):
        match key:
          case None:
            return _s
          case "s":
            return _s["s"]
          case "v":
            return _s["v"]
          case "c":
            return _s["c"]
          case _:
            return _s
      else:
        return None
    except:
      return None

  def AddNameParams(self, name, coment=""):
    self.Dan[name] = {"s": f"{self.model}/{name}/Value",
                              "c":coment}
    jj=1

  def AddNameParamsModel(self, name, model, coment=""):
    self.Dan[name] = {"s": f"{model}/{name}/Value",
                              "c":coment}
    jj=1
