import sys

class NegValException(Exception):
  def __init__(self, st: str, num=None):
    if num is None:
      print(st)
      sys.exit(-1)
    else:
      print(f" - {st}  c номером ошибки {num}")
      sys.exit(num)
