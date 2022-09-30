#
# Transfer control data no LabCar
# - пердаем управляющие данные в LabCar
# -- контрольные значения
# -- Task
# -- контролируемые значения
from LabCarBasa import LabCarBasa
from NegValException import NegValException

class TransferDataToLabCar(LabCarBasa):
  def __init__(self, *args, **kwargs):
    LabCarBasa.__init__(self, *args)
    self.Measurement = {}


  def InitControlDan(self, ddan: dict):
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

  def ActivParametrs(self, param, b=True):
    self.Experiment.ActivateFile(self._pathLabCar[param], b)

  def InitTask(self, ddan: dict):
    for _task in ddan.keys():
      dan = ddan[_task]
      # get value of measurement variable           # [name] = {"sig": signal, 'task':task,
      self.Measurement[_task] = self.SignalSources.CreateMeasurement(dan["sig"], dan["task"])

  def GetMeasurement(self, name):
    ValueObject = self.Measurement[name].GetValueObject
    Value = ValueObject.GetValue
    return Value

    #
    # time.sleep(3) # wait some time until value is updated from the model execution target
    # ValueObject = Measurement.GetValueObject
    # Value = ValueObject.GetValue

