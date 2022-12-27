using System;
using System.IO;
using csmatio.io;
using csmatio.types;

//using MathWorks.MATLAB.NET.Arrays;
//using MathWorks.MATLAB.NET.Arrays.native;
//using MathWorks.MATLAB.NET.Utility;
//using MWStructArray = MathWorks.MATLAB.NET.Arrays.native.MWStructArray;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main ( string[] args )
        {
            string filePath = "";
//            filePath = @"D:\TestSystem\Moto\VariableData\mM2.mat";
            filePath = @"D:\TestSystem\Moto\VariableData\xm22.mat";
            if ( args.Length == 1 )
                filePath = args[0];
            else
            {
                /* Раскомментировать если нужен ввод пути к файлу.
                 * Еесли не раскомментировать, то будет браться файл
                 * SFB__vector__SFB.mat.b в папке с приложением */

                // filePath = QueryUserFilePath();

                if ( !File.Exists( filePath ) )
                    filePath = QueryDefaultFilePath();
            }

            if ( !File.Exists( filePath ) )
            {
                Console.WriteLine( "Файл \"{0}\" не найден!", filePath );
                return;
            }

            Console.WriteLine( "Путь к файлу: " + filePath );
            Console.WriteLine();

            try
            {
                MatFileReader matReader = new MatFileReader( filePath );

        var xx0 = matReader.MatFileHeader;
        var xx1 = matReader.Content;
        var xx2 = matReader.Content.Keys;
        var xx3 = matReader.Content.Values;
        var xx4 = matReader.Content.Count;
        var xx5 = matReader.Data;
        var xx6 = matReader.Data.Count;
        var xx7 = matReader.GetMLArray("mM1");
        var _d01 = xx7.Dimensions[1];
        var _d0 = matReader.Content["V1"];
        var _d1 = matReader.Content["V0"];

        //var array = (MWNumericArray)xx7;
        MLDouble arr1 = (MLDouble)_d0;
//        var dblArr11 = arr1.GetArray();
        double[][] dblArr11 = arr1.GetArray();
        var __d01 = dblArr11[0];
        var __d02 = (dblArr11[0])[0];

        foreach ( MLArray mlArray in matReader.Data )
                {
                    // Выведет полную информацию о массиве
                    Console.WriteLine( "Тип: " + mlArray.GetType() );
                    Console.WriteLine( mlArray.ContentToString() );

                    // Читаем информацию для файла SFB__vector__SFB.mat.b
                    // а именно структуру которая на скриншоте:
                    // http://www.cyberforum.ru/attachments/294541d1374769670
                    if ( mlArray.Name == "unnamed1CopyCopyCopy" )
                    {
                        /* Элемент unnamed1CopyCopyCopy представляет из себя 
                         * двумерный массив типа double, конвертируем к структуре
                         * указанного типа. Все типы можно посмотреть в папке
                         * CSMatIO\src\types библиотеки CSMatIO */
                        MLDouble arr = (MLDouble)mlArray;
                        // Получаем значения массива из типа MLDouble - double[][]
                        double[][] dblArr = arr.GetArray();

                        foreach ( double[] line in dblArr )
                        {
                            foreach ( double value in line )
                            {
                                Console.Write( value + "\t" );
                            }

                            Console.WriteLine();
                        }
                    }
                }
            }
            catch ( Exception ex )
            {
                Console.WriteLine( "Ошибка времени выполнения: " );
                Console.WriteLine( ex );
            }

            Console.WriteLine( "Нажмите любую клавишу для завершения..." );
            Console.ReadKey();
        }

        static string QueryUserFilePath ( )
        {
            Console.WriteLine( "Введите путь к файлу: " );
            return Console.ReadLine();
        }

        /// <returns>Возвращает путь к файлу "SFB__vector__SFB.mat.b"</returns>
        static string QueryDefaultFilePath ( )
        {
            return Path.Combine( AppDomain.CurrentDomain.BaseDirectory, "SFB__vector__SFB.mat.b" );
        }
    }
}
