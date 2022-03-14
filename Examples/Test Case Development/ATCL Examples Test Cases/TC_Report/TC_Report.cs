using System;
using System.Reflection;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_Report
{
    public class TC_Report : TestCase
    {
        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_Report tc = new TC_Report (args);

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
        /// with the name "TC_TC_Report"
        /// </summary>
        public TC_Report (string[] args)
            : base ( "TC_Report", args )
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
            AddMetaData ( "Comment", "This test case shows how to create different report sections and verdicts." );
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
                Initalization ( );
                Stimulation ( );
                Measurement ( );
                Evaluation ( );
            }
            catch ( Exception ex )
            {
                Error ( );
                Reporting.SetErrorText ( 0, ex.Message, 0 );
                throw;
            }
        }

        private void Initalization ( )
        {
            Verdict sectionVerdict = new Verdict ( VerdictCode.None );
            try
            {
                Reporting.SectionBegin ( MethodInfo.GetCurrentMethod ( ).Name );
                sectionVerdict.Pass ( );
                Pass ( );
            }
            catch ( Exception )
            {
                sectionVerdict.Error ( );
                throw;
            }
            finally
            {
                Reporting.SectionFinished ( sectionVerdict.ActualVerdictCode.ToString ( ), sectionVerdict );
            }
        }

        private void Stimulation ( )
        {
            Verdict sectionVerdict = new Verdict ( VerdictCode.None );
            try
            {
                Reporting.SectionBegin ( MethodInfo.GetCurrentMethod ( ).Name );
                sectionVerdict.Pass ( );
                Pass ( );
            }
            catch ( Exception )
            {
                sectionVerdict.Error ( );
                throw;
            }
            finally
            {
                Reporting.SectionFinished ( sectionVerdict.ActualVerdictCode.ToString ( ), sectionVerdict );
            }
        }

        private void Measurement ( )
        {
            Verdict sectionVerdict = new Verdict ( VerdictCode.None );
            try
            {
                Reporting.SectionBegin ( MethodInfo.GetCurrentMethod ( ).Name );
                sectionVerdict.Pass ( );
                Pass ( );
            }
            catch ( Exception )
            {
                sectionVerdict.Error ( );
                throw;
            }
            finally
            {
                Reporting.SectionFinished ( sectionVerdict.ActualVerdictCode.ToString ( ), sectionVerdict );
            }
        }

        private void Evaluation ( )
        {
            Verdict sectionVerdict = new Verdict ( VerdictCode.None );
            try
            {
                Reporting.SectionBegin ( MethodInfo.GetCurrentMethod ( ).Name );
                sectionVerdict.Pass ( );
                Pass ( );
            }
            catch ( Exception )
            {
                sectionVerdict.Error ( );
                throw;
            }
            finally
            {
                Reporting.SectionFinished ( sectionVerdict.ActualVerdictCode.ToString ( ), sectionVerdict );
            }
        }

        #endregion
    }
}