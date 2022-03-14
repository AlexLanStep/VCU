using System;

using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Factory;
using Etas.Eas.Atcl.Interfaces.Configuration;
using Etas.Eas.Atcl.Interfaces.Ports;
using Etas.Eas.Atcl.Interfaces.Verdicts;
using Etas.Eas.Atcl.Interfaces.Types;

namespace TC_StartSAR
{
    public class TC_StartSAR : Etas.Eas.Atcl.Interfaces.TestCase
    {
        #region Main Entry Point and Constructor

        public static void Main(string[] args)
        {

#if DEBUG
            AtclFactory.GetInstance().GetSARHostController().StartSARHostProcess();
            AtclFactory.GetInstance().GetSARHostController().InitializeSAR( new SarConfiguration (
                          @"Q:\ATCL Examples\Test Bench Configuration\LCO.tbc"
                        , @"Q:\ATCL Examples\Test Cases\Report"));
#endif

            TC_StartSAR tc = new TC_StartSAR();
            tc.Init();
            tc.PerformTest();
            tc.Finished();

#if DEBUG
            AtclFactory.GetInstance().GetSARHostController().ShutdownSARHostProcess();
#endif

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCase"/> class
        /// with the name "TC_TC_StartSAR"
        /// </summary>
        public TC_StartSAR()
            : base("TC_StartSAR")
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
        private void Init()
        {
            try
            {
                RegisterMetaData();
                RegisterPorts();
                RegisterParameters();
                LoadParameters();
            }
            catch (Exception ex)
            {
                Reporting.SetErrorText(0, string.Format("Execption during Test Case initialization! Exception message is {0}", ex.Message), 0);
                Error();
                throw (ex);
            }
        }

        /// <summary>
        /// Registers the meta data.
        /// </summary>
        /// <remarks>The version attribute is read from the assembly version which
        /// is defined in the AssemblyInfo.cs file</remarks>
        private void RegisterMetaData()
        {
            AddMetaData("TCD", "Alexander Mayer");
            AddMetaData("Comment", "This test case shows how to start the SAR from the TC.");
            AddMetaData("Version", GetType().Assembly.GetName().Version.ToString());

            //TODO: Add any additional meta data here
        }

        /// <summary>
        /// Registers the ports.
        /// </summary>
        private void RegisterPorts()
        {
            //TODO: Register any port used in your test case here
        }


        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private void RegisterParameters()
        {
            Factory.GetParameterManager().CreateTpaFile();

            //TODO: Register any parameters which should be exported to tpa here

            Factory.GetParameterManager().Save();
        }

        /// <summary>
        /// Loads the parameters.
        /// </summary>
        private void LoadParameters()
        {
            //TODO: Retreive the parameterized values from your tpa file here
        }


        #endregion

        #region Test Case implementation

        private void PerformTest()
        {
            Pass();
        }

        #endregion
    }
}
