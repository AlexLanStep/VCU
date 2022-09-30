def smart_divide(func):
    def inner(a, b):
        print("I am going to divide", a, "and", b)
        if b == 0:
            print("Whoops! cannot divide")
            return

        return func(a, b)
    return inner


@smart_divide
def divide(a, b):
  return a/b

if __name__ == '__main__':
  say_whee = divide(2,5)
  print(say_whee)
  say_whee = divide(2,0)
  print(say_whee)
  k=1
