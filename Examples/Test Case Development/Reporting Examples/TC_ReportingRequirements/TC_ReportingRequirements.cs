using System;
using System.IO;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_ReportingRequirements
{
	public class TC_ReportingRequirements : TestCase
	{
		private TypeSutBool m_showLargeTable =
			new TypeSutBool ( "ShowLargeTable", "", "Defines wether the large table is shown.", true );

		#region Main Entry Point and Constructor

		/// <summary>
		/// Main entry point of this test case
		/// </summary>
		/// <param name="args">The args.</param>
		public static void Main ( string [ ] args )
		{
			// create a new instance of the test case class
			TC_ReportingRequirements tc = new TC_ReportingRequirements ( );

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
				tc.Reporting.LogExtension ( string.Format ( "Exception occured in Test Case: {0}", ex.Message ) );
				tc.Reporting.SetErrorText ( 0, ex.Message, 0 );
			}
			finally
			{
				tc.Finished ( );
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TestCase"/> class
		/// with the name "TC_TC_ReportingRequirements"
		/// </summary>
		public TC_ReportingRequirements ( )
			: base ( "TC_ReportingRequirements" )
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
		private void Init ( )
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
				Reporting.SetErrorText ( 0,
				                         string.Format ( "Execption during Test Case initialization! Exception message is {0}",
				                                         ex.Message ), 0 );
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
			AddMetaData ( "TCD", "beschnai" );
			AddMetaData ( "Comment", "" );
			AddMetaData ( "Version", GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );

			//TODO: Add any additional meta data here
		}

		/// <summary>
		/// Registers the ports.
		/// </summary>
		private void RegisterPorts ( )
		{
		}

		/// <summary>
		/// Registers the parameters.
		/// </summary>
		private void RegisterParameters ( )
		{
			Factory.GetParameterManager ( ).CreateTpaFile ( );

			Factory.GetParameterManager ( ).Register ( m_showLargeTable );

			Factory.GetParameterManager ( ).Save ( );
		}

		/// <summary>
		/// Loads the parameters.
		/// </summary>
		private void LoadParameters ( )
		{
			m_showLargeTable = ( TypeSutBool ) Factory.GetParameterManager ( ).Parameterise ( m_showLargeTable );
		}

		#endregion

		#region Test Case implementation

		private void PerformTest ( )
		{
			try
			{
                Reporting.SectionAbstractBegin();
                Reporting.SetInfoText(0, "TEST in TC Abstract.", 0);
                Reporting.SectionAbstractFinished();

				Reporting.SectionBegin ( "Test Execution" );
                Reporting.AddPageBreak();

				// Line Feed in Section Test Result.
				Reporting_WHAT25926 ( );
                Reporting.AddPageBreak();

				// Report Table Width Sizes.
				Reporting_WHAT25936 ( );
                Reporting.AddPageBreak();

				// User relative URLs in Test Report Viewer.
				Reporting_WHAT25934 ( );
                Reporting.AddPageBreak();
				Reporting_WHAT25934_WithPlot ( );
                
                Reporting_SectionAbstract ( );

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

                Reporting.SectionAbstractBegin();
                Reporting.SetInfoText(0, "TEST in TC Abstract 2.", 0);
                Reporting.SectionAbstractFinished();
			}
		}

		/// <summary>
		/// Use relative URLs in the Test Report Viewer.
		/// </summary>
		private void Reporting_WHAT25934 ( )
		{
			Reporting.SectionBegin ( "WHAT25934 - Relative URLs in TRV" );
			Reporting.SetText ( 2, 0,
			                    "A text file 'Test.txt' is created in the 'ProcessInfo' folder. Then a link on this file is created.",
			                    0 );
			string link =
				Path.Combine (
					Path.Combine ( Path.Combine ( Factory.GetReportManager ( ).ReportPath, "ProcessInfo" ),
					               Factory.GetReportManager ( ).TCReportPath ), "Test.txt" );
			StreamWriter fs = File.CreateText ( link );
			fs.WriteLine ( "This is only for test!" );
			fs.Flush ( );
			fs.Close ( );

			Reporting.SetLink ( 2, 1, "The relative Link on 'Test.txt'!",
			                    @"..\..\ProcessInfo\" + Factory.GetReportManager ( ).TCReportPath + @"\Test.txt", 1 );

			Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );
		}

		/// <summary>
		/// Use relative URLs in the Test Report Viewer
		/// </summary>
		private void Reporting_WHAT25934_WithPlot ( )
		{
			Reporting.SectionBegin ( "WHAT25934 - Relative URLs in TRV" );
			Reporting.SetText ( 2, 0,
			                    "A plot 'MyPlot' is created in the 'ProcessInfo' folder. Then a link on the plot is created.", 0 );

			string plotName = "MyPlot";
			IPlot plot = Reporting.CreatePlot ( plotName, "Plot title", 1.0 );
			plot.SetXAxis ( "X-Axis", -1, 10, plot.CreateAxisFormat ( "m", 1 ) );
			IYAxis yAxis = plot.AddYAxis ( "Y-Axis", -1, 10, plot.CreateAxisFormat ( "h", 1 ) );
			yAxis.AddLine ( "line", new double [ ] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new double [ ] { 2, 3, 2, 3, 2, 3, 2, 3, 2 }, 0,
			                0 );

			Reporting.AddPlot2Report ( plot, 1 );

			Reporting.SetLink ( 2, 1, "The relative link on 'MyPlot.svg'!", plotName + ".svg", 1 );

			Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );
		}

		/// <summary>
		/// Report Table Width sizes.
		/// Two tables are created with each 10 columns but different contents, so that the tables have different sizes.
		/// </summary>
		private void Reporting_WHAT25936 ( )
		{
			Reporting.SectionBegin ( "WHAT25936 - Report Table Width sizes" );

			Reporting.SetText ( 2, 1, "First the large table is written.", 1 );

			if ( m_showLargeTable.Value )
			{
				Reporting.CreateTable ( "Table1", "Large Table", 1, 1 );
				Reporting.SetTableHeadline ( "Table1", "mmmmmmmmmm", 1, 2 );
				Reporting.SetTableHeadline ( "Table1", "xxxxxxxxxxx", 3, 5 );
				Reporting.SetTableColDescription ( "Table1", new string [ ]
				                                             	{
				                                             		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
				                                             		"mmmmmmmmmm", "mmmmmmmmmm",
				                                             		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
				                                             		"mmmmmmmmmm", "mmmmmmmmmm"
				                                             	} );
				Reporting.SetTableData ( "Table1", new string [ ]
				                                   	{
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm"
				                                   	} );
				Reporting.SetTableData ( "Table1", new string [ ]
				                                   	{
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
				                                   		"mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm"
				                                   	} );
				Reporting.AddTableToReport ( "Table1" );
			}
			else
			{
				Reporting.SetText ( 1, 1, "The large Table is disabled!", 1 );
			}

			Reporting.SetText ( 2, 1, "Then the small table is written.", 1 );

			Reporting.CreateTable ( "Table2", "Small Table", 1, 1 );
			Reporting.SetTableHeadline ( "Table2", "mmmmmmmmmm", 1, 2 );
			Reporting.SetTableColDescription ( "Table2", new string [ ]
			                                             	{
			                                             		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                             		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm"
			                                             	} );
			Reporting.SetTableData ( "Table2", new string [ ]
			                                   	{
			                                   		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		"mmmmmmmmmm", "mmmmmmmmmm"
			                                   	} );
			Reporting.SetTableData ( "Table2", new string [ ]
			                                   	{
			                                   		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		"mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		"mmmmmmmmmm", "mmmmmmmmmm"
			                                   	} );
			Reporting.AddTableToReport ( "Table2" );

			Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );
		}

		/// <summary>
		/// Line Feed in Section Test result.
		/// A section is created with a line break in the section test result.
		/// A text (error) is written, with a linebreak.
		/// 
		/// Expected Results:
		/// - The line break in the Section Test Result shall be visible in the Report.
		/// - The line break in the Section Test Result shall be visible in the Main Section overview (Section Verdict must be different from Pass).
		/// - The line break in the Text (SetText) shall be visible in the Report.
		/// - The line break in the Text shall be visible in the Main if the Text is an Error or Warning.
		/// </summary>
		private void Reporting_WHAT25926 ( )
		{
			Reporting.SectionBegin ( "WHAT25926 - Line Feed in Section Test Result" );

			Reporting.SetText ( 0, 0, "An error with line breaks!\nThis is the second line!\nAnd this is the third line.", 0 );
			Reporting.SetText ( 1, 0, "A warning with line breaks!\nThis is the second line!\nAnd this is the third line.", 0 );

			Reporting.SectionFinished ( "First line in the Result\nSecond line in the result", new Verdict ( VerdictCode.Fail ) );
		}


        private void Reporting_SectionAbstract()
        {
            Reporting.SectionBegin ( "Test the section abstract." );

                Reporting.SectionAbstractBegin (  );
                #region abstract content
                Reporting.SetInfoText ( 0, "Some text in the abstract!", 0 );
                #endregion
                Reporting.SectionAbstractFinished (  );

                Reporting.SetInfoText ( 0, "Some text that is not in the abstract!", 0 );

                Reporting.SectionAbstractBegin (  );
                #region  abstract content
                Reporting.CreateTable("Table2", "Small Table", 1, 1);
                Reporting.SetTableHeadline("Table2", "mmmmmmmmmm", 1, 2);
                Reporting.SetTableColDescription("Table2", new string[]
			                                             	    {
			                                             		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                             		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm"
			                                             	    });
                Reporting.SetTableData("Table2", new string[]
			                                   	    {
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm"
			                                   	    });
                Reporting.SetTableData("Table2", new string[]
			                                   	    {
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm", "mmmmmmmmmm",
			                                   		    "mmmmmmmmmm", "mmmmmmmmmm"
			                                   	    });
                Reporting.AddTableToReport("Table2");
                #endregion
                Reporting.SectionAbstractFinished (  );

            Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );



            Reporting.SectionBegin ( "Test " );
            Reporting.SetInfoText ( 0, "Text outside of the abstract.", 0 );

            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "TEST", 0 );
            Reporting.SectionAbstractFinished();
            Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );
            


            Reporting.SectionBegin ( "Test mit vielen Section Abstracts in der Section und Text dazwischen.!" );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Text in 1. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Text nach 1. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Text in 2. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Text nach 2. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Text in 3. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Text nach 3. Abstract Abschnitt.", 0 );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Text in 4. Abstract Abschnitt.", 0 );
            string plotName = "MyPlot";
            IPlot plot = Reporting.CreatePlot(plotName, "Plot title", 1.0);
            plot.SetXAxis("X-Axis", -1, 10, plot.CreateAxisFormat("m", 1));
            IYAxis yAxis = plot.AddYAxis("Y-Axis", -1, 10, plot.CreateAxisFormat("h", 1));
            yAxis.AddLine("line", new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 }, new double[] { 2, 3, 2, 3, 2, 3, 2, 3, 2 }, 0,
                            0);
            Reporting.AddPlot2Report(plot, 1);
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Text nach 4. Abstract Abschnitt.", 0 );
            Reporting.SectionFinished ( "", new Verdict ( VerdictCode.Pass ) );


            Reporting.FunctionCalled ( "F_MyFunction", "MyModule", new []{"par1", "par2"} );
            Reporting.SetInfoText ( 0, "Text in Function before Abstract", 0 );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Text in the function abstract.", 0 );
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Text in Function after abstract.", 0 );
            Reporting.FunctionFinished("F_MyFunction", "MyModule", new[] { "par1", "par2" });


            Reporting.TestEntityCalled ( "TE_MyEntity", "AnotherModule", "now", new[]{"par1", "par2"} );
            Reporting.SetInfoText ( 0, "Some text in the entity before the abstract.", 0 );
            Reporting.SectionAbstractBegin (  );
            Reporting.SetInfoText ( 0, "Some text in the entity abstract.", 0 );
            Reporting.SectionAbstractFinished (  );
            Reporting.SetInfoText ( 0, "Some text in the entity after the abstract.", 0 );
            Reporting.TestEntityFinished("TE_MyEntity", "AnotherModule", new Verdict(VerdictCode.Pass), "errorLevel", "someExecutionTime", new[] { "par1", "par2" });


            //Reporting.SectionBegin("Test2 invalid begin sectionabstract and finish it in the section");

            //Reporting.SectionAbstractBegin();
            //Reporting.SectionBegin("Begin");
            //Reporting.SetInfoText(0, "Some text!.", 0);
            //Reporting.SectionAbstractFinished();
            //Reporting.SectionFinished("", new Verdict(VerdictCode.Pass));

            //Reporting.SectionFinished("", new Verdict(VerdictCode.Pass));
        }

	    #endregion
	}
}
