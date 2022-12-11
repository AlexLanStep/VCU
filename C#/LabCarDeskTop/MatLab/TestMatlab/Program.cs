//  https://github.com/MariaKudryashova/mathdll/blob/main/SLMTest/SLMTest/Program.cs
//  https://habr.com/ru/post/661223/
// https://www.youtube.com/watch?v=5qDkr4GKwAw


using System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using LSMLib;


namespace SLMTest
{
  internal class Program
  {
    static void Main(string[] args)
    {
      //пример массивов, которые будем аппроксимировать
      double n = 3;
      double[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
      double[] y = { 3, 5, 4, 6, 7, 7, 4, 5, 6, 7, 8, 9, 6, 6, 7 };
      double[] wts = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

      //вызов нашей функции из библиотеки
      Console.WriteLine(" 0 --------");

      LSMLib.LSMLib test = new LSMLib.LSMLib();
      Console.WriteLine(" 1 --------");

      MWArray res = test.solve_lsm((MWArray)n, (MWNumericArray)x, (MWNumericArray)y, (MWNumericArray)wts);
      Console.WriteLine(" 2 --------");

      //обработка возвращаемых коэффициентов и перевод их в обычный список
      var array = (MWNumericArray)res;
      var dd = (double[,])array.ToArray(MWArrayComponent.Real);

      List<double> koeffs = new List<double>();
      for (int i = 0; i < dd.Length; i++)
      {
        koeffs.Add(dd[i, 0]);
      }
      Console.WriteLine("--------");
      Console.ReadKey();

    }
  }
}

/*
 
 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using LSMLib;



namespace SLMTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //пример массивов, которые будем аппроксимировать
            double n = 3;
            double[] x = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
            double[] y = {3,5,4,6,7,7,4,5,6,7,8,9,6,6,7};
            double[] wts = {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
                       
            //вызов нашей функции из библиотеки
            LSMLib.LSMLib test = new LSMLib.LSMLib();
            MWArray res = test.solve_lsm((MWArray)n, (MWNumericArray)x, (MWNumericArray)y, (MWNumericArray)wts);

            //обработка возвращаемых коэффициентов и перевод их в обычный список
            var array = (MWNumericArray)res; 
            var dd = (double[,])array.ToArray(MWArrayComponent.Real);

            List<double> koeffs = new List<double>();
            for (int i = 0; i < dd.Length; i++)
            {
                koeffs.Add(dd[i, 0]);
            }
            Console.ReadKey();

        }
    }
}
 
 */
