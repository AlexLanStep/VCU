# import COM module for Windows

from DanConst0 import inicialPath, inicialDanFestWert, inicialTask, inicialInput, inicialStrateg, inicialOut
from ContextLabCar import ContextLabCar


if __name__ == '__main__':
  _path = inicialPath()
  _danFestWert = inicialDanFestWert()
  _taskDan = inicialTask()
  _danin = inicialInput()
  _danout = inicialOut()
  _inicialStrateg = inicialStrateg()
  print(" Старт программы ")

  _contextLabCar = ContextLabCar(_path)
  _contextLabCar.InicialStrateg(name=_inicialStrateg[0],
                                path=_path,
                                festwertl=_danFestWert,
                                input= _danin,
                                task = _taskDan,
                                strateg =_inicialStrateg[1])


  _contextLabCar.ConnectLabCar()
  _contextLabCar.Run(_inicialStrateg[0])
  _contextLabCar.DisconnectLabCar()

  print(" Стоп программы !!! - все прошло хорошо")
