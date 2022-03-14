using System;

using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_SectionTest
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class TC_SectionTest : TestCase 
	{
		private TypeSutFloat m_float = new TypeSutFloat( "float", "label", "comment", 1.2, 1.0, 2.0, "km");


        public TC_SectionTest(string tcname)
            : base(tcname)
		{
		}
		
		public void PerformTest ()
		{
			
			RegisterParameters();

            Reporting.SectionBegin ( "Section 1" );
            Reporting.SectionBegin ( "Section 1.1" );
            Reporting.SectionBegin ( "Section 1.1.1." );
            Reporting.SetText ( 0, 0, "Operation failed!", 0 );
            Reporting.SectionFinished ( "Section 1.1.1. failed!", new Verdict ( VerdictCode.Fail ) );
            Reporting.SectionFinished ( "Section 1.1. failed!", new Verdict ( VerdictCode.Fail ) );
            Reporting.SectionFinished ( "Section 1. failed!", new Verdict ( VerdictCode.Fail ) );

            Reporting.SectionBegin ( "Section 2" );
            Reporting.SectionBegin ( "Section 2.1" );
            Reporting.SectionBegin ( "Section 2.1.1." );
            Reporting.SetText ( 0, 0, "Operation is inconc!", 0 );
            Reporting.SectionFinished ( "Section 2.1.1. failed!", new Verdict ( VerdictCode.Inconc ) );
            Reporting.SectionFinished ( "Section 2.1. failed!", new Verdict ( VerdictCode.Inconc ) );
            Reporting.SectionFinished ( "Section 2. failed!", new Verdict ( VerdictCode.Inconc ) );

            Reporting.SectionBegin ( "Section 3" );
            Reporting.SectionBegin ( "Section 3.1" );
            Reporting.SectionBegin ( "Section 3.1.1." );
            Reporting.SetText ( 0, 0, "Operation is erronous!", 0 );
            Reporting.SectionFinished ( "Section 3.1.1. is erronous!", new Verdict ( VerdictCode.Error ) );
            Reporting.SectionFinished ( "Section 3.1. is erronous!", new Verdict ( VerdictCode.Error ) );
            Reporting.SectionFinished ( "Section 3. is erronous!", new Verdict ( VerdictCode.Error ) );

            Reporting.SectionBegin ( "Section 4" );
            Reporting.SectionBegin ( "Section 4.1" );
            Reporting.SectionBegin ( "Section 4.1.1." );
            Reporting.SetText ( 1, 0, "Section 4 is passed, should not appear in Section List", 0 );
            Reporting.SectionFinished ( "Section 4.1.1. passed!", new Verdict ( VerdictCode.Pass ) );
            Reporting.SectionFinished ( "Section 4.1. passed!", new Verdict ( VerdictCode.Pass ) );
            Reporting.SectionFinished ( "Section 4. passed!", new Verdict ( VerdictCode.Pass ) );

            Reporting.SectionBegin ( "MixedSection 5" );
            Reporting.SectionBegin ( "MixedSection 5.1" );
            Reporting.SectionBegin ( "MixedSection 5.1.1." );
            Reporting.SetText ( 1, 0, "MixedSection 5 has every section a other verdict!", 0 );
            Reporting.SectionFinished ( "MixedSection 5.1.1. is inconc!", new Verdict ( VerdictCode.Inconc ) );
            Reporting.SectionFinished ( "MixedSection 5.1. failed!", new Verdict ( VerdictCode.Fail ) );
            Reporting.SectionFinished ( "MixedSection 5. is erronous!", new Verdict ( VerdictCode.Error ) );
		}
		
		
		/// <summary>
		/// Registers the parameters.
		/// </summary>
		private void RegisterParameters ( )
		{
			Factory.GetParameterManager ( ).CreateTpaFile ( );
			Factory.GetParameterManager().Register(m_float);
		
			Factory.GetParameterManager ( ).Save ( );
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
            TC_SectionTest testCase = new TC_SectionTest("SectionTestCase");
			testCase.PerformTest();
			testCase.Pass();
			testCase.Finished();
		}
	}
}
