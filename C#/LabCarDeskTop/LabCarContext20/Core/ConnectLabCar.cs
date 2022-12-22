﻿
namespace LabCarContext20.Core;

public class ConnectLabCar: IConnectLabCar
{
  #region === Data ===
     #region ===- public -===
  public IExperimentEnvironment ExperimentEnvironment { get; set; }
  public IWorkspace Workspace { get; set; }
  public IExperiment Experiment { get; set; }
  public ISignalSourceCollection SignalSources { get; set; }
  #endregion

  #region ===- local -==
  private readonly ILoggerDisplay _iloggerDisplay;
  private readonly ICPathLc _icPathLc;

  private string _pathWorkspace;
  private string _pathExperimentEnvironment;

    public event AsyncEventHandler? AsyncEvent;
    #endregion
    #endregion


#pragma warning disable CS8618
    public ConnectLabCar(ILoggerDisplay loggerDisplay, ICPathLc icpathlc)
#pragma warning restore CS8618
  {
    _pathWorkspace="";
    _pathExperimentEnvironment="";
    _iloggerDisplay=loggerDisplay;
    _icPathLc=icpathlc;
    _pathWorkspace = _icPathLc.Workspace;
    _pathExperimentEnvironment = _icPathLc.Experiment;

  }

  public void Initialization(string pathWorkspace, string pathExperimentEnvironment)
  {
    _pathWorkspace = pathWorkspace;
    _pathExperimentEnvironment = pathExperimentEnvironment;
    Initialization();
  }
  public void Initialization()
  {
//    _pathWorkspace = pathWorkspace;
//    _pathExperimentEnvironment = pathExperimentEnvironment;

    try
    {
      ExperimentEnvironment = EEFactory.GetInstance();// get root object
    }
    catch
    {
      throw new MyException("Error EEFactory.GetInstance - Not in conp .", -1);
    }

    Workspace = ExperimentEnvironment.HasOpenWorkspace ? ExperimentEnvironment.Workspace : ExperimentEnvironment.OpenWorkspace(_pathWorkspace);

    Experiment = Workspace.HasOpenExperiment ? Workspace.Experiment : Workspace.OpenExperiment(_pathExperimentEnvironment);

    SignalSources = Experiment.SignalSources;
  }


  public void Connect()
  {
    SignalSources.Download();           // download the model to the target
    SignalSources.StartSimulation();    // start simulation on the target
    SignalSources.StartMeasurement();   // start measurement
  }
  public void StartSimulation() 
  {
    try
    {
      SignalSources.StartSimulation();    // start simulation on the target
    }
    catch (Exception)
    {
      // ignored
    }
  }
  public void StopSimulation() 
  { 
    try
    {
      SignalSources.StopSimulation();
    }
    catch (Exception)
    {
      // ignored
    }
  }
  public void DisConnect()
  {
    Experiment.Close();                 // close the Experiment
    Workspace.Close();                  // close the workspace
    ExperimentEnvironment.ShutDown();   // shut down the application
  }

    public void InitializationConsole()=>_iloggerDisplay.InitializationConsole();
    public void InitializationWindows()=>_iloggerDisplay.InitializationWindows();
    public void Write(string str)=>_iloggerDisplay.Write(str);
}



/*
                                                                                                                                                             //Workspace = ExperimentEnvironment.Workspace;


      // get value of measurement variable ---!!!!  это TASK
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


 */