using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_SignalGenerator
{
    public class TC_SignalGenerator : TestCase
    {
        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_SignalGenerator tc = new TC_SignalGenerator (args);

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
        /// with the name "TC_TC_SignalGenerator"
        /// </summary>
        public TC_SignalGenerator (string[] args)
            : base ( "TC_SignalGenerator", args )
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
            AddMetaData ( "TCD", "almayer" );
            AddMetaData ( "Comment", "" );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        private IPortMA m_maPort = null;

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            m_maPort = Factory.GetPortMA ( "ModelAccess" );
            m_maPort.Timeout = -1;
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
                if ( m_maPort.GetState ( ) != PortStatusEnum.PortConfigured && m_maPort.GetState ( ) != PortStatusEnum.PortToolConfigured )
                {
                    m_maPort.Create ( );
                    m_maPort.ConfigureTool ( "ECU", new string [ ] { "" }, new string [ ] { "", "default", "default", "default" } );
                }

                Reporting.SectionBegin ( "Test Execution" );

                m_maPort.Configure ( new string [ ] { } );

                TypeDLConfigureRecord configureRecord =
                    new TypeDLConfigureRecord (
                        "datalogger",
                        15.0, new TypeDLSignal [ ]
                                  {
                                      new TypeDLSignal ( SutMapping.ENGINE, m_maPort, SutMapping.ACQUISITIONTASK )
                                  } );

                m_maPort.ConfigureDataLogger ( configureRecord );

                m_maPort.ConfigureSignalGenerator (
                    "ACTorqueSignal", // SignalGenerator name <SG .../> from StimulisSetInfo.xml 
                    "pulse" ); // SGSet name <SGSet .../> from StimuliSetInfo.xml, selects .lcs file

                
                m_maPort.SetModelValue ( new TypeSutString ( "", SutMapping.SC_SG, "", "ACTorqueSignal" ) );
                m_maPort.SetModelValue ( new TypeSutString ( "", SutMapping.SC_SGCHANNEL, "", "Torque" ) );
                // channel label inside the lcs file


                // Available modes:
                // 0 = constant
                // 1 = stimuli
                // 2 = model
                // 3 = stim + model
                // 4 = stim * model
                TypeSutFloat mode = new TypeSutFloat("", SutMapping.AC_MODE, "Mode value", 3.0, 0, 0, "");

                m_maPort.SetModelValue(mode);


                m_maPort.Start ( );
                m_maPort.StartDataLogger ( );

                m_maPort.SetModelValue ( new TypeSutInteger ( "starter", SutMapping.STARTENGINE, "", 1, 0, 0, "" ) );
                Thread.Sleep ( 2000 );

                m_maPort.SetModelValue ( new TypeSutInteger ( "starterAc", SutMapping.STARTAIRCONDITION, "", 1, 0, 0, "" ) );
                Thread.Sleep ( 2000 );

                m_maPort.StartSignalGenerator ( "ACTorqueSignal" );

                while ( m_maPort.GetDataLoggerState ( ) == PortStatusEnum.PortRunning )
                {
                    Thread.Sleep ( 500 );
                }

                m_maPort.StopSignalGenerator ( "ACTorqueSignal" );
                m_maPort.StopDataLogger ( );
                m_maPort.Stop ( );

                TypeSut1DFloatTable [ ] signals = new TypeSut1DFloatTable[1];
                signals [ 0 ] = new TypeSut1DFloatTable ( "signal", SutMapping.ENGINE, "", new double [ ] { 0 }, new double [ ] { 0 }, 0, 0, 0, 0, "", "" );

                signals = m_maPort.GetLoggedSignals ( "datalogger", signals );

                Reporting.AddPlot2Report ( PlotHelper.PlotHelper.CreatePlot ( this, signals ) );

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
                Reporting.SectionFinished ( Verdict, new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
            }
        }

        #endregion
    }
}