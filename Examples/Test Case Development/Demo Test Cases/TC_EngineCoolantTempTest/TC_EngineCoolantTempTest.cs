using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Ports;



namespace GM_Tests
{
    public class TC_EngineCoolantTempTest : Etas.Eas.Atcl.Interfaces.TestCase
    {
        #region Variables used in the Test Case

        //Parameter definition
        TypeSutFloat CoolTemp = new TypeSutFloat("Coolant Temperature","ECT", "Parameter to define the temperature of the Coolant in the model",0,-50,95, "degC" );
        TypeSutFloat AllowedDeviation = new TypeSutFloat("Detection Deviation allowed ", "--", "Percentage of ECU measurement deviation allowed ",3,0,100,"%");

        // Ports to the Test Bench
        IPortMA     HiLAccess = null; 
        IPortEAM    ECUAccess = null;

        #endregion

        #region Main entry point and execution

        //Main entry point of this test case
        // All test cases use this  function
        public static void Main(string[] args)
        {

            TC_EngineCoolantTempTest tc = new TC_EngineCoolantTempTest(); // create a new instance of the test case class
            try
            {
                //The internal test state machine is called here:
                tc.Init();
                tc.PerformTest();

            }
            catch (Exception ex)
            {
                // Default test case error handling
                tc.Error();     //Setting the test verdict to "ERROR"
                tc.Reporting.LogExtension("Error in TestCase TC_EngineCoolantTemp:"); //Write an error message to the test handler's application window
                tc.Reporting.SetErrorText(0, ex.Message, 0);  //write an error message into the report
            }
            finally
            {
                tc.Finished();  //finish the test case anyway
            }
        }

        // Initializes a new instance of the TC_HelloWorld class ()
        public TC_EngineCoolantTempTest() : base("TC_EngineCoolantTempTest") { }

        #endregion

        #region Test Case Initialization
        //Initializes the Test case data
        private void Init()
        {
            //Assign Metadata to the test case
            AddMetaData("TCD", "ETAS Product Team");
            AddMetaData("Comment", "Demo Tes case for GM!");

            
            //Define the parameter interface for this test cases available for LCA Parameterization
            Factory.GetParameterManager().CreateTpaFile();  //Initialisation for parameter creation
            Factory.GetParameterManager().Register(CoolTemp); //Add "Cooltemp" to parameter file
            Factory.GetParameterManager().Register(AllowedDeviation); //Add "Allowed deviation" to parameter file
            Factory.GetParameterManager().Save(); //Finish test parameter creation
            
            //Tell Automation, what port you need
            Reporting.LogExtension("Preparing Test Bench Access");
            HiLAccess = Factory.GetPortMA("HiLAccess");  //Tell Automation to use a Model Access port named "HilAccess"
            HiLAccess.Timeout = 200000;                     //Tell Automation that each signature shall wait not longer than 100s before continuing
            ECUAccess = Factory.GetPortEAM("ECUAccess");  //Tell Automation to use a Model Access port named "HilAccess"
            ECUAccess.Timeout = 200000;

        }

        #endregion

        #region Test Case Execution

        //The main test case funtionality
        private void PerformTest()
        {
            try
            {
                //Read in actual Parameter data!

                CoolTemp            = (TypeSutFloat)Factory.GetParameterManager().Parameterise(CoolTemp);
                AllowedDeviation = (TypeSutFloat)Factory.GetParameterManager().Parameterise(AllowedDeviation); 

            //Check the State of the Test Bench and ensure all tools are in "Configured" State
                Reporting.LogExtension("Preparing HilAccess Tool");
                if ( HiLAccess.GetState () != PortStatusEnum.PortConfigured )
                    HiLAccess.Configure(new string[0]);

                Reporting.LogExtension("Preparing ECUAccess Tool");
                if ( ECUAccess.GetState() != PortStatusEnum.PortConfigured )
                    ECUAccess.Configure("GMDEVICE", "DEFAULT", "DEFAULT");


                // For ECU Access , the signals to be measured have to be prepared in the ETK access...)
                Reporting.LogExtension("Preparing ECU Access Measuremet labels...");
                TypeDLSignal ECUElement1 = new TypeDLSignal("SfECTI_T_EngCool", ECUAccess, "Task 2ms");
                TypeDLSignal ECUElement2 = new TypeDLSignal("Demo Input 1 label", ECUAccess, "Task 2ms"); //Example of a second inner ECU element
                TypeDLSignal[] SelectedElements = new TypeDLSignal[] { ECUElement1, ECUElement2 };  //Telling ECUAccess, that elements in the tasks shall be generated
                ECUAccess.SelectElements(SelectedElements);


                HiLAccess.Start(); //Start HilAccess
                ECUAccess.Start(); //Start ECUccess
                Reporting.LogExtension("Test Bench set to 'running'");
                Reporting.SetText(2,0,"Test Bench is running",0);

                Reporting.LogExtension("* Switching on the ECU...");
                ECM_ON();

                Reporting.LogExtension("* Start ECT Test");
                Reporting.SetText(2,0,"Initializations for ECT Test successsful.",0);

                HiLAccess.SetModelValue(CoolTemp);                 //Set the value in the HiL
                Reporting.SetText(2,0,"Setting the Engine Coolant Temp to " + CoolTemp.Value.ToString() + " °C",0);
                Reporting.LogExtension("Setting Engine Speed to " + CoolTemp.Value.ToString() + " °C");

                //Read a Value from the ECU
                TypeSutFloat SfECTI_T_EngCool = new TypeSutFloat("ECU Coolant temperature","","",0,-1000,1000,"degC" );
                SfECTI_T_EngCool.Label = "SfECTI_T_EngCool";
                SfECTI_T_EngCool.Unit = "degC";
                SfECTI_T_EngCool = (TypeSutFloat) ECUAccess.GetValue(SfECTI_T_EngCool);
                Reporting.SetText(2,0,"Reading the Engine Coolant Temp from ECU: " + SfECTI_T_EngCool.Value.ToString() + " °C",0);
                Reporting.LogExtension("Reading the Engine Coolant Temp from ECU: " + SfECTI_T_EngCool.Value.ToString() + " °C");

                //Evaluate the result:
                Reporting.LogExtension("Evaluation beginning...");
                double MinAllowed = CoolTemp.Value *(1-AllowedDeviation.Value/100);
                double MaxAllowed = CoolTemp.Value * (1 + AllowedDeviation.Value / 100);

                if ((SfECTI_T_EngCool.Value > MinAllowed) && (SfECTI_T_EngCool.Value < MaxAllowed))
                {
                    Reporting.SetText(2,0,"'SfECTI_T_EngCool' and 'ECT' are equal within the allowed deviation" ,0);
                    Pass();
                }
                else
                {
                    Reporting.SetText(2,0,"'SfECTI_T_EngCool' and 'ECT' are not equal within the allowed deviation" ,0);
                    Fail();
                }

                Reporting.LogExtension("Evaluation done.");
                
                ECUAccess.Stop();
                HiLAccess.Stop();


                // Creation of a table in the report depicting the result:
                string tableID = "ECT Evaluation Table";
                Reporting.CreateTable(tableID, "ECT Test Evaluation Result", 0, 0);
                Reporting.SetTableHeadline(tableID, "Measurement", 0, 1);
                Reporting.SetTableHeadline(tableID, "Limits", 2, 4);
                Reporting.SetTableHeadline(tableID, "Result", 4, 5);
                Reporting.SetTableColDescription(tableID,
                   new string[] { "Label", "Measure Value", "Tolerance", "Min", "Max", "Verdict" });
                Reporting.SetTableData(tableID,
                    new string[] { SfECTI_T_EngCool.Label, SfECTI_T_EngCool.Value.ToString(), this.AllowedDeviation.Value.ToString() + "%", MinAllowed.ToString(), MaxAllowed.ToString(), this.Verdict });
                Reporting.AddTableToReport(tableID);
                //Report Table finshed

                Reporting.LogExtension("* Stop ECT_test");
                Reporting.SetText(2,0,"ECT Test successsfully executed",0);
            }
            catch (Exception ex)
            {
                Error();
                Reporting.SetErrorText(0, ex.Message, 0);
                Reporting.LogExtension(ex.Message);
                throw new Exception ("Error in PerformTest!");
            }
        }

        #endregion

        #region Helper Functions
        //Helper Function to start the ECU
        private void ECM_ON()
        {
                //TODO Everything which is necessary to start the ECM
                // Probably Setting another Parameter???
				//TypeSutSwitch engineSwitch		= new TypeSutSwitch("Engine", "StartEngine", "",  "Off", "OnOffSwitch");
				//TypeSutSwitch ignitionSwitch	= new TypeSutSwitch("Ignition", "StartIgnition", "",  "Off", "OnOffSwitch");

				//engineSwitch.Value		= "On";
				//ignitionSwitch.Value	= "On";
				//HiLAccess.SetModelValue(ignitionSwitch );
				//HiLAccess.SetModelValue(engineSwitch );
				//Reporting.SetText(2,0,"Engine started and ignition switched on",0);
                //Reporting.LogExtension("Engine started and ignition switched on");	

            TypeSutFloat EngineSpeed = new TypeSutFloat("Engine Speed", "","",0.0,0,10000,"");
            EngineSpeed.Label   = "ENGSPD";
            EngineSpeed.Unit    = "rpm";
            EngineSpeed.Value = 700.0;

            HiLAccess.SetModelValue(EngineSpeed);

            Reporting.LogExtension("Engine Speed set to " + EngineSpeed.Value + " rpm");
            Reporting.SetText(2, 0, "Engine Speed set to " + EngineSpeed.Value + " rpm",0);


        }


        #endregion

    }


}
