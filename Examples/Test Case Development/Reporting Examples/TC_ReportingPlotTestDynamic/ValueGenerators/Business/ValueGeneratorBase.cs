using TC_ReportingPlotTestDynamic.ValueGenerators.Facade;

namespace TC_ReportingPlotTestDynamic.ValueGenerators.Business
{
	internal abstract class ValueGeneratorBase : IValueGenerator
	{
		#region private data ...

		private double m_xmin;
		private double m_xmax;
		private double m_ymin;
		private double m_ymax;
		private double m_frequency;
		private uint m_scores;

		#endregion

		#region construction ...

		internal ValueGeneratorBase ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
		{
			m_xmin = xmin;
			m_xmax = xmax;
			m_ymin = ymin;
			m_ymax = ymax;
			m_frequency = frequency;
			m_scores = scores;
		}

		#endregion

		#region public access ...

		#region XValues ...

		public double [ ] XValues
		{
			get { return GetXValues ( ); }
		}

		#endregion

		#region YValues ...

		public double [ ] YValues
		{
			get { return GetYValues ( ); }
		}

		#endregion

		#endregion

		#region protected access ...

		#region abstract ...

		protected abstract double [ ] GetXValues ( );

		protected abstract double [ ] GetYValues ( );

		#endregion

		#region properties ...

		protected double XMin
		{
			get { return m_xmin; }
		}

		protected double XMax
		{
			get { return m_xmax; }
		}

		protected double YMin
		{
			get { return m_ymin; }
		}

		protected double YMax
		{
			get { return m_ymax; }
		}

		protected double Frequency
		{
			get { return m_frequency; }
		}

		protected uint Scores
		{
			get { return m_scores; }
		}

		#endregion

		#endregion
	}
}