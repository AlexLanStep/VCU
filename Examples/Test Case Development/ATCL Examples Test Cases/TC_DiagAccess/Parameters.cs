//
// This source code was auto-generated 
//

using System;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;
using Etas.Eas.Atcl.Interfaces.TestParameter;
//using Etas.Eas.Atcl.Interfaces.TestParameter;

namespace ETAS.TestCaseTemplate
{
    public static class Parameters
    {
        public static TypeSutBool Validation = new TypeSutBool ( "Validation" , "" , "" , true );

        public static void Init ( TestCase myTestCase )
        {
            RegisterParameters ( myTestCase );
            LoadParameters ( myTestCase );
        }

        /// <summary>
        /// Registers the parameters.
        /// </summary>
        private static void RegisterParameters ( TestCase myTestCase )
        {
            // You have to tell the test manager which parameters 
            // shall be visible in the parameter manager view 

            // to cut it short we get a local instance of the parameter manager
            // to use IParameterManager you must add the using instruction at the top:
            // using Etas.Eas.Atcl.Interfaces.TestParameter;
            IParameterManager paramManager = myTestCase.Factory.GetParameterManager ( );
            paramManager.CreateTpaFile ( );
            paramManager.Register ( Parameters.Validation );

            //TODO: Register any parameters which should be exported to tpa here
            paramManager.Save ( );
        }

        /// <summary>
        /// Loads the parameters changed in Test Manager.
        /// </summary>
        private static void LoadParameters ( TestCase myTestCase )
        {
            //TODO: Retrieve the parameterized values for your exposed parameters  
            // to cut it short we get a local instance of the parameter manager
            // to use IParameterManager you must add the using instruction at the top:
            // using Etas.Eas.Atcl.Interfaces.TestParameter;
            IParameterManager paramManager = myTestCase.Factory.GetParameterManager ( );

            Parameters.Validation = ( TypeSutBool ) paramManager.Parameterise ( Parameters.Validation );
        }
    }
}
