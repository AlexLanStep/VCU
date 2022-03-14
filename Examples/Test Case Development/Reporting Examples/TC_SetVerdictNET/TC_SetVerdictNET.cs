using System;
using System.Threading;
using Etas.Eas.Atcl.Interfaces;
using Etas.Eas.Atcl.Interfaces.Types;
using Etas.Eas.Atcl.Interfaces.Verdicts;

namespace TC_SetVerdictNET
{
	/// <summary>
	/// Simple SetVerdict test case.
	/// </summary>
	class TC_SetVerdictNET : TestCase
	{
		private TypeSutCollectionUserRecord m_verdictParameter = new TypeSutCollectionUserRecord ("Verdict","The result of the Test Case");
		private TypeSutInteger m_durationParameter = new TypeSutInteger ("Duration","Duration","Duration time in seconds for this test case. Zero means as fast as possible",0,0,int.MaxValue ,"");
		
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			TC_SetVerdictNET tc = new TC_SetVerdictNET ( );
			tc.PerformTest ( );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TC_SetVerdictNET"/> class.
		/// </summary>
		public TC_SetVerdictNET ( ) : base ( "TC_SetVerdictNET" ) { }
		
		/// <summary>
		/// Performs the test.
		/// </summary>
		public void PerformTest ( )
		{
			Init ( );
			
			if ( m_durationParameter.Val != 0 )
				Thread.Sleep ( m_durationParameter.Val * 1000 );
			
			Factory.GetVerdictManager ( ).SetTestCaseVerdict (
				new Verdict ( ( VerdictCode ) Enum.Parse ( typeof ( VerdictCode ), m_verdictParameter.Selected.Val, true ) ) );
			
			Finished ( );
		}

		/// <summary>
		/// Inits this instance.
		/// </summary>
		protected void Init ()
		{
			RegisterMetaData ( );
			RegisterPorts ( );
			RegisterParameters ( );
			
			LoadParameters ( );
		}
		
		#region MetaData
		
		/// <summary>
		/// Registers the meta data.
		/// </summary>
		protected void RegisterMetaData ()
		{
			this.AddMetaData ( "Label", "TC_SetVerdictNET" );
			this.AddMetaData ( "Purpose", "Simple Verdict Test Case" );
			this.AddMetaData ( "Comment", "Test Case which finishes with the parameterized verdict." );
			this.AddMetaData ( "TCD", "Alexander Mayer (extern IT-Designers GmbH)" );
			this.AddMetaData ( "Version", this.GetType ( ).Assembly.GetName ( ).Version.ToString ( ) );
		}
		
		#endregion

		#region Ports
		
		/// <summary>
		/// Registers the ports.
		/// </summary>
		protected void RegisterPorts ()
		{
			// no ports in this test 
		}
		
		#endregion
		
		#region Parameters
		
		/// <summary>
		/// Registers the parameters.
		/// </summary>
		protected void RegisterParameters ()
		{	
			Factory.GetParameterManager ( ).CreateTpaFile ( );
			
			m_verdictParameter.Selection = new TypeSutStringArray ( 
											"Verdicts", 
											"Verdicts", 
											"", 
											new string [ ] { "Pass", "Fail", "Inconc", "Error", "None" },
											"",
											"" );
			m_verdictParameter.Selected = new TypeSutString ( "Current Verdict", "Current Verdict", "", "Pass", "", "" );
			
			Factory.GetParameterManager ( ).Register ( m_verdictParameter );
			Factory.GetParameterManager ( ).Register ( m_durationParameter );
			Factory.GetParameterManager ( ).Save ( );
		}
		
		/// <summary>
		/// Loads the parameters.
		/// </summary>
		protected void LoadParameters ()
		{
			m_verdictParameter = ( TypeSutCollectionUserRecord ) Factory.GetParameterManager ( ).Parameterise ( m_verdictParameter );
			m_durationParameter = ( TypeSutInteger ) Factory.GetParameterManager ( ).Parameterise ( m_durationParameter );
		}
		
		#endregion
	}
}
