using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Configuration;
using Etas.Eas.Atcl.Interfaces.Factory;
using Etas.Eas.Atcl.Interfaces.MetaData;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Sar;
using Etas.Eas.Atcl.Interfaces.TestParameter;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;



namespace GM_Tests
{
    public class TC_SparkDwellScatter : Etas.Eas.Atcl.Interfaces.TestCase
    {
        GMUsefulFunctions GMLib ;


        #region Variables used in the Test Case

        //Parameter definition:
        //Log Time and Allowed Deviation shall be parameterized 
        TypeSutFloat ESTALogTime = new TypeSutFloat("Time to Log Spark Events","--", "Spark Events Logging time ",100,0,10000, "s" );
        TypeSutFloat AllowedDeviation = new TypeSutFloat("Detection Deviation allowed ", "--", "Percentage of ECU measurement deviation allowed ",3,0,100,"%");


        #endregion

        #region main() Function
        //Main entry point of this test case
        public static void Main(string[] args)
        {


            TC_SparkDwellScatter tc = new TC_SparkDwellScatter(); // create a new instance of the test case class
            
            try
            {
                tc.GMLib = new GMUsefulFunctions(tc);
                
                tc.Init();
                tc.PerformTest();

            }
            catch (Exception ex)
            {
                tc.Error(); //Sets the overall Test Verdict to "Error"
                tc.Reporting.LogExtension("Error in TestCase TC_SparkTest:"); //Logs the ErrorDefinition in the Test Handler Log window
                tc.Reporting.SetErrorText(0, ex.Message, 0);    //Writes an error into the report 
            }
            finally
            {
                tc.Finished();  //Cleans up the test case
            }
        }

        // Initializes a new instance of the TC_HelloWorld class ()
        public TC_SparkDwellScatter() : base("TC_SparkDwellScatter") { }

        #endregion

        #region Test Case Initialization()

        //Initializes the Test case data
        private void Init()
        {
            //Assign Metadata to the test case
            AddMetaData("TCD", "ETAS Product Team");
            AddMetaData("Comment", "Demo Test case for GM");
            AddMetaData("Version", "V0.1.0");

            //Make "Spedifiy the Parameter interface for the Test Manager tool:
            Factory.GetParameterManager().CreateTpaFile();              //Start specification
            Factory.GetParameterManager().Register(ESTALogTime);        // Add "Logtime" to the parameter list"
            Factory.GetParameterManager().Register(AllowedDeviation);   // Add "Allowed Deviation " to the parameter list"
            Factory.GetParameterManager().Save();

            GMLib.InitTestBench();


        }

        #endregion

        #region TestCase()
        //The main test case funtionality
        private void PerformTest()
        {
            try
            {
                //Read in the values from the parameter file 
                ESTALogTime = (TypeSutFloat)GMLib.readParameter(ESTALogTime);
                AllowedDeviation = (TypeSutFloat)GMLib.readParameter(AllowedDeviation);

                GMLib.configureHilLogger("ESTA", "AcquisitionTask", ESTALogTime.Value);
                GMLib.configureECULogger("SfSPKO_t_Dwell", "Task 2ms", ESTALogTime.Value);
                Reporting.LogExtension("All Dataloggers armed and triggered!");
 
                Reporting.LogExtension("Starting the measurements");
                GMLib.startTestbench();

                GMLib.ECM_ON();
                GMLib.SPARK_ACTIVE();

                Reporting.LogExtension("* Start Test Test");

                GMLib.startDataloggingAndWaitUntilFinished();

                TypeSut1DFloatTable ECUSparkLog = GMLib.getLoggedECUSignals("SfSPKO_t_Dwell");
                double ECUSparkMIN = GMLib.getMinValue(ECUSparkLog.ValueY);
                double ECUSparkMAX = GMLib.getMaxValue(ECUSparkLog.ValueY);
                double ECUSparkAVE = GMLib.getMeanValue(ECUSparkLog.ValueY);

                TypeSut1DFloatTable ESTALog = GMLib.getLoggedHiLSignals("ESTA");
                double ESTAMIN = GMLib.getMinValue(ESTALog.ValueY);
                double ESTAMAX = GMLib.getMaxValue(ESTALog.ValueY);
                double ESTAAVE = GMLib.getMeanValue(ESTALog.ValueY);

                if (GMLib.IsValueInLimit(ECUSparkMIN, ESTAMIN, AllowedDeviation.Value) == false)
                {
                    Reporting.SetText(2, 0, "Minimum values of Model and ECU are not within the specified range!", 0);
                    Fail();
                }
                else if (GMLib.IsValueInLimit(ECUSparkAVE, ESTAAVE, AllowedDeviation.Value) == false)
                {
                    Reporting.SetText(2, 0, "Average values of Model and ECU are not within the specified range!", 0);
                    Fail();
                }
                else if (GMLib.IsValueInLimit(ECUSparkMAX, ESTAMAX, AllowedDeviation.Value) == false)
                {
                    Reporting.SetText(2, 0, "Average values of Model and ECU are not within the specified range!", 0);
                    Fail();
                }
                else
                {
                    Reporting.SetText(2, 0, "Minimum, maximum and average values Model and ECU  within the specified range!", 0);
                    Pass();
                }

                #region Plot and Log

                // Create a table with all the results
                Reporting.CreateTable("ResultTable", "", 0, 0);
                Reporting.SetTableHeadline("ResultTable", "Overview on the test evaluation of the Spark Events", 0, 3);
                Reporting.SetTableColDescription("ResultTable", new string[] { "Events", "Min ", "Ave", "Max" });
                Reporting.SetTableData("ResultTable", new string[] { "Software", ECUSparkMIN.ToString(), ECUSparkAVE.ToString(), ECUSparkMAX.ToString() });
                Reporting.SetTableData("ResultTable", new string[] { "Hardware", ESTAMIN.ToString(), ESTAAVE.ToString(), ESTAMAX.ToString() });
                Reporting.AddTableToReport("ResultTable");

                // Create a plot with all the results
                IPlot plot = Reporting.CreatePlot("Plot", "Spark Events Software/Hardware ", 1.3);
                plot.AddYAxis("Spark Events ", -30,10, "--", 10.0);
                plot.SetXAxis("time", 0.0, ESTALogTime.Value, "s", 1.0);


                plot.YAxisCollection[0].AddLine("Software", ESTALog.ValueX, ESTALog.ValueY, 0.0, 0.0,
                    plot.CreateLineFormat("red", LineWeight.Thin, LineStyle.Stroke));
                plot.YAxisCollection[0].AddLine("Hardware", ECUSparkLog.ValueX, ECUSparkLog.ValueY, 0.0, 0.0,
                    plot.CreateLineFormat("blue", LineWeight.Thin, LineStyle.Dashed));

                Reporting.AddPlot2Report(plot);

                #endregion

                Reporting.LogExtension("* Stop ECT_test");
            }
            catch (Exception ex)
            {
                Error();
                Reporting.SetErrorText(0, ex.Message, 0);
                throw new Exception("Error in PerformTest!");
            }

            finally
            {
                GMLib.stopTestbench();

                Reporting.LogExtension("* Stop ECT_test");
            }
        }

        #endregion
    }

}
