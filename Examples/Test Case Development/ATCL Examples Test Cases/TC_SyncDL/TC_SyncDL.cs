using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_SyncDL
{
    public class TC_SyncDL : TestCase
    {
        private IPortSyncDL syncDL = null;
        private IPortMA maPort = null;
        private IPortEAM eamPort = null;
        private IPortEAM eamPort2 = null;

        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_SyncDL tc = new TC_SyncDL (args);

            try
            {
                tc.Init ( );
                tc.PerformTest ( );
            }
            catch ( Exception ex )
            {
                // On exception, set the test case verdict to error, display the error
                // message in the Test Handler and the report.
                tc.Error ( );
                tc.Reporting.LogExtension ( string.Format ( "Exception occurred in Test Case: {0}", ex.Message ) );
                tc.Reporting.SetErrorText ( 0, ex.Message, 0 );
            }
            finally
            {
                tc.Finished ( );
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCase"/> class
        /// with the name "TC_TC_SyncDL"
        /// </summary>
        public TC_SyncDL (string[] args)
            : base ( "TC_SyncDL", args )
        {
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes this <see cref="TestCase"/>.
        /// </summary>
        /// <remarks>
        /// Initialization includes registering metadata, ports and parameters.
        /// Additionally all parameters which are registered are loaded.
        /// </remarks>
        protected override void Init ( )
        {
            try
            {
                base.Init();
                RegisterMetaData ( );
                RegisterPorts ( );
                RegisterParameters ( );
                LoadParameters ( );
            }
            catch ( Exception ex )
            {
                Reporting.SetErrorText ( 0, string.Format ( "Execption during Test Case initialization! Exception message is {0}", ex.Message ), 0 );
                Error ( );
                throw ( ex );
            }
        }

        /// <summary>
        /// Registers the meta data.
        /// </summary>
        /// <remarks>The version attribute is read from the assembly version which
        /// is defined in the AssemblyInfo.cs file</remarks>
        private void RegisterMetaData ( )
        {
            AddMetaData ( "TCD", "Alexander Mayer" );
            AddMetaData ( "Comment", "This test case shows how to use the synchronous datalogger." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            syncDL = Factory.GetPortSyncDL ( "SyncDL" );
            syncDL.Timeout = -1;

            maPort = Factory.GetPortMA ( "ModelAccess" );
            maPort.Timeout = -1;

            eamPort = Factory.GetPortEAM ( "ECUAccessMeasurement" );
            eamPort.Timeout = -1;

            eamPort2 = Factory.GetPortEAM ( "ECUAccessMeasurement2" );
            eamPort2.Timeout = -1;
        }

        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private void RegisterParameters ( )
        {
            Factory.GetParameterManager ( ).CreateTpaFile ( );

            //TODO: Register any parameters which should be exported to tpa here

            Factory.GetParameterManager ( ).Save ( );
        }

        /// <summary>
        /// Loads the parameters.
        /// </summary>
        private void LoadParameters ( )
        {
            //TODO: Retreive the parameterized values from your tpa file here
        }

        #endregion

        #region Test Case implementation

        private void PerformTest ( )
        {
            try
            {
                Reporting.SectionBegin ( "Test Execution" );

                maPort.Configure(new string[] { "", "" });
                eamPort.Configure("default", "default", "default");
                eamPort2.Configure ( "device2", "default", "default" );
                syncDL.Configure ( new string [ ] { "ECUAccessMeasurement", "ECUAccessMeasurement2", "ModelAccess" } );

                eamPort.RemoveAllSignals ( );
                eamPort2.RemoveAllSignals ( );

                TypeDLSignal signalChannelOne = new TypeDLSignal ( "Channel1", eamPort, "defaultTask" );
                TypeDLSignal signalChannelTwo = new TypeDLSignal ( "Channel2", eamPort, "defaultTask" );
                TypeDLSignal signalOutput = new TypeDLSignal ( "Output", eamPort2, "ETKTask" );
                TypeDLSignal signalEngine = new TypeDLSignal ( "Engine", maPort, "AcquisitionTask" );

                TypeDLTrigger starttrigger = new TypeDLValueTrigger ( signalChannelTwo, ">", -19.0 );
                TypeDLTrigger stoptrigger = new TypeDLValueTrigger ( signalChannelOne, ">", -14.0 );

                TypeSyncDLConfigureRecord configuration = new TypeSyncDLConfigureRecord
                    ( "datalogger", eamPort, 10.0,
                      new TypeDLSignal [ ] { signalChannelOne, signalChannelTwo, signalEngine, signalOutput },
                      new TypeDLTriggerCollection (
                          starttrigger,
                          stoptrigger ) );

                syncDL.ConfigureDataLogger ( configuration );

                eamPort.Start ( );
                eamPort2.Start ( );
                maPort.Start ( );
                syncDL.Start ( );

                maPort.SetModelValue ( new TypeSutFloat ( "Starter", "Starter", "", 1.0, 0.0, 0.0, "" ) );

                while ( syncDL.GetState ( ) == PortStatusEnum.PortRunning )
                {
                    Thread.Sleep ( 1000 );
                }

                syncDL.Stop ( );
                eamPort.Stop ( );
                eamPort2.Stop ( );
                maPort.Stop ( );

                TypeSyncDLLoggedSignalsRecord [ ] loggedSignals =
                    syncDL.GetLoggedSignals ( "datalogger", new TypeDLSignal [ ] { signalChannelOne, signalChannelTwo, signalEngine, signalOutput } );

                IPlot plot = Reporting.CreatePlot ( "Plot", "Title", 1.0 );

                plot.SetXAxis ( "s", 0.0, 10.0, "s", 1 );
                plot.AddYAxis ( "y", -25.0, 10.0, "", 5 );
                plot.AddYAxis ( "Engine", 0.0, 1000.0, "rpm", 100 );
                plot.YAxisCollection [ 0 ].AddLine ( "channel1", loggedSignals [ 0 ].TimeStamps, loggedSignals [ 0 ].Values, 0.0, 0.0,
                                                     plot.CreateLineFormat ( "red", LineWeight.Thin, LineStyle.Stroke ) );
                plot.YAxisCollection [ 0 ].AddLine ( "channel2", loggedSignals [ 1 ].TimeStamps, loggedSignals [ 1 ].Values, 0.0, 0.0,
                                                     plot.CreateLineFormat ( "blue", LineWeight.Thin, LineStyle.Stroke ) );
                plot.YAxisCollection [ 1 ].AddLine ( "Engine", loggedSignals [ 2 ].TimeStamps, loggedSignals [ 2 ].Values, 0.0, 0.0,
                                                     plot.CreateLineFormat ( "green", LineWeight.Thin, LineStyle.Stroke ) );
                plot.YAxisCollection [ 1 ].AddLine ( "Output", loggedSignals [ 3 ].TimeStamps, loggedSignals [ 3 ].Values, 0.0, 0.0,
                                                     plot.CreateLineFormat ( "yellow", LineWeight.Thin, LineStyle.Stroke ) );

                Reporting.AddPlot2Report ( plot );

                Pass ( );
            }
            catch ( Exception ex )
            {
                Error ( );
                Reporting.SetErrorText ( 0, ex.Message, 0 );
                throw;
            }
            finally
            {
                eamPort.RemoveAllSignals ( );
                eamPort2.RemoveAllSignals ( );
                Reporting.SectionFinished ( Verdict, new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
            }
        }

        #endregion
    }
}