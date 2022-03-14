using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Types;

namespace ETAS.TestCaseTemplate
{
    internal static  class Parameters
    {
        #region Defined Parameters
        private static string[] PluginFiles { get; set; }
        public static TypeSutStringArray PluginFilesArray;
        #endregion

        #region Initialization
        public static void Init(RealTimeScriptingTestCase myTestCase)
        {
            PluginFiles = myTestCase.PluginFiles;
            PluginFilesArray = new TypeSutStringArray("PluginFiles", "PluginFiles", "The set of plugin files to Activate", PluginFiles, "", "");

            RegisterParameters ( myTestCase);
            LoadParameters(myTestCase );
        }

        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private static void RegisterParameters(TestCase myTestCase)
        {
            myTestCase.Factory.GetParameterManager().CreateTpaFile();
            myTestCase.Factory.GetParameterManager().Register(Parameters.PluginFilesArray);
            myTestCase.Factory.GetParameterManager().Save();
        }

        /// <summary>
        /// Loads the parameters.
        /// </summary>
        private static void LoadParameters(TestCase myTestCase)
        {
            //TODO: Retreive the parameterized values from your tpa file here
            Parameters.PluginFilesArray =
                (TypeSutStringArray)myTestCase.Factory.GetParameterManager().Parameterise(Parameters.PluginFilesArray);
        }
        #endregion
    }
}