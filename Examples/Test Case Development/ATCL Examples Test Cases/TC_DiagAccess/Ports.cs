//
// This source code was auto-generated 
//

using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using System.Collections.Generic;

namespace ETAS.TestCaseTemplate
{
	public static class Ports
	{
		public static bool manualStart = false;

        /// <summary>
        /// Performs initialization of the ports used in this test case 
        /// It initializes the 
        /// model access,
        /// ECU measurement and the diagnostic access port.
        /// The ports are initialized, i.e. created and tool configured,
        /// in case the model access port is in state closed,
        /// which is typicallyfor not using the initialization by the Test Handler.
        /// </summary>
        /// <remarks>ConfigureTool needs the following arguments
        /// <code>ModelAccess.ConfigureTool("ECU", new string[] { "" }, new string[] { "", ModelID, ModelDataID, ModelTypeID });
        /// ECUAccessMeasurement.ConfigureTool("ECU", new string[] { "" }, new string[] { "", EA_ID });
        /// Diagnostics.ConfigureTool("ECU", new string[] { "" }, new string[] { "", Customer_ID, ECU_ID });</code></remarks>
        /// <param name="myTestCase">An instance of the test case class.</param>
		public static void Init( TestCase myTestCase ) 
		{
			try
			{
				RegisterPorts( myTestCase );
				if( ModelAccess.IsClosed )
				{
					ModelAccess.Create ( );
					ModelAccess.ConfigureTool("ECU", new string[] { "" }, new string[] { "", "default", "default", "default" });
					ECUAccessMeasurement.Create ( );
					ECUAccessMeasurement.ConfigureTool("ECU", new string[] { "" }, new string[] { "", "default" });
					Diagnostics.Create ( );
                    Diagnostics.ConfigureTool("ECU", new string[] { "" }, new string[] { "", "default", "default" });
					manualStart = true;
				}
				else
				{
				}
			}
			catch (Exception ex)
			{
				myTestCase.Reporting.SetErrorText(0, string.Format("Execption during Test Case initialization! Exception message is {0}", ex.Message), 0);
				myTestCase.Error();
				throw (ex);
			}
		}

        /// <summary>
        /// Registers the ports used in the test case and assigns the object references to the class instance members.
        /// <remarks>
        /// The instance name of the port is defined in the TBC file
        /// An easy line would look like 
        /// m_portMA = Factory.GetPortMA ( "P_MA" );
        ///     getting the port of type ModelAccess with the instance name "P_MA"
        /// A more tricky line is used here 
        ///     m_ModelAccess = ( IPortMA ) myTestCase.Factory.GetPort ( "Type_ModelAccess_port" , "ModelAccess" );
        /// </remarks>
        /// </summary>
		private static void RegisterPorts( TestCase myTestCase ) 
		{
            //TODO: Register any port used in your test case here
			m_ModelAccess = (IPortMA) myTestCase.Factory.GetPort("Type_ModelAccess_port", "ModelAccess");
			ModelAccess.Timeout = -1;;
			m_ECUAccessMeasurement = (IPortEAM) myTestCase.Factory.GetPort("Type_ECUAccessMeasurement_port", "ECUAccessMeasurement");
			ECUAccessMeasurement.Timeout = -1;;
			m_Diagnostics = (IPortDiag) myTestCase.Factory.GetPort("Type_Diagnostics_port", "Diagnostics");
			Diagnostics.Timeout = -1;;
		}
		public static void SetECUSignals(  ) 
		{
			List<TypeDLSignal> typeDLSignals = new List<TypeDLSignal>();
			ECUAccessMeasurement.SelectElements(typeDLSignals.ToArray());
		}
		public static void Finalize( TestCase myTestCase ) 
		{
			try
			{
				if( manualStart )
				{
					ModelAccess.Close ( );
					ECUAccessMeasurement.Close ( );
					Diagnostics.Close ( );
				}
				else
				{
				}
			}
			catch (Exception ex)
			{
				myTestCase.Reporting.SetErrorText(0, string.Format("Execption during Test Case finalize! Exception message is {0}", ex.Message), 0);
				myTestCase.Error();
				throw (ex);
			}
		}
		private static IPortMA m_ModelAccess= null;

		public static IPortMA ModelAccess
		{
			get { return m_ModelAccess; } 
		}
		private static IPortEAM m_ECUAccessMeasurement= null;

		public static IPortEAM ECUAccessMeasurement
		{
			get { return m_ECUAccessMeasurement; } 
		}
		private static IPortDiag m_Diagnostics= null;

		public static IPortDiag Diagnostics
		{
			get { return m_Diagnostics; } 
		}
	}
}
