using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Ports;



namespace ETASDemoTests
{
    public class TC_FuelPump : Etas.Eas.Atcl.Interfaces.TestCase
    {
        #region Variables used in the Test Case

        //Parameter definition
        TypeSutFloat    EngineSpeed     = new TypeSutFloat("Engine Speed","ENGSPD", "Parameter Setting the Engine Speed to a constant value",0,0,10000, "rpm" );
        TypeSutInteger  Waittime        = new TypeSutInteger("Wait time for Relaxation", "--", "Standard Wating time", 2, 0, 1000, "s");
        TypeSutSwitch   FuelPumpState = new TypeSutSwitch("Fuel Pump State", "FUELP2", "", "Off", "OnOffSwitch");

        // Ports to the Test Bench
        IPortMA     ModelAccess = null; 

        #endregion

        #region Main entry point and execution
        //Main entry point of this test case
        public static void Main(string[] args)
        {

            TC_FuelPump tc = new TC_FuelPump(); // create a new instance of the test case class
            try
            {
                //The standard Test engine sequence
                tc.Init();
                tc.PerformTest();

            }
            catch (Exception ex)
            {
                //Standard failure treatment
                tc.Error();
                tc.Reporting.LogExtension("Error in TestCase TC_FuelPump:");
                tc.Reporting.SetErrorText(0, ex.Message, 0);
            }
            finally
            {
                tc.Finished();
            }
        }

        // Initializes a new instance of the TC_FuelPump class ()
        public TC_FuelPump() : base("TC_FuelPump") { }

        #endregion

        //Initializes the Test case data
        private void Init()
        {
            //Assign Metadata to the test case
            AddMetaData("TCD", "ETAS Product Team");
            AddMetaData("Comment", "Demo Test case for GM!");

            //Make "the two parameters available" Available for LCA Parameterization
            Factory.GetParameterManager().CreateTpaFile();
            Factory.GetParameterManager().Register(EngineSpeed);
            Factory.GetParameterManager().Register(Waittime);
            Factory.GetParameterManager().Save();

            //Tell Automation, what port you need
            Reporting.LogExtension("Preparing Test Bench Access");
            ModelAccess = Factory.GetPortMA("ModelAccess");  //Tell Automation to use a Model Access port named "Model Access"
            ModelAccess.Timeout = 200000;                     //Tell Automation that each signature shall wait not longer than 100s before continuing


            //Read in the value from the parameter file 
            Reporting.LogExtension("Reading in parameters...");
            EngineSpeed = (TypeSutFloat)Factory.GetParameterManager().Parameterise(EngineSpeed);
            Waittime = (TypeSutInteger)Factory.GetParameterManager().Parameterise(Waittime);

            //Check the State of the Test Bench and ensure all tools are in "Configured" State
            if (ModelAccess.GetState() != PortStatusEnum.PortConfigured)
                ModelAccess.Configure(new string[0]);

            ModelAccess.Start();

            Reporting.LogExtension("Test Bench Configured and running");
            Reporting.SetText(2,0,"Test Bench Configured and running",0);

            ECM_ON();

        }

        //The main test case funtionality
        private void PerformTest()
        {
            try
            {
                Reporting.LogExtension("* Start Fuel Pump Test");
  
                //Set the value in the HiL
                ModelAccess.SetModelValue(EngineSpeed);
                Reporting.SetText(2,0,"Setting the Engine Speed to  " + EngineSpeed.Value.ToString() + " rpm",0);
                Reporting.LogExtension("Setting the Engine Speed to  " + EngineSpeed.Value.ToString() + " rpm");
   
                // Wait for a specific time
                Thread.Sleep(Waittime.Value*1000);
                Reporting.LogExtension("Waiting ...");
                Reporting.SetText(2, 0, "Waiting for " + (Waittime.Value*1000).ToString() + " s", 0);

                //Read the fuel Pump State:
                FuelPumpState = (TypeSutSwitch)ModelAccess.GetModelValue(FuelPumpState);
                Reporting.LogExtension("Reading Fuel Pump state from HiL ...");
                Reporting.SetText(2, 0, "Fuel Pump State in HiL: " + FuelPumpState.Value , 0);

                //Evaluation 
                Reporting.LogExtension("Starting Evaluation");

                if (EngineSpeed.Value > 0)
                {
                    if (FuelPumpState.Value == "On")
                    {
                        Reporting.SetText(2, 0, "Found Situation: Running Engine and Fuel Pump -> OK ", 0);
                        Pass();
                    }
                    else
                    {
                        Reporting.SetErrorText(0, "Found Situation: Running Engine and deactivated Fuel Pump -> NOK ", 0);
                        Fail();
                    }
                }
                else //Engine Speed ==0
                {
                    if (FuelPumpState.Value == "On")
                    {
                        Reporting.SetErrorText(0, "Found Situation: Engine off but running Fuel Pump -> NOK ", 0);
                        Fail();
                    }
                    else
                    {
                        Reporting.SetText(2, 0, "Found Situation: Engine and Fuel Pump not running -> OK ", 0);
                        Pass();
                    }
                }
                // Creation of a table in the report depicting the result:
                string tableID = "Fuel Pump Evaluation Table";
                Reporting.CreateTable(tableID, "Fuel Pump Test Evaluation Result", 0, 0);
                Reporting.SetTableHeadline(tableID, "Measurement", 0, 1);
                Reporting.SetTableHeadline(tableID, "Result", 2, 3);
                Reporting.SetTableColDescription(tableID,
                   new string[] { "Label", "Measure Value", "Engine Speed", "Verdict" });
                Reporting.SetTableData(tableID,
                    new string[] { FuelPumpState.Label, FuelPumpState.Value.ToString(), this.EngineSpeed.Value.ToString() + " rpm", this.Verdict });
                Reporting.AddTableToReport(tableID);
                //Report Table finshed

                ModelAccess.Stop();
                Reporting.LogExtension("* Stop FuelPump Test");
            }
            catch (Exception ex)
            {
                Error();
                Reporting.SetErrorText(0, ex.Message, 0);
                throw new Exception ("Error in PerformTest!");
            }
        }

        //Helper Function to start the ECU
        private void ECM_ON()
        {
            //To switch on the ECU, the Engine Speed is set to 700:

            TypeSutFloat EngineSpeed = new TypeSutFloat("Engine Speed", "", "", 0.0, 0, 10000, "");
            EngineSpeed.Label = "ENGSPD";
            EngineSpeed.Unit = "rpm";
            EngineSpeed.Value = 700.0;
            ModelAccess.SetModelValue(EngineSpeed);

            Reporting.LogExtension("Engine Speed set to " + EngineSpeed.Value + " rpm");
            Reporting.SetText(2, 0, "Engine Speed set to " + EngineSpeed.Value + " rpm", 0);

            Reporting.SetText(2, 0, "ECM switched on!",0);

        }

    }


}
