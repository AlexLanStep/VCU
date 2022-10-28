namespace TestContextLabCar;

internal class PrimerTest
{
}

/*
 using System;
using System.Reflection;
using ETAS.EE.Scripting;

namespace LabCar // Note: actual namespace depends on the project name.
{
  internal class Test02
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");


      IExperimentEnvironment experimentEnvironment = EEFactory.GetInstance();// get root object
            IWorkspace workspace=null;
            if (experimentEnvironment.HasOpenWorkspace)
            {
                workspace = experimentEnvironment.Workspace;
            }
            else
            {
                workspace = experimentEnvironment.OpenWorkspace(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew");// open workspace
            }

            IExperiment experiment = null;
            if (workspace.HasOpenExperiment)
            {
                experiment = workspace.Experiment;
            }
            else
            {
                experiment = workspace.OpenExperiment(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex");// open experiment
            }

            
                                                                                                                                                             // alternatively, for acessing an already opened project, simply get the Workspace and Experiment properties
                                                                                                                                                             //Workspace = ExperimentEnvironment.Workspace;
            ISignalSourceCollection signalSources;
            signalSources = experiment.SignalSources;
//            try
//            {
////                ISignalSourceCollection signalSources = experiment.SignalSources;// get all signal sources
//                signalSources = experiment.SignalSources;// get all signal sources


            //            }
            //            catch (Exception)
            //            {
            //                signalSources = experiment.SignalSources;

            //            }          

      signalSources.Download();// download the model to the target
      signalSources.StartSimulation();// start simulation on the target
      signalSources.StartMeasurement();// start measurement

      // get value of measurement variable
      ISignal measurement = signalSources.CreateMeasurement("TEST/Control_Signal/Value", "Acquisition");
      System.Threading.Thread.Sleep(3000); // wait some time until value is updated from the model execution target
      IScalarValue valueObject = (IScalarValue)measurement.GetValueObject();
      double value = valueObject.GetValue();

      // set value of calibration variable (parameter)
      ISignal parameter = signalSources.CreateParameter("TEST/Control_Signal/Value");
      valueObject = (IScalarValue)parameter.GetValueObject();
      valueObject.SetValue(2.000);
      parameter.SetValueObject(valueObject);

      // download parameters directly from parameter file
      experiment.CalibrationController.LoadParameters(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm");
      // add parameter file to experiment explorer
      experiment.AddFile(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm");
      experiment.ActivateFile(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm", true);

      // create a new data logger

      IDataLogger Datalogger = experiment.DataLoggers.CreateDatalogger("MyDataLogger");
      Datalogger.AddScalarRecordingSignal("TEST/Result", "");
      // get existing data logger
      Datalogger = experiment.DataLoggers.GetDataloggerByName("MyDataLogger");
      Datalogger.StartTriggerPreTriggerTime = 5;
      // this call is needed to apply all configuration setting (trigger, file settings)
      Datalogger.ApplyConfiguration();
      Datalogger.Activate();
      // start data logger for manual start trigger type
      Datalogger.Start();
      System.Threading.Thread.Sleep(10000); //wait
                                            // stop data logger for manual stop trigger type
      Datalogger.Stop();
      System.Threading.Thread.Sleep(3000); //wait some time until datalogger post processing is complete

      ISignalGeneratorConfig SignalGeneratorConfiguration = experiment.SignalGeneratorConfiguration;
      // create a signal generator based on signal description set (LCO use case)
      // see interface ISignalGeneratorWithSet in API reference
      ISignalGeneratorWithSet SignalGenerator = (ISignalGeneratorWithSet)SignalGeneratorConfiguration.SignalGenerators.CreateSignalGenerator(ESignalGeneratorType.SignalGeneratorWithSet, "MySignalGenerator");
      // or get an existing generator by name
      SignalGenerator = (ISignalGeneratorWithSet)SignalGeneratorConfiguration.SignalGenerators.GetSignalGeneratorByName("MySignalGenerator");
      SignalGenerator.StartTimeInSeconds = 5;
      // create signal description set by import of measure file or lcs file
      SignalGeneratorConfiguration.SignalDescriptionSets.Import(@"D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\SignalGenerator_Files\\TEST_Signal_generator.dat");
      // create a sine signal description
      ISignalDescriptionSet SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.CreateSignalDescriptionSet("MySignalDescription");
      ISignalDescription SignalDescription = SignalDescriptionSet.CreateSignalDescription("My Sine Signal");
      ISignalSegment SignalSegment = SignalDescription.CreateSegment(ESignalWaveForm.Sine);
      SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.GetSignalDescriptionSetByName("MySignalDescription");
      SignalGenerator.SignalDescriptionSet = SignalDescriptionSet;
      SignalGenerator.Device = "RTPC";
      SignalGenerator.Task = "Acquisition";
      SignalGenerator.Start();
      // for application of the signal generator to your LABCAR ports, please refer to ISignalFlowManager


      signalSources.StopMeasurement();// stop measurement
      signalSources.StopSimulation();// stop the simulation on the target
      signalSources.Disconnect();// disconnect the target

      experiment.Close();// close the Experiment
      workspace.Close();// close the workspace

      experimentEnvironment.ShutDown();// shut down the application

    }
  }
}


 
using System;
using System.Reflection;
using ETAS.EE.Scripting;

private void ExperimentEnvironment ()
{
    
//     Note: For this example you have to include the EE\EE.Sripting.interfaces.dll in your project.
//     This interface file belongs to a special version of the application and will ONLY work with this version.
    

IExperimentEnvironment experimentEnvironment = EEFactory.GetInstance();// get root object
IWorkspace workspace = experimentEnvironment.OpenWorkspace(@"c:\temp\ee\workspace.eew");// open workspace
IExperiment experiment = workspace.OpenExperiment(@"c:\temp\ee\experiment.eex");// open experiment
                                                                                // alternatively, for acessing an already opened project, simply get the Workspace and Experiment properties
                                                                                //Workspace = ExperimentEnvironment.Workspace;
                                                                                //Experiment = Workspace.Experiment;
ISignalSourceCollection signalSources = experiment.SignalSources;// get all signal sources

signalSources.Download();// download the model to the target
signalSources.StartSimulation();// start simulation on the target
signalSources.StartMeasurement();// start measurement

// get value of measurement variable
ISignal measurement = signalSources.CreateMeasurement("VariableLabel", "Task");
System.Threading.Thread.Sleep(3000); // wait some time until value is updated from the model execution target
IScalarValue valueObject = (IScalarValue)measurement.GetValueObject();
double value = valueObject.GetValue();

// set value of calibration variable (parameter)
ISignal parameter = signalSources.CreateParameter("VariableLabel");
valueObject = (IScalarValue)parameter.GetValueObject();
valueObject.SetValue(2.000);
parameter.SetValueObject(valueObject);

// download parameters directly from parameter file
experiment.CalibrationController.LoadParameters(@"C:\MyPath\AllTables.dcm");
// add parameter file to experiment explorer
experiment.AddFile(@"C:\MyPath\AllTables.dcm");
experiment.ActivateFile(@"C:\MyPath\AllTables.dcm", true);

// create a new data logger
IDataLogger Datalogger = experiment.DataLoggers.CreateDatalogger("MyDataLogger");
Datalogger.AddScalarRecordingSignal("AMod_AllDataTypes/Coupling_CT_DepPara_BDE/Output", "");
// get existing data logger
Datalogger = experiment.DataLoggers.GetDataloggerByName("MyDataLogger");
Datalogger.StartTriggerPreTriggerTime = 5;
// this call is needed to apply all configuration setting (trigger, file settings)
Datalogger.ApplyConfiguration();
Datalogger.Activate();
// start data logger for manual start trigger type
Datalogger.Start();
System.Threading.Thread.Sleep(10000); //wait
                                     // stop data logger for manual stop trigger type
Datalogger.Stop();
System.Threading.Thread.Sleep(3000); //wait some time until datalogger post processing is complete

ISignalGeneratorConfig SignalGeneratorConfiguration = experiment.SignalGeneratorConfiguration;
// create a signal generator based on signal description set (LCO use case)
// see interface ISignalGeneratorWithSet in API reference
ISignalGeneratorWithSet SignalGenerator = (ISignalGeneratorWithSet)SignalGeneratorConfiguration.SignalGenerators.CreateSignalGenerator(ESignalGeneratorType.SignalGeneratorWithSet, "MySignalGenerator");
// or get an existing generator by name
SignalGenerator = (ISignalGeneratorWithSet)SignalGeneratorConfiguration.SignalGenerators.GetSignalGeneratorByName("MySignalGenerator");
SignalGenerator.StartTimeInSeconds = 5;
// create signal description set by import of measure file or lcs file
SignalGeneratorConfiguration.SignalDescriptionSets.Import(@"c:\MyPath\MeasureFile.dat");
// create a sine signal description
ISignalDescriptionSet SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.CreateSignalDescriptionSet("MySignalDescription");
ISignalDescription SignalDescription = SignalDescriptionSet.CreateSignalDescription("My Sine Signal");
ISignalSegment SignalSegment = SignalDescription.CreateSegment(ESignalWaveForm.Sine);
SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.GetSignalDescriptionSetByName("Stimulation_Set1");
SignalGenerator.SignalDescriptionSet = SignalDescriptionSet;
SignalGenerator.Device = "RTPC";
SignalGenerator.Task = "Acquisition";
SignalGenerator.Start();
// for application of the signal generator to your LABCAR ports, please refer to ISignalFlowManager


signalSources.StopMeasurement();// stop measurement
signalSources.StopSimulation();// stop the simulation on the target
signalSources.Disconnect();// disconnect the target

experiment.Close();// close the Experiment
workspace.Close();// close the workspace

experimentEnvironment.ShutDown();// shut down the application
}

*/

