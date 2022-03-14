using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using TC_ReportingPlotTestDynamic.ValueGenerators.Facade;

namespace TC_ReportingPlotTestDynamic
{
	public class TC_ReportingPlotTestDynamic : TestCase
	{
		#region private data ...

		#region parameters ...

		private static TypeSutBase [ ] m_XAxisRange = new TypeSutBase[2]
			{
				new TypeSutFloat ( "StartX", "XStart", "Start in %", 0.0, 0.0, 100.0, "%" ),
				new TypeSutFloat ( "EndX", "", "XEnd in %", 100.0, 0.0, 100.0, "%" ),
			};

		private static TypeSutBase [ ] m_YAxisRange = new TypeSutBase[2]
			{
				new TypeSutFloat ( "StartY", "YStart", "Start in %", 0.0, 0.0, 100.0, "%" ),
				new TypeSutFloat ( "EndY", "", "YEnd in %", 100.0, 0.0, 100.0, "%" ),
			};

		private static TypeSutBase [ ] m_lineValues = new TypeSutBase[5]
			{
				new TypeSutInteger ( "Scores", "", "The amount of values that shall be created", 100, int.MinValue, int.MaxValue, "" )
				,
				new TypeSutCollectionUserRecord ( "Algorithm", "The algorithm to generate the values",
				                                  new TypeSutStringArray ( "AlgorithmChoice", "", "",
				                                                           new string[2]
				                                                           	{
				                                                           		LineType.Linear.ToString ( ), 
																				LineType.Random.ToString ( ), 
				                                                           	},
				                                                           "", "" ),
				                                  new TypeSutString ( "SelectedAlgorithm", "", "", LineType.Linear.ToString ( ), "", "" ) ),
				new TypeSutFloat ( "Frequency", "", "The value repetition frequency in %", 100.0, 0.0, 100.0, "%" ),
				new TypeSutUserRecord ( "XAxisRange", "The x-values range", m_XAxisRange ),
				new TypeSutUserRecord ( "YAxisRange", "The y-values range", m_YAxisRange )
			};

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

		private static TypeSutBase [ ] m_lineProperties = new TypeSutBase[5]
			{
				new TypeSutString ( "LineID", "", "The ID of the line", "UpperBound", "", "" ),
				new TypeSutFloat ( "XShift", "", "Shift the values on the x-axis", 0.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutFloat ( "YShift", "", "Shift the values on the y-axis", 0.0, float.MinValue, float.MaxValue, "" ),
				new TypeSutUserRecord ( "LineFormat", "Format of the line", m_lineFormat ),
				new TypeSutUserRecord ( "Values", "The values of the line to be generated", m_lineValues )
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

		#region class variables ...

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
			TC_ReportingPlotTestDynamic tc = new TC_ReportingPlotTestDynamic ( );

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
		public TC_ReportingPlotTestDynamic ( )
			: base ( "TC_ReportingPlotTestDynamic" )
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
			
			double xmin = 0;
			double xmax = 0;
			
			CreateXAxis ( plot, xAxis, ref xmin, ref xmax );

			TypeSutDynamicArray yAxisCollection = ( TypeSutDynamicArray ) plotProperties.Items.GetItem ( "YAxisCollection" );
			CreateYAxisCollection ( plot, yAxisCollection, xmin, xmax );

			Reporting.AddPlot2Report ( plot );
		}

		#endregion

		#region Create x-axis ...

		private void CreateXAxis ( IPlot plot, TypeSutUserRecord xAxis, ref double xmin, ref double xmax )
		{
			string label = ( ( TypeSutString ) xAxis.Items.GetItem ( "Label" ) ).Val;
			
			xmin = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XMin" ) ).Val;
			xmax = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XMax" ) ).Val;
			
			double tickSpan = ( ( TypeSutFloat ) xAxis.Items.GetItem ( "XTickSpan" ) ).Val;
			string unit = ( ( TypeSutString ) xAxis.Items.GetItem ( "XUnit" ) ).Val;

			plot.SetXAxis ( label, xmin, xmax, unit, tickSpan );
		}

		#endregion

		#region Create y-axis collection ...

		private void CreateYAxisCollection ( IPlot plot, TypeSutDynamicArray yAxisCollection, double xmin, double xmax )
		{
			foreach ( TypeSutUserRecord yAxis in yAxisCollection.Items )
			{
				CreateYAxis ( plot, yAxis, xmin, xmax );
			}
		}

		#endregion

		#region Create y-axis ...

		private void CreateYAxis ( IPlot plot, TypeSutUserRecord yAxis, double xmin, double xmax )
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
				CreateLine ( axis, line, min, max, xmin, xmax );
			}
		}

		#endregion

		#region Create line ...

		private void CreateLine ( IYAxis axis, TypeSutUserRecord line, double min, double max, double xmin, double xmax )
		{
			string id = ( ( TypeSutString ) line.Items.GetItem ( "LineID" ) ).Val;

			IValueGenerator generator = CreateValues ( ( TypeSutUserRecord ) line.Items.GetItem ( "Values" ), min, max, xmin, xmax );
			double [ ] xValues = generator.XValues;
			double [ ] yValues = generator.YValues;
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

		#region CreateValues ...
		
		private IValueGenerator CreateValues ( TypeSutUserRecord values, double min, double max, double xmin, double xmax )
		{
			int scores = (( TypeSutInteger ) values.Items.GetItem ( "Scores" )).Val;
			TypeSutCollectionUserRecord algorithmUR = ( TypeSutCollectionUserRecord ) values.Items.GetItem ( "Algorithm" );
			string algorithm = algorithmUR.Selected.Val;

			double frequency = (( TypeSutFloat ) values.Items.GetItem ( "Frequency" )).Val;
			TypeSutUserRecord xaxisRangeUR = ( TypeSutUserRecord ) values.Items.GetItem ( "XAxisRange" );
			TypeSutUserRecord yaxisRangeUR = ( TypeSutUserRecord ) values.Items.GetItem ( "YAxisRange" );
			
			double xstartoffset = ( ( TypeSutFloat ) xaxisRangeUR.Items.GetItem ( "StartX" ) ).Val;
			double xendoffset = ( ( TypeSutFloat ) xaxisRangeUR.Items.GetItem ( "EndX" ) ).Val;
			double ystartoffset = ( ( TypeSutFloat ) yaxisRangeUR.Items.GetItem ( "StartY" ) ).Val;
			double yendoffset = ( ( TypeSutFloat ) yaxisRangeUR.Items.GetItem ( "EndY" ) ).Val;
			
			double xrange = xmax - xmin;
			double yrange = max - min;
			
			double xstart = xmin + ( ( xrange / 100 ) * xstartoffset );
			double xend = xmin + ( ( xrange / 100 ) * xendoffset );
			double ystart = min + ( ( yrange / 100 ) * ystartoffset );
			double yend = min + ( ( yrange / 100 ) * yendoffset );
			
			return ValueGeneratorFactory.Instance.GetValueGenerator ( algorithm, xstart, xend, ystart, yend, frequency, ( uint ) scores );
		}
		
		#endregion
		
		#endregion
	}
}