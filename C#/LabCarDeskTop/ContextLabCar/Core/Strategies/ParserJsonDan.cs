
namespace ContextLabCar.Core.Strategies;
public class ParserJsonDan: ParserJson
{
  #region ===> Data <===
  #region ==__ Public __==
  #endregion
  #region ___ Local ___
  private const string Path = "Path";
  private const string PTask = "Task";
  private const string Parameter = "Parameter";
  private const string Output = "Output";
  private const string Calibration = "Calibration";

  #endregion
  #endregion

  #region _ Constructor _
  public ParserJsonDan(string pathFiles) : base(pathFiles) { }
  #endregion

  #region ___ run _ convert _ json

  public virtual void AddNewPoleJson(ParserJsonDan parserjson)
  {
    //    var x = parserjson.BasaParams;
  }

  protected override void ConvertJson(string tsxtJson)
  {
    DParameter = new Dictionary<string, Parameter>();
    DDanOutput = new Dictionary<string, DanOutput>();
    DCalibrat = new Dictionary<string, Dictionary<string, Calibrat>>();
    DPath = new Dictionary<string, string>();
    DTask = new Dictionary<string, LTask>();

    var jinfo = JObject.Parse(tsxtJson);
    var basaParams = StartParser(jinfo);

    #region ----  Load  -> Path  -------
    if (basaParams.TryGetValue(Path, out var valuePath))
      DPath = JsonToDicStStr(valuePath.ToString() ?? string.Empty);
    #endregion

    #region ----  Load  -> Task  -------
    if (basaParams.TryGetValue(PTask, out var valueTask))
      CalcPTask(valueTask);
    #endregion

    #region ----  Load  -> Parameter  -------
    if (basaParams.TryGetValue(Parameter, out var valueParameter))
      foreach (var (key, x) in JsonToDicStLsStr(valueParameter.ToString()))
        DParameter.Add(key, new Parameter(x.ElementAt(0), key, x.Count == 2 ? x.ElementAt(1) : ""));

    #endregion

    #region ----  Load  -> Output  -------
    if (basaParams.TryGetValue(Output, out var valueOutput))
      foreach (var (key, x) in JsonToDicStLsStr(valueOutput.ToString()))
        DDanOutput.Add(key, new DanOutput(x.ElementAt(0), key, x.Count == 2 ? x.ElementAt(1) : ""));
    #endregion

    #region ----  Load  -> Calibration  -------
    if (basaParams.TryGetValue(Calibration, out var valueCalibrat))
    {
      CalcCalibrat(valueCalibrat);
      WriteCalibrationDan();
    }
    #endregion

  }
  #endregion

  #region PTask
  private void CalcPTask(object val)
  {
    var basaParams = StartParser(val);

    foreach (var (key, _) in basaParams)
    {
      var x = JsonConvert.DeserializeObject<List<string>>(((JToken)val)[key]?.ToString() ?? string.Empty);
      if (x != null && x.Count < 2)
        continue;

      if (x != null) DTask.Add(key, new LTask(key, x.ElementAt(0), x.ElementAt(1), x.Count == 3 ? x.ElementAt(2) : ""));
    }
  }
  #endregion

  #region Calibration
  private void CalcCalibrat(object val)
  {
    var basaParams = StartParser(val);

    foreach (var (key, valparam) in basaParams)
    {
      Dictionary<string, Calibrat> dan = new();
      var val0 = JsonToDicStDyn(valparam.ToString() ?? string.Empty);
      if (val0 != null)
        foreach (var it in val0)
        {
          var val1 = JsonToDicStDyn(it.Value.ToString());

          // ReSharper disable once PossibleNullReferenceException
          dynamic F0(string s) => val1.TryGetValue(s, out dynamic v) ? v : "";

          dan.Add(it.Key, new Calibrat(F0("Model"), it.Key, F0("Val"), F0("Comment")));
        }

      DCalibrat.Add(key, dan);
    }
  }

  private void WriteCalibrationDan()
  {
    foreach (var (nameFile, val) in DCalibrat)
    {
      string text = "";
      foreach (var (_, vald) in val)
        text += vald.Text;

      if (DPath != null && DPath.TryGetValue(nameFile, out var fullPath))
      {
        var dir = System.IO.Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(dir))
          if (dir != null)
            Directory.CreateDirectory(dir);
        File.WriteAllText(fullPath, text);
      }
      else
      {
        throw new MyException($"В json InicialParams, не прописан путь к переменным {nameFile}", -3);
      }
    }
  }

  #endregion

}