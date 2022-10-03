using System;
using System.Reflection;
//using ETAS.EE.RealTimePlugin;

namespace LabCar // Note: actual namespace depends on the project name.
{

  internal class Test01
  {
    #region COM Wrapper Methods

    private static object COMInvokeMember(object comObject, string memberName, BindingFlags flagsToInvoke, object[] parameters)
    { return comObject.GetType().InvokeMember(memberName, flagsToInvoke, null, comObject, parameters); }

    private static object GetCOMProperty(object comObject, string propertyName, object[] parameters)
    { return COMInvokeMember(comObject, propertyName, BindingFlags.GetProperty, parameters); }

    private static object SetCOMProperty(object comObject, string propertyName, object[] parameters)
    { return COMInvokeMember(comObject, propertyName, BindingFlags.SetProperty, parameters); }

    private static object CallCOMMethod(object comObject, string methodName, object[] parameters)
    { return COMInvokeMember(comObject, methodName, BindingFlags.InvokeMethod, parameters); }

    #endregion

    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");


      object application = Activator.CreateInstance(Type.GetTypeFromProgID("ExperimentEnvironment.Application"));// startup
      object experimentEnvironment = GetCOMProperty(application, "Scripting", null);// get root object
      object workspace = CallCOMMethod(experimentEnvironment, "OpenWorkspace", new object[] { @"c:\temp\ee\workspace.eew" });// open workspace
      object experiment = CallCOMMethod(workspace, "OpenExperiment", new object[] { @"c:\temp\ee\experiment.eex" });// open experiment
      object signalSources = GetCOMProperty(experiment, "SignalSources", null);// get all signal sources

      CallCOMMethod(signalSources, "Download", null);// download the model to the target
      CallCOMMethod(signalSources, "StartSimulation", null);// start simulation on the target
      CallCOMMethod(signalSources, "StartMeasurement", null);// start measurement

      // get value of measurement variable
      object measurement = CallCOMMethod(signalSources, "CreateMeasurement", new object[] { "VariableLabel", "Task" });
      System.Threading.Thread.Sleep(3000); // wait some time until value is updated from the model execution target
      object valueObject = CallCOMMethod(measurement, "GetValueObject", null);
      double value = (double)CallCOMMethod(valueObject, "GetValue", null);

      // set value of calibration variable (parameter)
      object parameter = CallCOMMethod(signalSources, "CreateParameter", new object[] { "VariableLabel" });
      valueObject = CallCOMMethod(parameter, "GetValueObject", null);
      CallCOMMethod(valueObject, "SetValue", new object[] { 2.000 });
      CallCOMMethod(parameter, "SetValueObject", new object[] { valueObject });

      CallCOMMethod(signalSources, "StopMeasurement", null);// stop measurement
      CallCOMMethod(signalSources, "StopSimulation", null);// stop the simulation on the target
      CallCOMMethod(signalSources, "Disconnect", null);// disconnect the target

      CallCOMMethod(experiment, "Close", null);// close the Experiment
      CallCOMMethod(workspace, "Close", null);// close the workspace

      CallCOMMethod(experimentEnvironment, "ShutDown", null);// shut down the application

    }
  }
}


/*
  
using System;
using System.Reflection;

#region COM Wrapper Methods

private static object COMInvokeMember ( object comObject, string memberName, BindingFlags flagsToInvoke, object[] parameters )
{return comObject.GetType ( ).InvokeMember ( memberName, flagsToInvoke, null, comObject, parameters );}

private static object GetCOMProperty ( object comObject, string propertyName, object[] parameters )
{return COMInvokeMember ( comObject, propertyName, BindingFlags.GetProperty, parameters );}

private static object SetCOMProperty ( object comObject, string propertyName, object[] parameters )
{return COMInvokeMember ( comObject, propertyName, BindingFlags.SetProperty, parameters );}

private static object CallCOMMethod(object comObject, string methodName, object[] parameters)
{return COMInvokeMember(comObject, methodName, BindingFlags.InvokeMethod, parameters);}

#endregion

private void ExperimentEnvironment ()
{
    object application = Activator.CreateInstance ( Type.GetTypeFromProgID ( "ExperimentEnvironment.Application" ) );// startup
    object experimentEnvironment = GetCOMProperty ( application, "Scripting", null );// get root object
    object workspace = CallCOMMethod ( experimentEnvironment, "OpenWorkspace", new object[] {@"c:\temp\ee\workspace.eew"} );// open workspace
    object experiment = CallCOMMethod ( workspace, "OpenExperiment",new object[]{@"c:\temp\ee\experiment.eex"} );// open experiment
    object signalSources = GetCOMProperty ( experiment, "SignalSources", null );// get all signal sources

    CallCOMMethod ( signalSources, "Download", null );// download the model to the target
    CallCOMMethod ( signalSources, "StartSimulation", null );// start simulation on the target
    CallCOMMethod ( signalSources, "StartMeasurement", null );// start measurement

    // get value of measurement variable
    object measurement = CallCOMMethod ( signalSources, "CreateMeasurement", new object[]{"VariableLabel", "Task"} );
    System.Threading.Thread.Sleep(3000); // wait some time until value is updated from the model execution target
    object valueObject = CallCOMMethod ( measurement, "GetValueObject", null );
    double value = (double) CallCOMMethod ( valueObject, "GetValue", null );

    // set value of calibration variable (parameter)
    object parameter = CallCOMMethod ( signalSources, "CreateParameter", new object[]{"VariableLabel"} );
    object valueObject = CallCOMMethod ( parameter, "GetValueObject", null );
    CallCOMMethod ( valueObject, "SetValue", new object[]{2.000} );
    CallCOMMethod ( parameter, "SetValueObject", valueObject );

    CallCOMMethod ( signalSources, "StopMeasurement", null );// stop measurement
    CallCOMMethod ( signalSources, "StopSimulation", null );// stop the simulation on the target
    CallCOMMethod ( signalSources, "Disconnect", null );// disconnect the target

    CallCOMMethod ( experiment, "Close", null );// close the Experiment
    CallCOMMethod ( workspace, "Close", null );// close the workspace

    CallCOMMethod( experimentEnvironment, "ShutDown", null );// shut down the application
}


 */