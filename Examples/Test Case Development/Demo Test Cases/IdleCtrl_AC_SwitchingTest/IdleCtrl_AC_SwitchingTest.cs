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

namespace IdleCtrl_AC_SwitchingTest
{


	/// <summary>
    /// IdleCtrl_AC_SwitchingTest
	/// This test case shows how to use the ATCL to control LABCAR-OPERATOR, use parameters, set verdicts and create a report.
    /// The test itself starts an engine, switches on a the ac control which creates a dip in the idling engine speed
    /// The engine speed is recorded in the mean time and the evaluation determines, if 
    /// the speed lies within specific value ranges
	/// </summary>
    
	class IdleCtrl_AC_SwitchingTest : TestCase    
	{
		// A standard C# class turns into an LCA Test case by deriving from "TestCase" which is defined in the ATCL ("ETAS.EAS.Atcl.Interfaces")

		#region Constructor

        public IdleCtrl_AC_SwitchingTest(string name)
            : base("IdleCtrl_AC_SwitchingTest")
		{
		}
		#endregion
		
		#region Local variables and parameters

		private IPortMA		ModelAccess ; //The ModelAccess Port to control e.g. LABCAR-OPERATOR



		// Declaration of the LCA test paraemters used within this test
		// All parameters are of one of the TypeSuT.... class from ATCL
        // Please compare this set of declaraitions to the appearance of the test parameterization in the LABCAR-TM Parameter pane

		private static TypeSutUserRecord	m_InitialConditions		= new TypeSutUserRecord("InitialConditions","Containing all parameters to reach the working point of the test");
		private static TypeSutSwitch		m_enforceEngineStart	= new TypeSutSwitch("Enforce Engine Start","","Use this to enforce the test case to explicitly start the DVE's Engine","Yes","YesNoSwitch");
		private static TypeSutFloat			m_RelaxationTime		= new TypeSutFloat("Idle Controller Relaxation Time", "RelaxTime","Time set by the test to wait before continuing",1,0,6,"s");
		
		private static TypeSutUserRecord	m_IdleControllerPars	= new TypeSutUserRecord("Idle Controller Settings","Containing all parameters to complete specify the Idle Controller Module");	
		private static TypeSutFloat			m_iidle					= new TypeSutFloat ( "I" , "IIdle", "the integral part of the PI Controller " , 0.0005 , 0.0 , 1.0 , ""	);
        private static TypeSutFloat			m_pidle					= new TypeSutFloat ( "P" , "PIdle", "the amplification factor of the PI Controller" , 0.02 , 0.0 , 1.0 , "");

		private static TypeSutUserRecord	m_ACTestPoint			= new TypeSutUserRecord("AC Test Point ","The test point of the Air Condition Switch-on test");
		private static TypeSutFloat			m_torque				= new TypeSutFloat ("Torque" ,"AirConditionTorque" ,"Torque required by Air Conditioning system" , -10.0 , -100.0 , 0.0 , "Nm");
        private static TypeSutUserRecord 	m_EvalLimits			= new TypeSutUserRecord("Evaluation boundaries Limits","Engine speed values in between the evaluation has to be to pass the test");
		private static TypeSutFloat			m_lowerLimit			= new TypeSutFloat ("Lower Limit" , "LowerLimit" , "The minimum rpm which must be maintained" , 600.0 , 400.0 , 1000.0 , "rpm" );
		private static TypeSutFloat			m_upperLimit			= new TypeSutFloat ("Upper Limit" , "UpperLimit" , "The maximum rpm which must be maintained" , 800.0 , 700.0 , 1200.0 , "rpm");
		
		private static TypeSutDynamicArray m_ACTestPointsDA		= new TypeSutDynamicArray ("AC Test Vector", "Set of A/C Torques to be evaluated for the complete test" , m_ACTestPoint);



        // The Actal verdict of the test case can e retrieved as follows:
        private Verdict ActualVerdict
        {
            get { return new Verdict(Factory.GetVerdictManager().GetVerdictOfTestCase()); }
        }

        //Provide all Manager Functionality, which is available   
        private IParameterManager Parameters
        {
            get { return Factory.GetParameterManager(); }
        }

        private IVerdictManager Verdicts
        {
            get { return Factory.GetVerdictManager(); }
        }

		#endregion

		#region Main Function - the execution entry point 
		/// <summary>
		/// This is the standard main function, which is called in case the test case is strated (either from Visual Studio or from the Test Handler)
		/// </summary>
		[ STAThread ]
		static void Main ( string [ ] args )
		{
            IdleCtrl_AC_SwitchingTest tc = new IdleCtrl_AC_SwitchingTest("IdleCtrl_AC_SwitchingTest"); //Creating an instance of the test case 
			try
			{
				//In this example the Test Functionality has been split in three separate functions, this can be dealt with in any manner the user likes
				tc.Initialization ();		//This function shall execute all necessary initialization actions
				tc.RegisterParameters();	//Setting up the test case parameter structure 
				tc.ModelInitialization ();     // This function initializes the Idlecontroller Model  
                tc.TestExecution();          //This function shall execute the test and evaluation itself 
			}
			catch ( Exception ex )
			{ //The general exception block ensures, that no error will disappear.
				// On exception, the test case verdict is set to error, 
				// The Error Message is passed to the report and to any LogExtension which might be available (in our case the Test Handler window)
				tc.Error ( );  //The Error() Function of the ATCL's Verdict Manager handles everything for us
				string MessageText = "The following error occured in the Test Case: " + ex.Message ;
				tc.Reporting.LogExtension ( MessageText);
				tc.Reporting.SetErrorText ( 0, MessageText, 0 );  // Please refer to the ATCL Documentation for details on the Error levels 
			}

			finally
			{ //This "finally" functionality is executed in any case (error or not) 
				tc.Finished ( );  // Indicates the LABCAR-AUTOMATION, that the Test case has finsihed, thus any cleaning task can be perfromed now.
			}
		}
		
		#endregion

		#region 1 - Initialization() - For all Initialization functions 
		private void Initialization ()
		{
			try
			{
				// Create meta data for the Test case
				// This meta data appears later on in reports 
				AddMetaData ( "Test Case Developer" ,      "ETAS Product Team" );
				AddMetaData ( "Version" ,  "1.1" ) ;
				AddMetaData ( "Purpose" ,  "A Test Case Example with HiL Access and Evaluation" ) ;
				AddMetaData ( "Comment" ,  "A C# Test Case with UserRecords and DynamicArrays");
				AddMetaData ( "ChangeLog", "Created" );

				// Create the port which is called "ModelAccess" in the test bench configuration
				ModelAccess = Factory.GetPortMA("ModelAccess")  ;
                ModelAccess.Timeout = 50000; 				//Sets the time-out which specifies the wait time for the signature to return.
			}
			catch ( Exception ex )
			{ 
				Error ( );  
				string MessageText = "The following error occured during Test Case Initialization: " + ex.Message;
				Reporting.LogExtension ( MessageText);
				Reporting.SetErrorText ( 0, MessageText, 0 ); 
			}
		}

		#endregion

		#region 2 - RegisterParameters () - Creating a Parameterization interface to the test case  

		private void RegisterParameters ()
		{
			// To make use of the full ATCL Test Case PArameterization one has to to the following steps
			//	1) Create the test parameterization set
			//	2) Build up the Parameter Hierarchy as desired (and seen in the Parameter Manager)
			//  3) Register all top level Hierarchy Parameter in the LCA Parameter Manager 
			//  4) Save the Parameterization structure for the default Test Release Library declaration
            //  5) Read in the actual values of parameters (from the Parameter file modified by the LABCAR-TM (Test Manager)

			//*********************************************************************
			//	1) Create the test parameterization set			
			Parameters.CreateTpaFile ();
			
			//*********************************************************************
			//	2) Build up the Parameter Hierarchy as desired (and seen in the Parameter Manager)

			//Fill the structure of the "Initial Conditions User Record"
			m_InitialConditions.Items.Add(m_enforceEngineStart);
			m_InitialConditions.Items.Add(m_RelaxationTime);

			//Fill the ControllerPars Structure
			m_IdleControllerPars.Items.Add(m_iidle);
			m_IdleControllerPars.Items.Add(m_pidle);

			//Fill the Test Point Structure
			m_ACTestPoint.Items.Add(m_torque);
			m_EvalLimits.Items.Add(m_lowerLimit);
			m_EvalLimits.Items.Add(m_upperLimit);
			m_ACTestPoint.Items.Add(m_EvalLimits);

			//*********************************************************************
			//  3) Register all top level Hierarchy Parameter in the LCA Parameter Manager 
            Parameters.Register(m_InitialConditions);
            Parameters.Register(m_IdleControllerPars);
            Parameters.Register(m_ACTestPointsDA);

			//*********************************************************************			
			//  4) Save the Parameterization structure for the default Test Release Library declaration
            Parameters.Save();

            //*********************************************************************			
            //  5) Read in the actual values of parameters (from the Parameter file modified by the LABCAR-TM (Test Manager)
            //   Import the Parameter data from the project by reading the toplevel parameters, all children are automatically imported.
            //
            m_InitialConditions     = (TypeSutUserRecord) Parameters.Parameterise(m_InitialConditions);
            m_IdleControllerPars    = (TypeSutUserRecord) Parameters.Parameterise(m_IdleControllerPars);
            m_ACTestPointsDA        = (TypeSutDynamicArray) Parameters.Parameterise(m_ACTestPointsDA);

            //*********************************************************************			
            //  6) Assign the Children parameters of the top level hierarchy 
            //

            m_iidle = (TypeSutFloat)m_IdleControllerPars.Items.GetItem("I");
            m_pidle = (TypeSutFloat)m_IdleControllerPars.Items.GetItem("P");

            m_RelaxationTime = (TypeSutFloat)m_InitialConditions.Items.GetItem("Idle Controller Relaxation Time");

		}

		#endregion

        #region 3 - Model Initialization

        private void ModelInitialization()
		{
			try
			{
                // The Test Case is designed to use the LABCAR-AUTOMATION Testt Bench Initialization,
                // hence no explicit port creation is required.
                
				// Trigger the finalization of the configuration and check, that it has been successful
				ModelAccess.Configure ( new string [ ] { "" } );
                if (!ModelAccess.IsConfigured)
				{ //Check that the tool is there
                    Reporting.SetErrorText(0, "The Configure()-Signature of ModelAccess failed!", 0);
					Inconc ( ); //The Test Case VErdict is set to "inconclusive"
					return;
				}

				Reporting.SectionBegin ( "Model Initialization" );

                // Set the Idle Controllers Parameters:
				ModelAccess.SetModelValue(m_iidle);
                ModelAccess.SetModelValue(m_pidle);

                Reporting.SetText(2,0,"The following Idle Controller Parameters are used:",0); //Report Class 2 is a standard message
                Reporting.LogExtension("Idle Controller Parameters I and P are set in Controller Model.");  //LogExtension() shows the line in the Test Handlers application log

                //Create a table in the report which displays the 
				// minimum and maximum value recorded by the data logger
				string tableID = "IdleParam";
				Reporting.CreateTable ( tableID, "Controller Parameters", 0, 0 );
				Reporting.SetTableHeadline ( tableID, "The following Parameters were set", 0, 1 );
				Reporting.SetTableColDescription ( tableID, new string [ ] { "Parameter", "Value" } );
				Reporting.SetTableData (tableID, new string [ ] { "I", m_iidle.Value.ToString()} );
				Reporting.SetTableData (tableID, new string [ ] { "P", m_pidle.Value.ToString()} );
				Reporting.AddTableToReport ( tableID );
                
                Pass ( );
			}
			
            catch ( Exception ex )
			{
				Error ( );
				Reporting.SetErrorText ( 0, ex.Message, 0 );
				throw ex;
			}
			finally
			{					 	
				Reporting.SectionFinished ( "Model Initialization was executed", ActualVerdict );
			}
        }
        #endregion


		#region 4 - TestExecution () - The test and evaluation itself 
        private void TestExecution()
		{
			try 
			{
				Reporting.SectionBegin("Test Execution and Evaluation" );
				Reporting.LogExtension("Starting Test and Evaluation Section ");

                int i = 0;
				Verdict testpointverdict = new Verdict(VerdictCode.None);
                					
                Reporting.CreateTable ( "ResultTable", "", 0, 0 );
				Reporting.SetTableHeadline ( "ResultTable", "Overview on the test evaluation of Engine Speeds", 0, 4 );
				Reporting.SetTableColDescription ( "ResultTable", new string [ ] {"AC Torque", "Min allowed", "Min recorded", "Max Recorded", "Max Allowed"} );
				
                //The test iterates through the Dynamic array for the different torques
                foreach(TypeSutUserRecord m_testpoint in m_ACTestPointsDA.Items)
				{
					Reporting.LogExtension("Starting Test and Evaluation iteration : " + i.ToString());
                    // Call the evaluate() Function for each torque
					testpointverdict = MeasureAndEvaluate(m_testpoint, i);
					this.Factory.GetVerdictManager().SetTestCaseVerdict(testpointverdict); //Set the overall verdict with the result of the small one
					i = i+1;
				}
                
				Reporting.AddTableToReport ( "ResultTable" );

			}
			
            catch ( Exception ex )
			{
				Error ( );
                Reporting.SetErrorText ( 0, ex.Message, 0 );
				Reporting.LogExtension("Error executing test: " + ex.Message);
				throw ex;
			}
			finally
			{
				Reporting.SectionFinished ( "Test according 'Spec 08/15' was executed!", this.ActualVerdict );
			}
		}

        #endregion
        
        //The evaluation function for each torque point
		private Verdict MeasureAndEvaluate(TypeSutUserRecord TestPointData, int iterationindex)
		{

			//Initialize the local verdict of this function
			Verdict localVerdict = new Verdict(VerdictCode.None);

			try 
			{
                Reporting.SectionBegin("Evaluate AC Torque Test Point #" + iterationindex);

                //Create three Switch parameters to turn on and off the engine, the ignition and the aircondition.
                TypeSutSwitch engineSwitch		= new TypeSutSwitch("Engine", "StartEngine", "",  "Off", "2pol-Schalter");
				TypeSutSwitch ignitionSwitch	= new TypeSutSwitch("Ignition", "StartIgnition", "",  "Off", "2pol-Schalter");
				TypeSutSwitch AirCondition		= new TypeSutSwitch("AirCondition",	"StartAirCondition",		"",  "Off", "2pol-Schalter");

				// Read the chidlren parameter data of the AC Torque Dynamic array element for this iteration 
				TypeSutFloat		ac_torque	= (TypeSutFloat) TestPointData.Items.GetItem("Torque");
				TypeSutUserRecord	evalRange	= (TypeSutUserRecord) TestPointData.Items.GetItem("Evaluation boundaries Limits");
				TypeSutFloat		minRPM		= (TypeSutFloat) evalRange.Items.GetItem("Lower Limit");
				TypeSutFloat		maxRPM		= (TypeSutFloat) evalRange.Items.GetItem("Upper Limit");

                // Set the actual torque value in the Model:
                ModelAccess.SetModelValue(ac_torque );	
                Reporting.SetText(2,0,"Set the AC torque to " + ac_torque.Value.ToString()  + " " + ac_torque.Unit,0);
                Reporting.LogExtension("Set the AC torque to " + ac_torque.Value.ToString()  + " " + ac_torque.Unit);
					
	
				//Switch off all aggregates to have a specific start point (Engine, Ingition and Air Conditioning using an ON/OFF Switch)
				engineSwitch.Value		= "Off";
				ModelAccess.SetModelValue(engineSwitch );

				ignitionSwitch.Value	= "Off";
				ModelAccess.SetModelValue(ignitionSwitch );

				AirCondition.Value		= "Off";
                ModelAccess.SetModelValue(AirCondition );

				Reporting.SetText(2,0,"All aggregates (Engine, AC) switched off",0);
                Reporting.LogExtension("All aggregates (Engine, AC) switched off");

				// Create a Datalogger configuration
                string logfilename = "log_file_" + iterationindex.ToString();
				TypeDLConfigureRecord dlConfiguration = new TypeDLConfigureRecord (
					logfilename ,	// Name of the logfile
					20.0 ,			// Logging duation in seconds
					new TypeDLSignal [ ]	//Array for all signals to be recorded
						{
                            new TypeDLSignal ( "Engine" , ModelAccess, "AcquisitionTask" ),	//Engine Speed has to be recorded in the "Acquisitiontask"
						}
					);
			
				//Configure the datalogger by passing the configuration object to the Configure signature
				ModelAccess.ConfigureDataLogger ( dlConfiguration );

				//Start the measurement mode
				ModelAccess.Start();

				Reporting.SetText(2,0,"Datalogging configured and experiment started",0);
                Reporting.LogExtension("Datalogging configured and experiment started");			

				//Start Ignition and engine
				ignitionSwitch.Value	= "On";
				ModelAccess.SetModelValue(ignitionSwitch );
	
                engineSwitch.Value		= "On";
				ModelAccess.SetModelValue(engineSwitch );
				Thread.Sleep( 5000); 				//Wait for 5second until starter has been successful

				//Switch off starter again.
				engineSwitch.Value	= "Off";
				ModelAccess.SetModelValue(engineSwitch );

				Reporting.SetText(2,0,"Ignition switched on and engine started ",0);
                Reporting.LogExtension("Ignition switched on and engine started ");		
	
				//StartDatalogging
				ModelAccess.StartDataLogger();

                //Wait for the IdleController to be relaxing according to parameterization
                Reporting.SetText(2, 0, "Idle Controller relaxation time : " + m_RelaxationTime.Value.ToString(), 0);
                int waittime = (int)m_RelaxationTime.Value * 1000;
				Thread.Sleep(waittime);

				//Switch on The Air Conditioning
				AirCondition.Value	= "On";
				ModelAccess.SetModelValue(AirCondition );	
				Reporting.SetText(2,0,"Air Condition Switched On",0);
                Reporting.LogExtension("Air Condition Switched On");	

				//Wait for completion of Datalogger
				while ( ModelAccess.GetDataLoggerState() == PortStatusEnum.PortRunning )
				{
					Thread.Sleep ( 500 );
				}

				// Stop Datalogger has to be called in any case, here the file is really stored
                ModelAccess.StopDataLogger ();
				Reporting.SetText(2,0,"Data Acquisition finished",0);
                Reporting.LogExtension("Data Acquisition finished");	

				//Switch Off AirCondition again...
				AirCondition.Value = "Off";
				ModelAccess.SetModelValue ( AirCondition );

				Thread.Sleep( 1000);

                //Stop the measurement again
                ModelAccess.Stop();

				//To read loggedsignals from a file an array of 1D FloatTables needs to be defined collecting the data
				TypeSut1DFloatTable [ ] signals =
					new TypeSut1DFloatTable [ ]  //Create a 1D-Table Array  to contain time and value axes
						{
							new TypeSut1DFloatTable ( "" , "Engine" , "" , 	new double [ ] { 0.0 , 1.0 } , new double [ ] { 0.0 , 2.0 } , 
							0.0 , 0.0 , 0.0 , 0.0 , "" , "" )
						};

				//Get the signals array from the datalogger file  (it may contains several signals
				signals = ModelAccess.GetLoggedSignals ( logfilename , signals );

                // Since only one signal was recorded, the first element of the curve table is the one with the enine speed.
                // The correpsonding Y-Axis contains the recorded values:
                double[] RecordedValues = signals[0].ValueY;


                //Plot recorded Data with the value ranges in the Reporting 
                #region Display result in plot

                //Additional arrays to display the minimum /and maximum values in the plot
                double[] xthresh = new double[] { 0, 20 };
                double[] minthresh = new double[] { minRPM.Value, minRPM.Value };
                double[] maxthresh = new double[] { maxRPM.Value, maxRPM.Value };

                IPlot plot = Reporting.CreatePlot("Plot"+iterationindex.ToString() , "Engine Speed ", 1.3);
                plot.AddYAxis("nEngine", 0.0, 1000.0, "rpm", 100.0);
                plot.SetXAxis("time", 0.0, 20.0, "s", 1.0);

                
                plot.YAxisCollection[0].AddLine("EngineSpeedSignal", signals[0].ValueX, signals[0].ValueY, 0.0, 0.0,
                    plot.CreateLineFormat("red", LineWeight.Thin, LineStyle.Stroke));
                plot.YAxisCollection[0].AddLine("Minimum Allowed", xthresh, minthresh, 0.0, 0.0,
                    plot.CreateLineFormat("red", LineWeight.Thin, LineStyle.Dashed));
                plot.YAxisCollection[0].AddLine("Maximum Allowed", xthresh, maxthresh, 0.0, 0.0,
                    plot.CreateLineFormat("red", LineWeight.Thin, LineStyle.Dashed));

                Reporting.AddPlot2Report(plot);
                #endregion

				//get the min and max value of the recorded signal:
                double min = Double.MaxValue;
                double max = Double.MinValue;
                for (int i = 0; i < RecordedValues.Length; i++)
				{
                    if (RecordedValues[i] < min)
                        min = RecordedValues[i];
                    if (RecordedValues[i] > max)
                        max = RecordedValues[i];
				}
			
				Reporting.SetText(2,0,"Recorded Minimum Engine Speed: " + min.ToString(),0);
				Reporting.SetText(2,0,"Recorded Maximum Engine Speed: " + max.ToString(),0);
			
				//Evaluate, if minimum or maximum values exceed the range:
				if (min < minRPM.Value)
				{
					localVerdict.Fail();
				}
				else if (max > maxRPM.Value)
				{
					localVerdict.Fail();
				}
				else
				{
					localVerdict.Pass();				
				}

				Reporting.SetTableData ( "ResultTable", new string [ ] { ac_torque.Value.ToString() + " " + ac_torque.Unit, minRPM.Value.ToString(), min.ToString ( ), max.ToString ( ), maxRPM.Value.ToString() } );

			}
			catch(Exception ex)
			{
				localVerdict.Error ( );

				Reporting.SetErrorText ( 0, ex.Message, 0 );
				Reporting.LogExtension("Error executing evaluate Function: " + ex.Message);
				throw ex;
			}

			finally
			{
				Reporting.SectionFinished ( "evaluate() - method interation " + iterationindex.ToString() +" executed ", localVerdict );

			}
			return localVerdict;		
		}






	}
}