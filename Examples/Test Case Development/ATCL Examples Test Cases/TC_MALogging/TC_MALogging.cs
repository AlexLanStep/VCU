using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_MALogging
{
    public class TC_MALogging : TestCase
    {
        private IPortMA m_maPort = null;

        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_MALogging tc = new TC_MALogging (args);

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
        /// with the name "TC_TC_MALogging"
        /// </summary>
        public TC_MALogging (string[] args)
            : base ( "TC_MALogging", args )
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
            AddMetaData ( "Comment", "This test case shows how to use the model datalogger." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
            m_maPort = Factory.GetPortMA ( "ModelAccess" );
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

                m_maPort.Configure ( new string [ ] { } );

                TypeDLSignal engine = new TypeDLSignal ( "Engine", m_maPort, "AcquisitionTask" );

                TypeDLConfigureRecord configureDL = new TypeDLConfigureRecord ( "datalogger", 10.0, new TypeDLSignal [ ] { engine } );

                m_maPort.ConfigureDataLogger ( configureDL );
                m_maPort.Start ( );
                m_maPort.StartDataLogger ( );

                Thread.Sleep ( 500 );

                m_maPort.SetModelValue ( new TypeSutFloat ( "Starter", "Starter", "", 1.0, 0.0, 0.0, "" ) );

                TypeSutBase starter =
                    m_maPort.GetModelValue ( new TypeSutFloat ( "Starter", "Starter", "", 1.0, 0.0, 0.0, "" ) );
                if ( ( ( TypeSutFloat ) starter ).Value != 1.0 )
                {
                    Reporting.SetErrorText ( 0,
                                             string.Format ( "The value of \"Starter\" was not 1.0 but {0}",
                                                             ( ( TypeSutFloat ) starter ).Value ), 0 );
                }

                Thread.Sleep ( 11000 );

                m_maPort.StopDataLogger ( );
                m_maPort.Stop ( );

                #region Display result in plot 

                TypeSut1DFloatTable [ ] table =
                    m_maPort.GetLoggedSignals ( "datalogger",
                                                new TypeSut1DFloatTable [ ]
                                                    {
                                                        new TypeSut1DFloatTable ( "Engine", "Engine", "", new double[0], new double[0], 0.0, 0.0, 0.0, 0.0, "",
                                                                                  "" )
                                                    } );

                IPlot plot = Reporting.CreatePlot ( "Plot", "Engine Plot", 1.0 );

                plot.AddYAxis ( "y", 0.0, 1000.0, "rpm", 100.0 );

                plot.SetXAxis ( "x", 0.0, 11.0, "s", 1.0 );

                plot.YAxisCollection [ 0 ].AddLine ( "EngineSignal", table [ 0 ].ValueX, table [ 0 ].ValueY, 0.0, 0.0,
                                                     plot.CreateLineFormat ( "green", LineWeight.Thin, LineStyle.Stroke ) );

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
                Reporting.SectionFinished ( Verdict, new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
            }
        }

        #endregion
    }
}