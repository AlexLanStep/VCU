using System.Collections;
using TC_ReportingPlotTestDynamic.ValueGenerators.Business;

namespace TC_ReportingPlotTestDynamic.ValueGenerators.Facade
{
	internal sealed class ValueGeneratorFactory
	{
		#region private data ...

		private Hashtable m_creatorTypes;

		private delegate IValueGenerator Creator ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores );

		#endregion

		#region internal access ...

		internal IValueGenerator GetValueGenerator ( string algorithm, double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
		{
			return ( ( Creator ) m_creatorTypes [ algorithm ] ) ( xmin, xmax, ymin, ymax, frequency, scores );
		}
		
		#endregion

		#region Factory implementation

		private IValueGenerator LinearCreator ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
		{
			return new ValueGeneratorLinear ( xmin, xmax, ymin, ymax, frequency, scores );
		}
		private IValueGenerator RandomCreator ( double xmin, double xmax, double ymin, double ymax, double frequency, uint scores )
		{
			return new ValueGeneratorRandom ( xmin, xmax, ymin, ymax, frequency, scores );
		}


		#endregion

		#region singleton implementation ...

		private ValueGeneratorFactory ( )
		{
			m_creatorTypes = new Hashtable ( );
			
			m_creatorTypes.Add ( LineType.Linear.ToString ( ), new Creator ( LinearCreator ) );
			m_creatorTypes.Add ( LineType.Random.ToString ( ), new Creator ( RandomCreator ) );
		}

		internal static ValueGeneratorFactory Instance
		{
			get { return Nested.instance; }
		}

		private class Nested
		{
			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested ( )
			{
			}

			internal static readonly ValueGeneratorFactory instance = new ValueGeneratorFactory ( );
		}

		#endregion
	}
}