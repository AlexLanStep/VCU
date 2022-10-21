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

