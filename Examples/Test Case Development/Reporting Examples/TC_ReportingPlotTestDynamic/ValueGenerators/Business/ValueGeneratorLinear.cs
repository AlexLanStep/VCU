using System.Collections;
using TC_ReportingPlotTestDynamic.ValueGenerators.Facade;

namespace TC_ReportingPlotTestDynamic.ValueGenerators.Business
{
	internal class ValueGeneratorLinear : ValueGeneratorBase, IValueGenerator
	{
		#region private data ...

		private double [ ] m_xvalues;
		private double [ ] m_yvalues;

		private double m_xticks;
		private double m_yticks;

		#endregion

		#region construction ...

		#region constructor ...

		internal ValueGeneratorLinear ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
			: base ( xmin, xmax, ymin, ymax, frequency, scores )
		{
			Init ( );
		}

		#endregion

		#region Init ...

		private void Init ( )
		{
			m_xticks = ( XMax - XMin ) / Scores;
			m_yticks = ( YMax - YMin ) / Scores;
			
			GenerateXValues ( );
			GenerateYValues ( );
		}

		#endregion

		#endregion

		#region protected access ...

		#region GetXValues ...

		protected override double [ ] GetXValues ( )
		{
			return m_xvalues;
		}

		#endregion

		#region GetYValues ...

		protected override double [ ] GetYValues ( )
		{
			return m_yvalues;
		}

		#endregion

		#endregion

		#region internal implementation ...

		#region GenerateXValues ...
		
		private void GenerateXValues ( )
		{
			ArrayList xvalues = new ArrayList ( ( int ) Scores );

			for ( double i = 0 ; i < Scores ; ++i )
			{
				xvalues.Add ( XMin + ( m_xticks * i ) );
			}

			m_xvalues = ( double [ ] ) xvalues.ToArray ( typeof ( double ) );
		}

		#endregion
		
		#region GenerateYValues ...
		
		private void GenerateYValues ( )
		{
			ArrayList yvalues = new ArrayList ( ( int ) Scores );

			int freqcount = 1;
			double offset = 0;
			
			for ( double i = 0 ; i < Scores ; ++i )
			{
				double yvalue = YMin + ( m_yticks * offset );
				yvalues.Add ( yvalue );
				
				++ offset;
				
				if ( FrequencyReached ( i, freqcount ) )
				{
					freqcount ++;
					offset = 0;
				}
			}

			m_yvalues = ( double [ ] ) yvalues.ToArray ( typeof ( double ) );
		}

		#endregion
		
		#region FrequencyReached ...
		
		private bool FrequencyReached ( double count, double freqcount )
		{
			if ( Frequency == 0 || Frequency == 100 ) return false;
			
			if ( m_xvalues [ ( int ) count ] > ( XMin + (((XMax - XMin ) / 100 ) * ( Frequency * freqcount ) ) ) )
			{
				return true;
			}
			
			return false;
		}
		
		#endregion
		
		#endregion
	}
}