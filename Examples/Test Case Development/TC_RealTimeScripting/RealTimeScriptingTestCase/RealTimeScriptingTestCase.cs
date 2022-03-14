//
// This source code was auto-generated 
//

using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace ETAS.TestCaseTemplate
{
	public class RealTimeScriptingTestCase:TestCase
	{
        public string[] PluginFiles { get; set; }

		public static void Main( string[] args ) 
		{
			RealTimeScriptingTestCase tc = new RealTimeScriptingTestCase (args);
			try
			{
				tc.Init();
				tc.PerformTestCase();
			}
			catch (Exception ex)
			{
				tc.Error();
				tc.Reporting.LogExtension(string.Format("Exception occured in Test Case: {0}", ex.Message));
				tc.Reporting.SetErrorText(0, ex.Message, 0);
			}
			finally 
			{
				tc.Finished();
			}
		}
		
        public RealTimeScriptingTestCase(string[] args) : this()
        {
            PluginFiles = args;
            if (args.Length == 0)
                PluginFiles = new string[] {"" };
        }

        public  RealTimeScriptingTestCase(  )  : base("RealTimeScriptingTestCase")
		{
            PluginFiles = new string[] { "" };
		}

		private void RegisterMetaData(  ) 
		{
            // these meta data are written to the Test Hierarchy Desccription File (THD)
            // will be seen as meta data in test manager

			//AddMetaData("TCD", "User");
			AddMetaData("Comment", "Empty RealTimeScripting test case.");
            AddMetaData("Purpose", "Test RealTimeScripting port-adapter-tool chain.");
			AddMetaData("Version", GetType().Assembly.GetName().Version.ToString());
		}
		private void Init(  ) 
		{
			RegisterMetaData();
			Parameters.Init(this);
			Ports.Init(this);
		}
		private void PerformTestCase(  ) 
		{
			try
			{
				Reporting.SectionBegin("Test Execution");
#if DEBUG
                Ports.MAPort.Create ( );
                Ports.MAPort.ConfigureTool ( "RealTimeTest", new string[] { "" }, new string[] { "", "default", "default", "default" } );
#endif
                Ports.MAPort.Configure ( new string[0] );

#if DEBUG
                Ports.RealTimeScripting.Create ( );
                Ports.RealTimeScripting.ConfigureTool("", new string[] { "" }, new string[] { "", "default", "default", "default" });
#endif

                if (Ports.RealTimeScripting.GetState ( ) != PortStatusEnum.PortConfigured)
                {
                    Ports.RealTimeScripting.Configure ( );
                }

                State state;
			    string ret;
                ret = Ports.RealTimeScripting.AddFiles(Parameters.PluginFilesArray.Value); // pass initial set of C files to Activate
                state = Ports.RealTimeScripting.GetRTPCState ( );
                
                Reporting.SetInfoText ( 2, state.Info, 0 );
                Reporting.SetInfoText ( 2, ret, 0 );


                ret = Ports.RealTimeScripting.Activate();
                state = Ports.RealTimeScripting.GetRTPCState (  );
                
                Reporting.SetInfoText ( 2, state.Info, 0 );
                Reporting.SetInfoText ( 2, ret, 0 );
                

                logData ( );
			    
                ret = Ports.RealTimeScripting.Deactivate();
                state = Ports.RealTimeScripting.GetRTPCState ( );
                
                Reporting.SetInfoText ( 2, state.Info, 0 );
                Reporting.SetInfoText ( 2, ret, 0 );
                
#if DEBUG
                Ports.RealTimeScripting.Close ( );
			    Ports.MAPort.Close ( );
#endif
            }
			catch (Exception ex)
			{
				Error();
				Reporting.SetErrorText(0, ex.Message, 0);
				throw;
			}
			finally
			{
				Reporting.SectionFinished(Verdict, new Verdict(Factory.GetVerdictManager().GetVerdictOfTestCase()));
			}
		}

        private void logData ()
        {
            TypeDLSignal outSignal = new TypeDLSignal ( "Out", Ports.MAPort, "AcquisitionTask" );

            TypeDLConfigureRecord configureDL = new TypeDLConfigureRecord ( "datalogger", 10.0, new TypeDLSignal[] { outSignal } );

            Ports.MAPort.ConfigureDataLogger ( configureDL );
            Ports.MAPort.Start ( );
            Ports.MAPort.StartDataLogger ( );

            Thread.Sleep ( 11000 );

            Ports.MAPort.StopDataLogger ( );
            //Ports.MAPort.Stop ( );

            #region Display result in plot

            TypeSut1DFloatTable[] table =
                Ports.MAPort.GetLoggedSignals ( "datalogger",
                                            new TypeSut1DFloatTable[]
                                                    {
                                                        new TypeSut1DFloatTable ( "Out", "Out", "", new double[0], new double[0], 0.0, 0.0, 0.0, 0.0, "",
                                                                                  "" )
                                                    } );

            if (table.Length>0)
            {
                if (table[0].ValueY[0]== 0 && table[0].ValueY[1]==0)
                {
                    Fail ( );
                    Reporting.SetErrorText ( 0,"Plugin not loaded!",0 );
                }
                else
                {
                    Pass ( );
                }
            }

            IPlot plot = Reporting.CreatePlot ( "Plot", "Out Plot", 1.0 );

            plot.AddYAxis ( "y", -5.0, 5.0, "rpm", 100.0 );

            plot.SetXAxis ( "x", 0.0, 11.0, "s", 1.0 );

            plot.YAxisCollection[0].AddLine ( "EngineSignal", table[0].ValueX, table[0].ValueY, 0.0, 0.0,
                                                 plot.CreateLineFormat ( "green", LineWeight.Thin, LineStyle.Stroke ) );

            Reporting.AddPlot2Report ( plot );

            #endregion
        }
	}
}
