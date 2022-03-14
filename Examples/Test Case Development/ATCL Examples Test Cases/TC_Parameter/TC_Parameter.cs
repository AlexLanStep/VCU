using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_Parameter
{
    public class TC_Parameter : TestCase
    {
        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_Parameter tc = new TC_Parameter (args);

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
        /// with the name "TC_TC_Parameter"
        /// </summary>
        public TC_Parameter (string[] args)
            : base ( "TC_Parameter", args )
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
            AddMetaData ( "Comment", "This test case shows how to publish and load test case parameters." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
        }

        private TypeSutUserRecord ur1 = new TypeSutUserRecord ( "User Record 1", "Fails when FLT is greater than 10.0" );
        private TypeSutUserRecord ur2 = new TypeSutUserRecord ( "User Record 2", "Fails when FLT is less than 10.0" );

        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private void RegisterParameters ( )
        {
            Factory.GetParameterManager ( ).CreateTpaFile ( );

            TypeSutFloat float1 = new TypeSutFloat ( "Float", "FLT", "A float value", 1.0, 0.0, 100.0, "s" );

            ur1.Items.Add ( float1 );
            Factory.GetParameterManager ( ).Register ( ur1 );

            ur2.Items.Add ( float1 );
            Factory.GetParameterManager ( ).Register ( ur2 );

            //TODO: Register any parameters which should be exported to tpa here

            Factory.GetParameterManager ( ).Save ( );
        }

        /// <summary>
        /// Loads the parameters.
        /// </summary>
        private void LoadParameters ( )
        {
            ur1 = ( TypeSutUserRecord ) Factory.GetParameterManager ( ).Parameterise ( ur1 );
            ur2 = ( TypeSutUserRecord ) Factory.GetParameterManager ( ).Parameterise ( ur2 );
        }

        #endregion

        #region Test Case implementation

        private void PerformTest ( )
        {
            try
            {
                Reporting.SectionBegin ( "Test Execution" );

                if ( ( ( TypeSutFloat ) ur1.Items [ 0 ] ).Value > 10.0 )
                {
                    Reporting.SetErrorText(0, "The value of the parameter 'FLT' in the 'User Record 1' must be less than 10!", 0);
                    Fail ( );
                }

                if ( ( ( TypeSutFloat ) ur2.Items [ 0 ] ).Value < 10.0 )
                {
                    Reporting.SetErrorText(0, "The value of the parameter 'FLT' in the 'User Record 2' must be greater than 10!", 0);
                    Fail ( );
                }

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