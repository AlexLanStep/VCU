using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.TestParameter;


//*********************************************
// The "TC_HelloWorld" Example shows the basic concept of test case development.
// A string ("m_TestParameter") is parameterized by the and the evaluation checks, if the text of the parameter is "Hello World!".
// If not, the test case fails


namespace TC_HelloWorld
{
    //A Test Case is a class derived from the Base Test Case of the ATCL
    public class TC_HelloWorld : Etas.Eas.Atcl.Interfaces.TestCase
    {
        // Parameters and Test Case Variables:
        // Some Parameters shall be used with values defined form a Test Parameter Manager.
        // These parameters should be used in the test case as variables of the ATCL Variable types ("TypeSut...")
        // These ATCL Types contain meta data like comments units and so forth next to the value itself.
        // In the following, three Parameters (a string, a bool and an integer are defined
        TypeSutString   m_TestParameter = new TypeSutString("A Test Parameter","--", "An example of parameterization usage, in this case a string", "Hello World!");
        TypeSutBool     m_bUpperCase    = new TypeSutBool("Upper Case conversion", "--", "Determines if the Test Parameter shall be converted to upper case ", true);
        TypeSutInteger  m_Waittime      = new TypeSutInteger("Sleep Time before Evaluation", "--", "Waiting time before executing the evaluation", 1, 0, 10, "s");

        string ReferenceText = "Hello World!";

        //Main entry point of this test case (The Main function)
        public static void Main(string[] args)
        {
            TC_HelloWorld tc = new TC_HelloWorld(); // create a new instance of the test case class
            try
            {   // A Typical test case sequence consists of Initialization, Stim and MEasure, and evaluation:
                tc.Initialization();
                tc.StimulationAndMeasurement();
                tc.Evaluation(); 
            }
            catch (Exception ex)
            {
                tc.Error(); //Sets the Test Case verdict to "Error"
                tc.Reporting.LogExtension("Error Occured in Test Case! See report for details"); // Logs the error in the Test Handler's Message Window
                tc.Reporting.SetErrorText(0, "Test Case 'TC_HelloWorld' exited with exception:" + ex.Message, 0); //Sets an Error Text in the Report 
            }
            finally
            { // The finally statement is executed in any case (evenif an exception occured, therefor the Test Case Finsihing is used here
                tc.Finished();  //Closes down the test case execution with all report storage
            }
        }

        // The Constructor of the Test Case class (deines the name of the test case)
        public TC_HelloWorld() : base("TC_HelloWorld") { }

        //Test Sequence:  1 - Initialization of test case data
        private void Initialization()
        {
            //Assign Metadata to the test case, Meta data consiss of any key/value pair
            AddMetaData("Test Case Developer", "ETAS Product Team");
            AddMetaData("Comment", "This is the Hello World example for LCA!");
            AddMetaData("Version", "V1.0.1");

            //The following sequence tells LCA to create a parameter specification for later parameterization in LABCAr-TM Parameter Manager
            Factory.GetParameterManager().CreateTpaFile();              // Start the parameter specification 
            Factory.GetParameterManager().Register(m_TestParameter);    // Add "m_TestParameter" 
            Factory.GetParameterManager().Register(m_bUpperCase);       // Add "m_bUpperCase"
            Factory.GetParameterManager().Register(m_Waittime);         // Add "m_Waittime"
            Factory.GetParameterManager().Save();                       // Specification Finished -> Generation of Test Case Library xml files (*.thd, *.tad, *.tpa)
        }

        //Test Sequence:  2 - Stimulation and Measurment 
        private void StimulationAndMeasurement()
        {
            try
            {   
                //Read in Parameter Data from Parameter Set with the data created by the Test Parameter Manager in LABCAR-TM   
                m_TestParameter = (TypeSutString)Factory.GetParameterManager().Parameterise(m_TestParameter);
                m_bUpperCase = (TypeSutBool)Factory.GetParameterManager().Parameterise(m_bUpperCase);
                m_Waittime = (TypeSutInteger)Factory.GetParameterManager().Parameterise(m_Waittime);
                
                // print the data to the report
                Reporting.SetText(2, 0, "The Test Parameter has the value: " + m_TestParameter.Value,0);
                Reporting.SetText(2, 0, "Upper Case String Handling: " + m_bUpperCase.Value.ToString(), 0);

                // Turn the string to upper case if defined so by the respective parameter:
                if (m_bUpperCase.Value == true)
                {
                    m_TestParameter.Value = m_TestParameter.Value.ToUpper();
                    ReferenceText = ReferenceText.ToUpper();
                }
            }
            catch (Exception ex)
            {
                Error();
                Reporting.SetErrorText(0, "Error in StimandMeasure: " + ex.Message, 0);
                throw new Exception("Error in Function StimulationandMeasurement");
            }
        }

        //Test Sequence:  3 - Evaluation of measured data & verdict creation
        private void Evaluation()
        {
            try
            {
                Reporting.SetText(2, 0, "Waiting for " + m_Waittime.Value + m_Waittime.Unit, 0);
                Thread.Sleep(m_Waittime.Value * 1000); // Wait the specified time before continuing

                if (m_TestParameter.Value == ReferenceText)
                {
                    Reporting.SetText(2, 0, "The Test Parameter could be validated as 'Hello World' Text!",0);
                    Pass();
                }
                else
                {
                    Reporting.SetText(2, 0, "The Test Parameter could not be validated as 'Hello World' Text!",0);
                    Fail();
                }
                Reporting.SetText(2, 0, "Test finished!", 0);
            }
            catch (Exception ex)
            {
                Error();
                Reporting.SetErrorText(0, "Error in Evaluation: " + ex.Message, 0);
                throw new Exception("Error in Function Evaluation");
            }
        }
    }


}
