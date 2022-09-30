# В строках 5-10 содержатся директории проекта.
# 5 – Путь проекта
# 6 – Путь рабочей области эксперимента
# 7 – Путь с экспериментом
# 8 – Путь с файлом параметров
# 9 – Путь с данными для генератора сигналов
# 10 – Записанные логи
# import COM module for Windows
import win32com.client
# import time module for sleep functionality
import time
Path_LabcarProject = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION"
Path_Workspace = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION"
Path_Experiment = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp"
Path_ParameterFile = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files"
Path_SignalGeneratorData = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\SignalGenerator_Files"
Path_LogFiles = "D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp"

Application = win32com.client.dynamic.Dispatch ("ExperimentEnvironment.Application") # startup
ExperimentEnvironment = Application.Scripting # get root object
# Workspace = ExperimentEnvironment.OpenWorkspace ("c:\\temp\\ee\\workspace.eew") # open workspace
# Experiment = Workspace.OpenExperiment("c:\\temp\\ee\\experiment.eex") # open experiment
Workspace = ExperimentEnvironment.OpenWorkspace("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\AUTOMATION\\AUTOMATION.eew") # open workspace
Experiment = Workspace.OpenExperiment("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\DefaultExp.eex") # open experiment
# alternatively, for acessing an already opened project, simply get the Workspace and Experiment properties
#Workspace = ExperimentEnvironment.Workspace
#Experiment = Workspace.Experiment
SignalSources = Experiment.SignalSources # get all signal sources

SignalSources.Download # download the model to the target
SignalSources.StartSimulation # start simulation on the target
SignalSources.StartMeasurement # start measurement

# get value of measurement variable
Measurement = SignalSources.CreateMeasurement("TEST/Control_Signal/Value", "Acquisition")
time.sleep(3) # wait some time until value is updated from the model execution target
ValueObject = Measurement.GetValueObject
Value = ValueObject.GetValue

# set value of calibration variable (parameter)
Parameter = SignalSources.CreateParameter("TEST/Control_Signal/Value")
ValueObject = Parameter.GetValueObject
ValueObject.SetValue(30.000)
Parameter.SetValueObject(ValueObject)

# download parameters directly from parameter file
Experiment.CalibrationController.LoadParameters("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm")
# add parameter file to experiment explorer
Experiment.AddFile("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm")
Experiment.ActivateFile("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\Parameter_Files\\ParameterFile.dcm", True)


# create a new data logger
Datalogger = Experiment.DataLoggers.CreateDatalogger("MyDataLogger")
Datalogger.AddScalarRecordingSignal("TEST/Result", "")
# get existing data logger
Datalogger = Experiment.DataLoggers.GetDataloggerByName("MyDataLogger")
Datalogger.StartTriggerPreTriggerTime = 5
# this call is needed to apply all configuration setting (trigger, file settings)
Datalogger.ApplyConfiguration
Datalogger.Activate
# start data logger for manual start trigger type
Datalogger.Start
time.sleep(10) #wait
# stop data logger for manual stop trigger type
Datalogger.Stop
time.sleep(3) #wait some time until datalogger post processing is complete

SignalGeneratorConfiguration = Experiment.SignalGeneratorConfiguration
# create a signal generator based on signal description set (LCO use case)
# see interface ISignalGeneratorWithSet in API reference
SignalGenerator = SignalGeneratorConfiguration.SignalGenerators.CreateSignalGenerator(0, "MySignalGenerator")
# or get an existing generator by name
SignalGenerator = SignalGeneratorConfiguration.SignalGenerators.GetSignalGeneratorByName("MySignalGenerator")
SignalGenerator.StartTimeInSeconds = 5
# create signal description set by import of measure file or lcs file
SignalGeneratorConfiguration.SignalDescriptionSets.Import("D:\\Projects\\LABCAR_Model_2022\\AUTOMATION\\AUTOMATION\\Experiments\\DefaultExp\\SignalGenerator_Files\\TEST_Signal_generator.dat")
# create a sine signal description
SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.CreateSignalDescriptionSet("MySignalDescription")
SignalDescription = SignalDescriptionSet.CreateSignalDescription("My Sine Signal")
SignalSegment = SignalDescription.CreateSegment(7)
SignalDescriptionSet = SignalGeneratorConfiguration.SignalDescriptionSets.GetSignalDescriptionSetByName("MySignalDescription")
SignalGenerator.SignalDescriptionSet = SignalDescriptionSet
SignalGenerator.Device = "RTPC"
SignalGenerator.Task = "Acquisition"
SignalGenerator.Start
# for application of the signal generator to your LABCAR ports, please refer to ISignalFlowManager


SignalSources.StopMeasurement # stop measurement
SignalSources.StopSimulation # stop the simulation on the target
SignalSources.Disconnect # disconnect the target

Experiment.Close # close the Experiment
Workspace.Close # close the workspace

ExperimentEnvironment.ShutDown # shut down the application

