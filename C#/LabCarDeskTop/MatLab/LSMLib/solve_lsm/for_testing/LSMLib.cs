/*
* MATLAB Compiler: 8.2 (R2021a)
* Date: Wed Dec  7 19:26:01 2022
* Arguments:
* "-B""macro_default""-W""dotnet:LSMLib,LSMLib,4.0,private,version=1.0""-T""link:lib""-d""
* E:\LabCar\C#\LabCarDeskTop\MatLab\LSMLib\solve_lsm\for_testing""-v""class{LSMLib:E:\LabC
* ar\C#\LabCarDeskTop\MatLab\solve_lsm.m}"
*/
using System;
using System.Reflection;
using System.IO;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;

#if SHARED
[assembly: System.Reflection.AssemblyKeyFile(@"")]
#endif

namespace LSMLib
{

  /// <summary>
  /// The LSMLib class provides a CLS compliant, MWArray interface to the MATLAB
  /// functions contained in the files:
  /// <newpara></newpara>
  /// E:\LabCar\C#\LabCarDeskTop\MatLab\solve_lsm.m
  /// </summary>
  /// <remarks>
  /// @Version 1.0
  /// </remarks>
  public class LSMLib : IDisposable
  {
    #region Constructors

    /// <summary internal= "true">
    /// The static constructor instantiates and initializes the MATLAB Runtime instance.
    /// </summary>
    static LSMLib()
    {
      if (MWMCR.MCRAppInitialized)
      {
        try
        {
          Assembly assembly= Assembly.GetExecutingAssembly();

          string ctfFilePath= assembly.Location;

		  int lastDelimiter = ctfFilePath.LastIndexOf(@"/");

	      if (lastDelimiter == -1)
		  {
		    lastDelimiter = ctfFilePath.LastIndexOf(@"\");
		  }

          ctfFilePath= ctfFilePath.Remove(lastDelimiter, (ctfFilePath.Length - lastDelimiter));

          string ctfFileName = "LSMLib.ctf";

          Stream embeddedCtfStream = null;

          String[] resourceStrings = assembly.GetManifestResourceNames();

          foreach (String name in resourceStrings)
          {
            if (name.Contains(ctfFileName))
            {
              embeddedCtfStream = assembly.GetManifestResourceStream(name);
              break;
            }
          }
          mcr= new MWMCR("",
                         ctfFilePath, embeddedCtfStream, true);
        }
        catch(Exception ex)
        {
          ex_ = new Exception("MWArray assembly failed to be initialized", ex);
        }
      }
      else
      {
        ex_ = new ApplicationException("MWArray assembly could not be initialized");
      }
    }


    /// <summary>
    /// Constructs a new instance of the LSMLib class.
    /// </summary>
    public LSMLib()
    {
      if(ex_ != null)
      {
        throw ex_;
      }
    }


    #endregion Constructors

    #region Finalize

    /// <summary internal= "true">
    /// Class destructor called by the CLR garbage collector.
    /// </summary>
    ~LSMLib()
    {
      Dispose(false);
    }


    /// <summary>
    /// Frees the native resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true);

      GC.SuppressFinalize(this);
    }


    /// <summary internal= "true">
    /// Internal dispose function
    /// </summary>
    protected virtual void Dispose(bool disposing)
    {
      if (!disposed)
      {
        disposed= true;

        if (disposing)
        {
          // Free managed resources;
        }

        // Free native resources
      }
    }


    #endregion Finalize

    #region Methods

    /// <summary>
    /// Provides a single output, 0-input MWArrayinterface to the solve_lsm MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray solve_lsm()
    {
      return mcr.EvaluateFunction("solve_lsm", new MWArray[]{});
    }


    /// <summary>
    /// Provides a single output, 1-input MWArrayinterface to the solve_lsm MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="n">Input argument #1</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray solve_lsm(MWArray n)
    {
      return mcr.EvaluateFunction("solve_lsm", n);
    }


    /// <summary>
    /// Provides a single output, 2-input MWArrayinterface to the solve_lsm MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray solve_lsm(MWArray n, MWArray x)
    {
      return mcr.EvaluateFunction("solve_lsm", n, x);
    }


    /// <summary>
    /// Provides a single output, 3-input MWArrayinterface to the solve_lsm MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <param name="y">Input argument #3</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray solve_lsm(MWArray n, MWArray x, MWArray y)
    {
      return mcr.EvaluateFunction("solve_lsm", n, x, y);
    }


    /// <summary>
    /// Provides a single output, 4-input MWArrayinterface to the solve_lsm MATLAB
    /// function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <param name="y">Input argument #3</param>
    /// <param name="wts">Input argument #4</param>
    /// <returns>An MWArray containing the first output argument.</returns>
    ///
    public MWArray solve_lsm(MWArray n, MWArray x, MWArray y, MWArray wts)
    {
      return mcr.EvaluateFunction("solve_lsm", n, x, y, wts);
    }


    /// <summary>
    /// Provides the standard 0-input MWArray interface to the solve_lsm MATLAB function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] solve_lsm(int numArgsOut)
    {
      return mcr.EvaluateFunction(numArgsOut, "solve_lsm", new MWArray[]{});
    }


    /// <summary>
    /// Provides the standard 1-input MWArray interface to the solve_lsm MATLAB function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="n">Input argument #1</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] solve_lsm(int numArgsOut, MWArray n)
    {
      return mcr.EvaluateFunction(numArgsOut, "solve_lsm", n);
    }


    /// <summary>
    /// Provides the standard 2-input MWArray interface to the solve_lsm MATLAB function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] solve_lsm(int numArgsOut, MWArray n, MWArray x)
    {
      return mcr.EvaluateFunction(numArgsOut, "solve_lsm", n, x);
    }


    /// <summary>
    /// Provides the standard 3-input MWArray interface to the solve_lsm MATLAB function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <param name="y">Input argument #3</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] solve_lsm(int numArgsOut, MWArray n, MWArray x, MWArray y)
    {
      return mcr.EvaluateFunction(numArgsOut, "solve_lsm", n, x, y);
    }


    /// <summary>
    /// Provides the standard 4-input MWArray interface to the solve_lsm MATLAB function.
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return.</param>
    /// <param name="n">Input argument #1</param>
    /// <param name="x">Input argument #2</param>
    /// <param name="y">Input argument #3</param>
    /// <param name="wts">Input argument #4</param>
    /// <returns>An Array of length "numArgsOut" containing the output
    /// arguments.</returns>
    ///
    public MWArray[] solve_lsm(int numArgsOut, MWArray n, MWArray x, MWArray y, MWArray 
                         wts)
    {
      return mcr.EvaluateFunction(numArgsOut, "solve_lsm", n, x, y, wts);
    }


    /// <summary>
    /// Provides an interface for the solve_lsm function in which the input and output
    /// arguments are specified as an array of MWArrays.
    /// </summary>
    /// <remarks>
    /// This method will allocate and return by reference the output argument
    /// array.<newpara></newpara>
    /// </remarks>
    /// <param name="numArgsOut">The number of output arguments to return</param>
    /// <param name= "argsOut">Array of MWArray output arguments</param>
    /// <param name= "argsIn">Array of MWArray input arguments</param>
    ///
    public void solve_lsm(int numArgsOut, ref MWArray[] argsOut, MWArray[] argsIn)
    {
      mcr.EvaluateFunction("solve_lsm", numArgsOut, ref argsOut, argsIn);
    }



    /// <summary>
    /// This method will cause a MATLAB figure window to behave as a modal dialog box.
    /// The method will not return until all the figure windows associated with this
    /// component have been closed.
    /// </summary>
    /// <remarks>
    /// An application should only call this method when required to keep the
    /// MATLAB figure window from disappearing.  Other techniques, such as calling
    /// Console.ReadLine() from the application should be considered where
    /// possible.</remarks>
    ///
    public void WaitForFiguresToDie()
    {
      mcr.WaitForFiguresToDie();
    }



    #endregion Methods

    #region Class Members

    private static MWMCR mcr= null;

    private static Exception ex_= null;

    private bool disposed= false;

    #endregion Class Members
  }
}
