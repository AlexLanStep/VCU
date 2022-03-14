using System;
using System.Collections;
using TC_ReportingPlotTestDynamic.ValueGenerators.Facade;

namespace TC_ReportingPlotTestDynamic.ValueGenerators.Business
{
	internal class ValueGeneratorRandom : ValueGeneratorBase, IValueGenerator
	{
		#region private data ...
		
		private double [] m_xvalues;
		private double [] m_yvalues;
		
		private double m_xTickOffset;
		private double m_yTickOffset;

		
		#endregion
		
		#region construction ...
		
		#region constructor ...
		
		internal ValueGeneratorRandom ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
			: base ( xmin, xmax, ymin, ymax, frequency, scores )
		{
			Init ( );
		}

		#endregion
		
		#region Init ...
		
		private void Init ( )
		{
			m_xTickOffset = ( XMax - XMin ) / Scores;
			m_yTickOffset = ( YMax - YMin ) / Scores;
			
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
		
		#region private implementation ...

		#region Director ...
		
		private delegate double Director ( double baseval, int counter );
		
		#endregion
		
		#region GenerateXValues ...
		
		private void GenerateXValues ( )
		{
			ArrayList xvalues = new ArrayList ( ( int ) Scores );

			for ( double i = 0 ; i < Scores ; ++i )
			{
				xvalues.Add ( XMin + ( m_xTickOffset * i ) );
			}

			m_xvalues = ( double [ ] ) xvalues.ToArray ( typeof ( double ) );
		}	
		
		#endregion 
		
		#region GenerateYValues ...
		
		private void GenerateYValues ( )
		{
			ArrayList yvalues = new ArrayList ( ( int ) Scores );
			
			int freqcount = 1;
			
			Random randomizer = new Random ( ( int ) Scores );
			double rand = randomizer.NextDouble ( );
			
			double actualvalue;
			double baseval = YMin;
			int counter = 0;
			
			Director director = new Director ( Add );
			
			for ( double i = 0 ; i < Scores ; ++i )
			{
				if ( FrequencyReached ( i, freqcount ) )
				{
					rand = randomizer.NextDouble ( );
					
					freqcount ++;
					
					counter = 0;
					
					if ( freqcount % 2 > 0 )
					{
						director = new Director ( Subtract );
					}
					else
					{
						director = new Director ( Add );
					}
				}
				
				actualvalue = baseval * rand;
				
				if ( actualvalue > YMax || actualvalue < YMin )
				{
					rand = randomizer.NextDouble ( );
					actualvalue = YMin + ( m_yTickOffset * Scores ) * rand;
				}
				
				yvalues.Add ( actualvalue );
				
				baseval = director ( baseval, counter );
				
				++counter;
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
		
		#region Director Substract ...
		
		private double Subtract ( double baseval, int counter )
		{
			double i = ( m_yTickOffset / 100 ) * Frequency;
			double v = baseval - ( i * counter );
			
			if ( v <= YMin )
				return YMin + ( i * counter );
			
			return baseval - ( i * counter );
		}
		
		#endregion
		
		#region Director Add ...
		
		private double Add ( double baseval, int counter )
		{
			double i = ( m_yTickOffset / 100 ) * Frequency;
			double v = baseval + ( i * counter );
			
			if ( v >= YMax )
				return YMax - ( i * counter );
			
			return v;
		}
		
		#endregion
		
		#endregion
	}
}