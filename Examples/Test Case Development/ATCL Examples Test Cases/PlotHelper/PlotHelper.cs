using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Reporting;
using Etas.Eas.Atcl.Interfaces.Types;

namespace PlotHelper
{
    public static class PlotHelper
    {
        public static IPlot CreatePlot ( TestCase tc, TypeSut1DFloatTable [ ] data )
        {
            IPlot plot = tc.Reporting.CreatePlot ( Guid.NewGuid ( ).ToString ( ), "Plot", 1.0 );

            plot.SetXAxis ( "X", CalcualteMin ( data [ 0 ].ValueX ), CalculateMax ( data [ 0 ].ValueX ), data [ 0 ].UnitX );

            foreach ( TypeSut1DFloatTable table in data )
            {
                IYAxis axis = plot.AddYAxis ( table.Label, CalcualteMin ( table.ValueY ), CalculateMax ( table.ValueY ), table.UnitY );
                axis.AddLine ( table.Label, table.ValueX, table.ValueY, 0.0, 0.0 );
            }

            return plot;
        }

        private static double CalcualteMin ( double [ ] values )
        {
            double min = values [ 0 ];

            for ( int index = 1 ; index < values.Length ; index++ )
            {
                if ( values [ index ] < min )
                {
                    min = values [ index ];
                }
            }
            return min;
        }

        private static double CalculateMax ( double [ ] values )
        {
            double max = values [ 0 ];

            for ( int index = 1 ; index < values.Length ; index++ )
            {
                if ( values [ index ] > max )
                {
                    max = values [ index ];
                }
            }
            return max;
        }
    }
}