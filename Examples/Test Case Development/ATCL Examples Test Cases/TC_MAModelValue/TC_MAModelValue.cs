using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_MAModelValue
{
    public class TC_MAModelValue : TestCase
    {
        private IPortMA maPort = null;

        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_MAModelValue tc = new TC_MAModelValue (args);

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
        /// with the name "TC_TC_MAModelValue"
        /// </summary>
        public TC_MAModelValue (string[] args)
            : base ( "TC_MAModelValue", args )
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
            AddMetaData ( "Comment", "This test case shows how to set and get values at the MA port." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
            maPort = Factory.GetPortMA ( "ModelAccess" );
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

                if ( maPort.GetState ( ) != PortStatusEnum.PortConfigured )
                {
                    maPort.Configure ( new string[0] );
                }

                maPort.Start ( );

                // this is the value we will set/get and check
                TypeSutFloat starterValue = new TypeSutFloat ( "ModelValue", "Starter", "The starter which starts the engine", 1.0, 0.0, 1.0, "" );

                // Get the value - should be 0.0
                TypeSutFloat beforeSet = ( TypeSutFloat ) maPort.GetModelValue ( starterValue );
                if ( beforeSet.Value != 0.0 )
                {
                    Reporting.SetErrorText ( 0, string.Format ( "Expected value '0.0' but was '{0}'", beforeSet.Value ), 0 );
                    Fail ( );
                    return;
                }

                // set the value to 1.0
                maPort.SetModelValue ( starterValue );

                // get the value again and check if it is 1.0 now
                TypeSutFloat afterSet = ( TypeSutFloat ) maPort.GetModelValue ( starterValue );
                if ( afterSet.Value != 1.0 )
                {
                    Reporting.SetErrorText ( 0, string.Format ( "Expected value '1.0' but was '{0}'", afterSet.Value ), 0 );
                    Fail ( );
                    return;
                }

                maPort.Stop ( );

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