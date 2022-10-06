# import COM module for Windows

from DanConst0 import IniciaAll
from ContextLabCar import ContextLabCar

if __name__ == '__main__':
  print(" Load config ")
  x = IniciaAll()

  print(" Старт программы ")
  _contextLabCar = ContextLabCar(x["path"])


  _contextLabCar.ConnectLabCar()
  print("Загружаем файл с исходными данными")
  _contextLabCar.InicialStrateg(x)

  print("Запускаем стратегию")
  _contextLabCar.Run(x["name"])
  _contextLabCar.DisconnectLabCar()

  print(" Стоп программы !!! - все прошло хорошо")


'''
# from DanConst0 import inicialPath, inicialDanFestWert, inicialTask, inicialInput, inicialStrateg, inicialOut, IniciaAll

  # _path = inicialPath()
  # _danFestWert = inicialDanFestWert()
  # _taskDan = inicialTask()
  # _danin = inicialInput()
  # _danout = inicialOut()
  # _inicialStrateg = inicialStrateg()

  # _contextLabCar.InicialStrateg(name =_inicialStrateg[0],
  #                               path = _path,
  #                               festwertl =_danFestWert,
  #                               input = _danin,
  #                               output = _danout,
  #                               task = _taskDan,
  #                               strateg = _inicialStrateg[1])
'''
