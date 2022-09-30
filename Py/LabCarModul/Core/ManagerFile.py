import os

class ManagerFiles:
  def __init__(self, path:dict):
    self.path=path

  def IsFile(self, chapter, file):
    _path = self.path[chapter]+"\\"+file
    return os.path.isfile(_path)
