using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_ReportingPlotTest
{
	public class TC_ReportingPlotTest : TestCase
	{
		#region private data ...

		#region parameters ...

		private static TypeSutBase [ ] m_lineFormat = new TypeSutBase[3]
			{
				new TypeSutString ( "Color", "", "The color of the line", "red", "", "" ),
				new TypeSutCollectionUserRecord ( "LineWeight", "The weight of the line",
				                                  new TypeSutStringArray ( "LineWeightCollection", "", "",
				                                                           new string[3] { "Thin", "Normal", "Bold" }, "", "" ),
				                                  new TypeSutString ( "SelectedLineWeight", "", "", "Normal", "", "" ) ),
				new TypeSutCollectionUserRecord ( "LineStyle", "The style of the line",
				                                  new TypeSutStringArray ( "LineStyleCollection", "", "",
				                                                           new string[3] { "Stroke", "Dashed", "Dotted" }, "", "" ),
				                                  new TypeSutString ( "SelectedLineStyle", "", "", "Stroke", "", "" ) ),
			};

		private static TypeSutBase [ ] m_lineProperties = new TypeSutBase[6]
			{
				new TypeSutString ( "LineID", "", "The ID of the line", "UpperBound", "", "" ),
				new TypeSutFloatArray ( "XValues", "", "The x-values of the line (must have the same length as YValues)",
				                        new double[3] { 0.01, 2500.01, 5000.01 }, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloatArray ( "YValues", "", "The y-values of the line (must have the same length as XValues)",
				                        new double[3] { 32.7, 85.3, 90.5 }, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "XShift", "", "Shift the values on the x-axis", 0.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "YShift", "", "Shift the values on the y-axis", 0.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutUserRecord ( "LineFormat", "Format of the line", m_lineFormat )
			};

		private static TypeSutBase [ ] m_yAxisProperties = new TypeSutBase[6]
			{
				new TypeSutString ( "ID", "", "The ID of the axis", "Heat", "", "" ),
				new TypeSutFloat ( "YMin", "", "The minimum value of the y-axis", 30.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "YMax", "", "The maximum value of the y-axis", 120.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "YTickSpan", "", "The span of the ticks of the y-axis", 10.0, float.MinValue, float.MaxValue, "" )
				,
				new TypeSutString ( "YUnit", "", "The unit of the y-axis", "°C", "", "" ),
				new TypeSutDynamicArray ( "Lines", "The lines of the axis",
				                          new TypeSutUserRecord ( "Line", "Represents a line", m_lineProperties ) )
			};

		private static TypeSutUserRecord m_yAxis =
			new TypeSutUserRecord ( "YAxis", "Represents one y-axis of the plot", m_yAxisProperties );

		private static TypeSutDynamicArray m_yAxisCollection =
			new TypeSutDynamicArray ( "YAxisCollection", "", new TypeSutBase[1] { m_yAxis } );

		private static TypeSutBase [ ] m_xAxisProperties = new TypeSutBase[5]
			{
				
				new TypeSutString ( "Label", "", "The label of the x-axis", "engine speed", "", "" ),
				new TypeSutFloat ( "XMin", "", "The minimum value of the x-axis", 0.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "XMax", "", "The maximum value of the x-axis", 7000.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "XTickSpan", "", "The span of the ticks of the x-axis", 100.0, float.MinValue, float.MaxValue, "" )
				,
				new TypeSutString ( "XUnit", "", "The unit of the x-axis", "rpm", "", "" )
			};

		private static TypeSutUserRecord m_XAxis =
			new TypeSutUserRecord ( "XAxis", "The x-axis of the plot.", m_xAxisProperties );

		private static TypeSutBase [ ] m_plotPropertyItems = new TypeSutBase[5]
			{
				new TypeSutString ( "Name", "", "The file name of the generated plot", "HUGO", "", "" ),
				new TypeSutString ( "Title", "", "The title of the generated plot", "BERTA", "", "" ),
				new TypeSutFloat ( "Scale", "", "The scale of the plot", 1.0, 0.1, float.MaxValue, "" ),
				m_XAxis,
				m_yAxisCollection
			};

		private static TypeSutUserRecord m_plotProperties = new TypeSutUserRecord ( "Properties", "", m_plotPropertyItems );
		
		private static TypeSutUserRecord m_plot =
			new TypeSutUserRecord ( "Plot", "Represents one plot", new TypeSutBase[1] { m_plotProperties } );

		private static TypeSutDynamicArray m_plots =
			new TypeSutDynamicArray ( "Plots", "Keeps all plots",
			                          m_plot );

		#endregion

		#endregion

		#region Main Entry Point and Constructor

		/// <summary>
		/// Main entry point of this test case
		/// </summary>
		/// <param name="args">The args.</param>
		public static void Main ( string [ ] args )
		{
			// create a new instance of the test case class
			TC_ReportingPlotTest tc = new TC_ReportingPlotTest ( );

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
		/// with the name "TC_TC_ReportingPlotTest"
		/// </summary>
		public TC_ReportingPlotTest ( )
			: base ( "TC_ReportingPlotTest" )
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
			AddMetaData ( "TCD", "matthias_hurrle" );
			AddMetaData ( "Comment", "" );
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
			Factory.GetParameterManager ( ).Register ( m_plots );

			Factory.GetParameterManager ( ).Save ( );
		}

		/// <summary>
		/// Loads the parameters.
		/// </summary>
		private void LoadParameters ( )
		{
			//TODO: Retreive the parameterized values from your tpa file here
			m_plots = ( TypeSutDynamicArray ) Factory.GetParameterManager ( ).Parameterise ( m_plots );
		}

		#endregion

		#region Test Case implementation

		#region Perform Test ...

		private void PerformTest ( )
		{
			try
			{
				Reporting.SectionBegin ( "Test Execution" );

				CreatePlots ( );

				Pass ( );
			}
			catch ( Exception ex )
			{
				Error ( );
				Reporting.SetErrorText ( 0, ex.Message, 0 );
				throw ex;
			}
			finally
			{
				Reporting.SectionFinished ( Verdict, new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
			}
		}

		#endregion

		#region CreatePlots ...

		private void CreatePlots ( )
		{
			foreach ( TypeSutUserRecord plot in m_plots.Items )
			{
				CreatePlot ( plot );
			}
		}

		#endregion

		#region CreatePlot ...

		private void CreatePlot ( TypeSutUserRecord plotUR )
		{
			TypeSutUserRecord plotProperties = ( TypeSutUserRecord ) plotUR.Items [ 0 ];
			string name = ( ( TypeSutString ) plotProperties.Items.GetItem ( "Name" ) ).Val;
			string title = ( ( TypeSutString ) plotProperties.Items.GetItem ( "Title" ) ).Val;
			double scale = ( ( TypeSutFloat ) plotProperties.Items.GetItem ( "Scale" ) ).Val;

			IPlot plot = Reporting.CreatePlot ( name, title, scale );

			TypeSutUserRecord xAxis = ( TypeSutUserRecord ) plotProperties.Items.GetItem ( "XAxis" );
			CreateXAxis ( plot, xAxis );

			TypeSutDynamicArray yAxisCollection = ( TypeSutDynamicArray ) plotProperties.Items.GetItem ( "YAxisCollection" );
			CreateYAxisCollection ( plot, yAxisCollection );

			Reporting.AddPlot2Report ( plot );
		}

		#endregion

		#region Create x-axis ...
		
		private void CreateXAxis ( IPlot plot, TypeSutUserRecord xAxis )
		{
			string label = ( ( TypeSutString ) xAxis.Items.GetItem ( "Label" ) ).Val;
			double min = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XMin" ) ).Val;
			double max = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XMax" ) ).Val;
			double tickSpan = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XTickSpan" ) ).Val;
			string unit = ( ( TypeSutString ) xAxis.Items.GetItem ( "XUnit" ) ).Val;

			plot.SetXAxis ( label, min, max, unit, tickSpan );
		}

		#endregion
		
		#region Create y-axis collection ...
		
		private void CreateYAxisCollection ( IPlot plot, TypeSutDynamicArray yAxisCollection )
		{
			foreach ( TypeSutUserRecord yAxis in yAxisCollection.Items )
			{
				CreateYAxis ( plot, yAxis );
			}
		}
		
		#endregion

		#region Create y-axis ...
		
		private void CreateYAxis ( IPlot plot, TypeSutUserRecord yAxis )
		{
			string id = ( ( TypeSutString ) yAxis.Items.GetItem ( "ID" ) ).Val;
			double min = ( ( TypeSutFloat ) yAxis.Items.GetItem ( "YMin" ) ).Val;
			double max = ( ( TypeSutFloat ) yAxis.Items.GetItem ( "YMax" ) ).Val;
			string unit = ( ( TypeSutString ) yAxis.Items.GetItem ( "YUnit" ) ).Val;
			double tickSpan = ( ( TypeSutFloat ) yAxis.Items.GetItem ( "YTickSpan" ) ).Val;

			IYAxis axis = plot.AddYAxis ( id, min, max, unit, tickSpan );
			
			TypeSutDynamicArray lines = ( TypeSutDynamicArray ) yAxis.Items.GetItem ( "Lines" );
			
			foreach ( TypeSutUserRecord line in lines.Items )
			{
				CreateLine ( axis, line );
			}
		}

		#endregion
		
		#region Create line ...
		
		private void CreateLine ( IYAxis axis, TypeSutUserRecord line )
		{
			string id = ( ( TypeSutString ) line.Items.GetItem ( "LineID" ) ).Val;
			double [] xValues = ( ( TypeSutFloatArray ) line.Items.GetItem ( "XValues" ) ).Val;
			double [] yValues = ( ( TypeSutFloatArray ) line.Items.GetItem ( "YValues" ) ).Val;
			double xShift = ( ( TypeSutFloat ) line.Items.GetItem ( "XShift" ) ).Val;
			double yShift = ( ( TypeSutFloat ) line.Items.GetItem ( "YShift" ) ).Val;
			
			TypeSutUserRecord format = ( TypeSutUserRecord ) line.Items.GetItem ( "LineFormat" );
			
			string color = ( ( TypeSutString ) format.Items.GetItem ( "Color" ) ).Val;
			string lineWeight = ( ( TypeSutCollectionUserRecord ) format.Items.GetItem ( "LineWeight" ) ).Selected.Val;
			string lineStyle = ( ( TypeSutCollectionUserRecord ) format.Items.GetItem ( "LineStyle" ) ).Selected.Val;
			
			ILine newLine = axis.AddLine ( id, xValues, yValues, xShift, yShift );
			newLine.Format.Color = color;
			newLine.Format.LineWeight = ( LineWeight ) Enum.Parse ( typeof ( LineWeight ), lineWeight, true );
			newLine.Format.LineStyle = ( LineStyle ) Enum.Parse ( typeof ( LineStyle ), lineStyle, true );
		}
		
		#endregion
		
		#endregion
	}
}