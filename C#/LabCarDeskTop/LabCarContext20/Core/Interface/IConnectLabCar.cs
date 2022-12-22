namespace LabCarContext20.Core.Interface;

public interface IConnectLabCar: ILoggerDisplay
{
    IExperimentEnvironment ExperimentEnvironment { get; set; }
    IWorkspace Workspace { get; set; }
    IExperiment Experiment { get; set; }
    ISignalSourceCollection SignalSources { get; set; }

    void Initialization(string pathWorkspace, string pathExperimentEnvironment);
    void Initialization();
    void Connect();
    void DisConnect();
    void StartSimulation();
    public void StopSimulation();
}