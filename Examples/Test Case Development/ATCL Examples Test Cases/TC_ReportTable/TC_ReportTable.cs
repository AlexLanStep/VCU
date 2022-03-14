using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_ReportTable
{
    public class TC_ReportTable : TestCase
    {
        #region Main Entry Point and Constructor

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">The args.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_ReportTable tc = new TC_ReportTable (args);

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
        /// with the name "TC_TC_ReportTable"
        /// </summary>
        public TC_ReportTable (string[] args)
            : base ( "TC_ReportTable", args )
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
            AddMetaData ( "Comment", "This test case shows how to create tables in a report." );
            AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private static void RegisterPorts ( )
        {
            //TODO: Register any port used in your test case here
        }

        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private void RegisterParameters ( )
        {
            Factory.GetParameterManager ( ).CreateTpaFile ( );
            Factory.GetParameterManager ( ).Save ( );
        }

        /// <summary>
        /// Loads the parameters.
        /// </summary>
        private static void LoadParameters ( )
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

                string tableID = "table_ID";

                Reporting.CreateTable ( tableID, "Value Range Demo Table", 1, 0 );
                Reporting.SetTableHeadline ( tableID, "Measurement", 0, 1 );
                Reporting.SetTableHeadline ( tableID, "Limits", 2, 4 );
                Reporting.SetTableHeadline ( tableID, "Result", 4, 5 );
                Reporting.SetTableColDescription ( tableID,
                                                   new string [ ] { "Label", "Measure Value", "Tolerance", "Min", "Max", "Verdict" } );

                Reporting.SetTableData ( tableID,
                                         new string [ ] { "Angle A", "5.0", "7%", "4.65", "5.35", "pass" } );
                Reporting.SetTableData ( tableID,
                                         new string [ ] { "Angle B", "8.42", "1%", "8.32", "8.48", "pass" } );
                Reporting.SetTableData ( tableID,
                                         new string [ ] { "Angle C", "1.00", "5%", "3.00", "3.30", "fail" } );
                Reporting.SetTableData ( tableID,
                                         new string [ ] { "Angle D", "9.32", "10%", "10.00", "11.00", "fail" } );

                Reporting.AddTableToReport ( tableID );

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