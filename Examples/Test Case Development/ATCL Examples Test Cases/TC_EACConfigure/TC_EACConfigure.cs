using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;

namespace TC_EACConfigure
{
    public class TC_EACConfigure : TestCase
    {
        private IPortEAC m_portEAC = null;
        private IPortMA m_portMA = null;

        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_EACConfigure tc = new TC_EACConfigure (args);

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
        /// with the name "TC_TC_EASetValue"
        /// </summary>
        public TC_EACConfigure (string[] args)
            : base ( "TC_EACConfigure", args )
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
                throw;
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
            AddMetaData ( "Comment", "This test case shows how to configure the ECU access port." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
            m_portEAC = Factory.GetPortEAC ( "ECUAccessCalibration" );
            m_portMA = Factory.GetPortMA ( "ModelAccess" );
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
                if ( !m_portMA.IsToolConfigured )
                {
                    m_portMA.Create ( );
                    m_portMA.ConfigureTool ( "ECU", new string [ ] { "" },
                                             new string [ ] { "", "default", "default", "default" } );
                }
                m_portMA.Configure ( new string[0] );
                
                bool createAndToolConfigureInTC = false;
                if (!m_portEAC.IsToolConfigured)
                {
                    m_portEAC.Create ( );
                    m_portEAC.ConfigureTool ( "ECU", new string [ ] { "" }, new string [ ] { "", "default" } );
                    createAndToolConfigureInTC = true;
                }
                m_portEAC.Configure ( "default", "default", "default" );

                Pass ( );

                if (createAndToolConfigureInTC)
                {
                    m_portEAC.Close ( );
                }
            }
            catch
            {
                Error ( );
                throw;
            }
        }

        #endregion
    }
}