//
// This source code was auto-generated 
//

using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Ports;

namespace ETAS.TestCaseTemplate
{
	public static class Ports
	{
        private static IPortRealTimeScripting m_RealTimeScripting = null;
        private static IPortMA m_MAPort = null;

        public static bool manualStart = false;

		public static void Init( TestCase myTestCase ) 
		{
			try
			{
				RegisterPorts( myTestCase );
			}
			catch (Exception ex)
			{
				myTestCase.Reporting.SetErrorText(0, string.Format("Execption during Test Case initialization! Exception message is {0}", ex.Message), 0);
				myTestCase.Error();
				throw (ex);
			}
		}
		private static void RegisterPorts( TestCase myTestCase ) 
		{
            m_RealTimeScripting = (IPortRealTimeScripting)myTestCase.Factory.GetPort("Type_RealTimeScripting_port", "RealTimeScript"); // type of port (portName), name of port (instanceName)
		    m_RealTimeScripting.Timeout = -1;
            m_MAPort = (IPortMA)myTestCase.Factory.GetPort ( "Type_ModelAccess_port", "ModelAccess" );
		    m_MAPort.Timeout = -1;
		}
		public static void Finalize( TestCase myTestCase ) 
		{
			try
			{
                ;
			}
			catch (Exception ex)
			{
				myTestCase.Reporting.SetErrorText(0, string.Format("Execption during Test Case finalize! Exception message is {0}", ex.Message), 0);
				myTestCase.Error();
				throw (ex);
			}
		}
		
		public static IPortRealTimeScripting RealTimeScripting
		{
			get { return m_RealTimeScripting; } 
		}

        public static IPortMA MAPort
        {
            get { return m_MAPort; }
        }
	}
}
