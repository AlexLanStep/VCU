/* Test case created by ETAS GmbH 2010 
   $Id: TC_DiagAccess.cs 14158 2011-09-28 08:58:52Z seb9fe $ */
/* 
 Copyright 2010 (c) All rights reserved for ETAS GmbH 
 This test case is provided by ETAS as an easy entry example to use LABCAR-AUTOMATION immediately. 
 It is not provided for free and is not allowed to be distributed without permission of ETAS GmbH.
*/

using System;
// to use elements of ATCL namespace directly
// this saves you from typing the whole namespace 
// each time you want to use an ATCL element like
// Parameter, Factory, etc. using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces;

// to allow direct use of TypeSutxxx within the test case 
// we added the using directive
using Etas.Eas.Atcl.Interfaces.Types;

// to allow direct use of datalogger types 
// we added the using directive
using Etas.Eas.Atcl.Interfaces.Types.Datalogger;

// to allow direct use of VerdictCode as type within the test case 
// we added the using directive
using Etas.Eas.Atcl.Interfaces.Verdicts;

// to allow direct use of SectionVerdict as type within the test case 
// we added the using directive
using SectionVerdict = Etas.Eas.Atcl.Interfaces.Verdicts.Verdict;

namespace ETAS.TestCaseTemplate
{
    public class TC_DiagAccess : TestCase
    {

        /// <summary>
        /// Main entry point of this test case
        /// </summary>
        /// <param name="args">Not used in this example.</param>
        public static void Main ( string [ ] args )
        {
            // create a new instance of the test case class
            TC_DiagAccess tc = new TC_DiagAccess ( args);
            try
            {
#if DEBUG
                // A more specific test, replaced to be more flexible
                //if ( ( args.Length == 1 ) && args [ 0 ].ToString ( ).Equals ( "-build" , StringComparison.InvariantCultureIgnoreCase ) )

                // for a build run and to generate the .tpa file it is not necessary to configure the tools completely, therefore the PerformTest part is omitted
                if ( ( args.Length > 0 ) && ( args [ 0 ] == "-build" ) )
                {

                    tc.Init ( );
                }
                else
                {
#endif
                    tc.Init ( );
                    tc.PerformTestCase ( );
#if DEBUG
                }
#endif

            }
            catch ( Exception ex )
            {
                // On exception, set the test case verdict to error, display the error
                // message in the Test Handler and the report.
                tc.Error ( );
                tc.Reporting.LogExtension ( string.Format ( "Exception occurred in Test Case: {0}" , ex.Message ) );
                tc.Reporting.SetErrorText ( 0 , ex.Message , 0 );
            }
            finally
            {
                Ports.Finalize (tc );
                tc.Finished ( );
            }
        }

        public TC_DiagAccess (string[] args )
            : base ( typeof ( TC_DiagAccess ).Name, args )
        {
        }

        /// <summary>
        /// Registers the meta data for the test case,i.e.
        /// adds the meta data to the THD file of the test case. 
        /// These data are shown in the Overview part of the report
        /// within the "Test Case Metadata table".
        /// 
        /// All keys used in the AddMetaData method are preceded by 
        /// a TC in the table ( "TCD" will be displayed as "TC TCD").
        /// <remarks>The version attribute is read from the assembly version which
        /// is defined in the AssemblyInfo.cs file</remarks>
        /// </summary>
        private void RegisterMetaData ( )
        {
            AddMetaData ( "TCD" , "User" );
            AddMetaData ( "Comment" , "This test case is generated." );
            AddMetaData ( "Version" , GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );
            AddMetaData ( "Purpose" , "Example for use of Diagnosis port" );
            AddMetaData ( "ODX Version" , "2.0.1" );
            //TODO: Add any additional meta data you need here
        }

        /// <summary>
        /// Initializes this <see cref="TestCase"/>.
        /// </summary>
        /// <remarks>
        /// Initialization includes registering metadata, ports and parameters.
        /// Additionally all parameters which are registered are loaded, i.e.
        /// all changes made in the test manager tool to the parameters are read
        /// by the test case during runtime.
        /// </remarks>
        protected override void Init ( )
        {
            SectionVerdict sectionverdict = new SectionVerdict ( VerdictCode.None );

            sectionverdict = new Verdict ( VerdictCode.None );
            Reporting.SectionBegin ( "Initialization" );
            try
            {
                base.Init();

                // Meta data like 
                // - who created the test case 
                // - what is the purpose of the test case 
                // - etc.

                RegisterMetaData ( );
                // filename, fault simulations, etc.
                Parameters.Init ( this );
                // Access to model, ECU, etc.
                Ports.Init ( this );
            }
            catch ( Exception ex )
            {
                sectionverdict.Error ( );
                Reporting.SetErrorText ( 0 , string.Format ( "Exception during Test Case initialization!" ) , 50 );
                throw ( ex );
            }
            sectionverdict.Pass ( );
            Reporting.SectionFinished ( "Initialization completed successful." , sectionverdict );
        }

        /// <summary>
        /// Reads the programming date of the ECU by use of the service 'readECUIdentification'
        /// 
        /// LLSendPayloadSymbolic takes the ShortName of the service as first parameter 
        /// followed by an array of the TypeTAgValueStringRecord
        /// holding a pair of Parameter-ShortName and it's value, e.g.
        /// new TypeTagValueStringRecord ( "identificationOption" , "programmingDate" )
        /// </summary>
        private void readECUIdentifiction_programmingDate ( )
        {

            TypeTagValueStringRecord [ ] result = Ports.Diagnostics.LLSendPayloadSymbolic ( "readECUIdentification" , new TypeTagValueStringRecord [ ] { new TypeTagValueStringRecord ( "identificationOption" , "programmingDate" ) , } );
            //new TypeTagValueStringRecord("identificationOption", "VIN - Vehicle Identification Number")
            Reporting.SetInfoText ( 0 , "Result LLSendPayloadSymbolic: " , 0 );
            foreach ( var record in result )
            {
                Reporting.SetInfoText ( 0 , "Key: " + record.Tag + " Value:" + record.Value , 0 );
            }
        }

        /// <summary>
        /// Reads the programming date of the ECU by use of the service 'readECUIdentification'
        /// but uses a hex string to specify the parameters of the service.
        /// E.g. if the Parameter Shortname and it's value are
        /// "identificationOption" , "programmingDate"
        /// the hex representation of the request parameters might be "99"
        /// then the request composes to
        /// 0x1A 99
        /// Or, to request the VIN - Vehicle Identification Number
        /// use service "readECUIdentification" with "identificationOption" set to "VIN - Vehicle Identification Number"
        /// => 0x1A 90
        /// </summary>
        private void readECUIdentifiction_WholePDUAsHex ( )
        {
            TypeTagValueStringRecord [ ] result = Ports.Diagnostics.LLSendPayloadSymbolic ( "readECUIdentification" , new TypeTagValueStringRecord [ ] { new TypeTagValueStringRecord ( "WholePDUAsHex" , "99" ) , } );

            Reporting.SetInfoText ( 0 , "Result LLSendPayloadSymbolic with whole PDU as hex string: " , 0 );
            foreach ( var record in result )
            {
                Reporting.SetInfoText ( 0 , "Key: " + record.Tag + " Value:" + record.Value , 0 );
            }
        }

        /// <summary>
        /// Reads the programming date of the ECU by use of the
        /// hex representation of the service 'readECUIdentification'
        /// The request composes to
        /// 0x1A 99
        /// The value depends on your ODX project.
        /// </summary>
        private void readECUIdentifiction_AsHex ( )
        {
            String [ ] result = Ports.Diagnostics.LLSendPayloadHex ( "1A" , "99" );
            //5A 99 19 94 09 11
            Reporting.SetInfoText ( 0 , "Result LLSendPayloadHex : " , 0 );
            int i = 0;
            foreach ( var record in result )
            {
                Reporting.SetInfoText ( 0 , "Value Nr " + i + " : " + record , 0 );
                i++;
            }
        }

        /// <summary>
        /// Performs the test case.
        /// Ports are configured 
        /// Simulation and ECU access are started
        /// Diagnosis access is performed
        /// Report is written.
        /// </summary>
        /// <remarks>Use of configure signatures
        /// <code>Ports.ModelAccess.Configure ( new string [ ] { "" } );
        /// Ports.ECUAccessMeasurement.Configure ( "default" , DeviceID,HWLayerID ,ProtocolID  );
        /// Ports.Diagnostics.ConfigureTool("ECU", new string[] { "" }, new string[] { "",Node_ID , Protocol_ID });</code></remarks>
        private void PerformTestCase ( )
        {
            try
            {
                #region preparation of ports
                // The preparation of the ports is shown as a separate section in the report
                Reporting.SectionBegin ( "Port Preparation" );
                SectionVerdict sectionVerdict = new Verdict ( VerdictCode.None );
                try
                {
                    Ports.ModelAccess.Configure ( new string [ ] { "" } );
                    Ports.ECUAccessMeasurement.Configure ( "default" , "default" , "default" );
                    Ports.Diagnostics.LLConfigure ( "default" , "default" );
                }
                catch ( Exception ex )
                {
                    sectionVerdict.Error ( );
                    Reporting.SectionFinished ( "Ports preparation unsuccessful." , sectionVerdict );
                    Reporting.SetErrorText ( 0 , string.Format ( "Exception during port preparation!" ) , 0 );
                    throw ( ex );
                }
                sectionVerdict.Pass ( );
                Reporting.SectionFinished ( "Ports prepared successfully." , sectionVerdict );
                Pass ( );
                Reporting.SaveReport ( );
                #endregion preparation of ports

                Reporting.SectionBegin ( "Test Execution" );
                Ports.SetECUSignals ( );

                // Start measurement of variables in experiment environment
                Ports.ModelAccess.Start ( );

                // Start measurement in INCA experiment
                Ports.ECUAccessMeasurement.Start ( );

                // Start communication with ECU On Board Diagnosis
                Ports.Diagnostics.LLCommunicationStart ( );

                // Send a service request and hand over the parameter part as one hex string, 
                // the response is still interpreted by the D-Server (e.g. DTS7)
                readECUIdentifiction_WholePDUAsHex ( );

                // Send a service request by name and
                // specify the used parameters by name, too.
                // The response is interpreted by the D-Server for you.
                readECUIdentifiction_programmingDate ( );

                // Send a service request as hex string
                // The response is the hex answer of the ECU and not interpreted by the D-Server
                readECUIdentifiction_AsHex ( );

                // Stop communication with ECU On Board Diagnosis
                Ports.Diagnostics.LLCommunicationStop ( );
                Pass ( );
            }
            catch ( Exception ex )
            {
                Error ( );
                Reporting.SetErrorText ( 0 , ex.Message , 0 );
                throw;
            }
            finally
            {
                Reporting.SectionFinished ( Verdict , new Verdict ( Factory.GetVerdictManager ( ).GetVerdictOfTestCase ( ) ) );
            }
        }
    }
}
