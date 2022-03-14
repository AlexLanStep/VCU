using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_EAMLogging
{
    public class TC_EAMLogging : TestCase
    {
        private IPortEAM m_portEAM = null;

        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_EAMLogging tc = new TC_EAMLogging (args);

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
        /// with the name "TC_TC_EAMLogging"
        /// </summary>
        public TC_EAMLogging (string[] args)
            : base ( "TC_EAMLogging", args )
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
            AddMetaData ( "Comment", "This test case shows how to use the ECU Access datalogger." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
            m_portEAM = Factory.GetPortEAM ( "ECUAccessMeasurement" );
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

                m_portEAM.Configure ( "default", "default", "default" );

                TypeDLSignal ch1 = new TypeDLSignal ( "Channel1", m_portEAM, "defaultTask" );

                TypeDLConfigureRecord configuration = new TypeDLConfigureRecord ( "datalogger", 10.0, new TypeDLSignal [ ] { ch1 } );

                m_portEAM.ConfigureDataLogger ( configuration );
                m_portEAM.Start ( );
                m_portEAM.StartDataLogger ( );

                Thread.Sleep ( 11000 );

                m_portEAM.StopDataLogger ( );
                m_portEAM.Stop ( );

                #region Display results in plot

                TypeSut1DFloatTable [ ] data = m_portEAM.GetLoggedSignals ( "datalogger", new TypeSut1DFloatTable [ ]
                                                                                              {
                                                                                                  new TypeSut1DFloatTable ( "table", ch1.SignalName, "",
                                                                                                                            new double[0], new double[0], 0.0,
                                                                                                                            0.0, 0.0, 0.0, "", "" )
                                                                                              } );

                IPlot plot = PlotHelper.PlotHelper.CreatePlot ( this, data );

                Reporting.AddPlot2Report ( plot );

                #endregion

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
                m_portEAM.RemoveAllSignals ( );
                Reporting.SectionFinished ( Verdict, new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
            }
        }

        #endregion
    }
}