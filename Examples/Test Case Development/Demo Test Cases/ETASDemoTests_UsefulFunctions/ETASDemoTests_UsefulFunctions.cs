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

namespace ETASDemoTests
{


    public class ETASDemoTests_UsefulFunctions 
    {

        

        // Ports to the Test Bench
        IPortMA HiLAccess = null;
        IPortEAM ECUAccess = null;
        public TestCase tc = null;


        // Initializes a new instance of the TC_HelloWorld class ()
        public ETASDemoTests_UsefulFunctions(TestCase testcase)
        {
            this.tc = testcase;
            
        }

        /// <summary>
        /// Standard quick and easy reporting function writing a text into the report and in the Log Window of the Test Handler 
        /// </summary>
        /// <param name="text">The string appearing in the report and the application log</param>
        public void LogAndReport(string text)
        {
            
            tc.Reporting.LogExtension(text);
            tc.Reporting.SetText(2, 0, text, 0);              
        }

        public void InitTestBench()
        {
            try{

                //Tell Automation, what port you need
                HiLAccess = tc.Factory.GetPortMA("HiLAccess");                 //tell LCA that you need a Model Access Port 
                HiLAccess.Timeout = 200000;
                ECUAccess = tc.Factory.GetPortEAM("ECUAccess");                //tell LCA that you need an ECU Access Port
                ECUAccess.Timeout = 200000;

                //Check the State of the Test Bench and ensure all tools are in "Configured" State
                tc.Reporting.LogExtension("Preparing HilAccess Tool");
                if (HiLAccess.GetState() != PortStatusEnum.PortConfigured)
                    HiLAccess.Configure(new string[0]);

                tc.Reporting.LogExtension("Preparing ECUAccess Tool");
                if (ECUAccess.GetState() != PortStatusEnum.PortConfigured)
                    ECUAccess.Configure("INCADEMODEVICE", "DEFAULT", "DEFAULT");


            }
            catch(Exception ex)
            {
                tc.Reporting.SetErrorText(0, ex.Message, 0);    //Writes an error into the report 
                throw new Exception(ex.Message);
            }


        }


        public void configureHilLogger(string signalname, string acquisitiontask, double duration)
        {
            // Preparing Datalogger for HIL
            TypeDLConfigureRecord dlConfigurationHIL = new TypeDLConfigureRecord(
                "StandardLogHIL",	    // abstract Name of the logfile
                duration,			// Logging duation in seconds
                new TypeDLSignal[]	    //Array for all signals to be recorded
						{
							new TypeDLSignal ( signalname , HiLAccess, acquisitiontask )	//ESTA 
						}
                );

            //Configure the datalogger
            HiLAccess.ConfigureDataLogger(dlConfigurationHIL);

        }

        public void configureECULogger(string signalname, string acquisitiontask, double duration)
        {
            // Preparing Datalogger for HIL
            TypeDLConfigureRecord dlConfigurationECU = new TypeDLConfigureRecord(
                "StandardLogECU",	    // abstract Name of the logfile
                duration,			// Logging duation in seconds
                new TypeDLSignal[]	    //Array for all signals to be recorded
						{
							new TypeDLSignal ( signalname , ECUAccess, acquisitiontask )	//ESTA 
						}
                );

            //Configure the datalogger
            ECUAccess.ConfigureDataLogger(dlConfigurationECU);
        }

        public void startTestbench()
        {
            HiLAccess.Start();
            ECUAccess.Start();

        }

        public void stopTestbench()
        {
            
            ECUAccess.Stop();
            HiLAccess.Stop();
        }

        public void startDataloggingAndWaitUntilFinished()
        {
            HiLAccess.StartDataLogger();
            ECUAccess.StartDataLogger();

            //Wait for completion of Datalogger
            while ((HiLAccess.GetDataLoggerState() == PortStatusEnum.PortRunning)
                    || (ECUAccess.GetDataLoggerState() == PortStatusEnum.PortRunning))
            {
                Thread.Sleep(100);
            }
        }


        public double getMeanValue(double[] values)
        {
            double average = 0;

                for (int i = 0; i < values.Length; i++)
                {
                    average = average + values[i];
                }
                average = average / values.Length;

                return average;
        }

        public double getMinValue(double[] values)
        {
            double min = double.MaxValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] < min)
                    min = values[i];
            }
            return min;
        }

        public double getMaxValue(double[] values)
        {
            double max = double.MinValue;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] > max)
                    max = values[i];
            }
            return max;
        }

        public TypeSut1DFloatTable getLoggedHiLSignals(string signallabel)
        {

            //Evaluate the Signals
            TypeSut1DFloatTable[] signals =
                new TypeSut1DFloatTable[]  //Create a 1D-Table Array  to contain time and value axes
						{
							//Use the default constructor
							new TypeSut1DFloatTable ( "" , signallabel , "" , new double [ ] { 0.0 , 1.0 } , new double [ ] { 0.0 , 2.0 } , 
							0.0 , 0.0 , 0.0 , 0.0 , "" , "" )
						};

            //Get the signals array from the datalogger file  (it may contains several signals
            signals = HiLAccess.GetLoggedSignals("StandardLogHIL", signals);
            // the interest is for the first available signal of the signals array, which is the engine Speed:
            return signals[0];
        }
        public TypeSut1DFloatTable getLoggedECUSignals(string signallabel)
        {

            //Evaluate the Signals
            TypeSut1DFloatTable[] signals =
                new TypeSut1DFloatTable[]  //Create a 1D-Table Array  to contain time and value axes
						{
							//Use the default constructor
							new TypeSut1DFloatTable ( "" , signallabel , "" , new double [ ] { 0.0 , 1.0 } , new double [ ] { 0.0 , 2.0 } , 
							0.0 , 0.0 , 0.0 , 0.0 , "" , "" )
						};

            //Get the signals array from the datalogger file  (it may contains several signals
            signals = ECUAccess.GetLoggedSignals("StandardLogECU", signals);
            // the interest is for the first available signal of the signals array, which is the engine Speed:
            return signals[0];
        }


        public bool IsValueInLimit(double value, double valueshould, double PercentAllowedDeviation)
        {
            if ((value * (1 - PercentAllowedDeviation / 100) < valueshould)
                && (value * (1 + PercentAllowedDeviation / 100) > valueshould))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Helper Function to start the ECU
        public void ECM_ON()
        {
            TypeSutFloat EngineSpeed = new TypeSutFloat("Engine Speed", "", "", 0.0, 0, 10000, "");
            EngineSpeed.Label = "ENGSPD";
            EngineSpeed.Unit = "rpm";
            EngineSpeed.Value = 700.0;

            HiLAccess.SetModelValue(EngineSpeed);
            tc.Reporting.SetText(2, 0, "ECM started!", 0);


        }

        //Helper Function to start ignition
        public void SPARK_ACTIVE()
        {

        }
        public TypeSutBase readParameter(TypeSutBase Param)
        {
            return (TypeSutBase)tc.Factory.GetParameterManager().Parameterise(Param);
        }
    }
}
