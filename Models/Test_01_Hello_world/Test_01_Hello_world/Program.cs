using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.TestParameter;

namespace Test_HelloWorld // Note: actual namespace depends on the project name.
{
    internal class MyTest01:Etas.Eas.Atcl.Interfaces.TestCase
    {
        public MyTest01(string tcname) : base(tcname) { }
        TypeSutString m_myParameter = new TypeSutString("My Test 01 Parameter"
                                                        , "-/-/-"
                                                        , "bardak ((( ;-)"
                                                        , "Hello!!! - World! ");
        TypeSutBool m_bUpperClase = new TypeSutBool("U!!!pper Case conversion"
                                                        , "!!!--"
                                                        , "----Determines if the Test Parameter shall be converted to upper case "
                                                        , true);

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}