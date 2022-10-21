
using System;
using System.IO;

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
  private const string Calibrat = "Calibrat";

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
    DParameter = new();
    DDanOutput = new();
    DCalibrat = new();
    DPath = new();
    DTask = new();

  JObject? _jinfo = JObject.Parse(tsxtJson);
    var BasaParams = StartParser(_jinfo);

    #region ----  Load  -> Path  -------
    if (BasaParams.TryGetValue(Path, out object valuePath))
      DPath = jsonToDicStStr(valuePath.ToString() ?? string.Empty);
    #endregion

    #region ----  Load  -> Task  -------
    if (BasaParams.TryGetValue(PTask, out object valueTask) && valueTask != null)
      CalcPTask(valueTask);
    #endregion

    #region ----  Load  -> Parameter  -------
    if (BasaParams.TryGetValue(Parameter, out object valueParameter) && valueParameter != null)
      foreach (var (key, x) in jsonToDicStLsStr(valueParameter.ToString()))
        DParameter.Add(key, new Parameter(x.ElementAt(0), key, x.Count == 2 ? x.ElementAt(1) : ""));
    #endregion

    #region ----  Load  -> Output  -------
    if (BasaParams.TryGetValue(Output, out object valueOutput) && valueOutput != null)
      foreach (var (key, x) in jsonToDicStLsStr(valueOutput.ToString()))
        DDanOutput.Add(key, new DanOutput(x.ElementAt(0), key, x.Count == 2 ? x.ElementAt(1) : ""));
    #endregion

    #region ----  Load  -> Calibrat  -------
    if (BasaParams.TryGetValue(Calibrat, out object valueCalibrat) && valueCalibrat != null)
    {
      CalcCalibrat(valueCalibrat);
      writeFestWert();
    }
    #endregion

  }
  #endregion

  //#region function

  //protected Dictionary<string, string> jsonToDicStStr(string name) =>
  //              (Dictionary<string, string>)JsonConvert.DeserializeObject<Dictionary<string, string>>(name); //
  //protected Dictionary<string, List<string>> jsonToDicStLsStr(string? name) =>
  //              (Dictionary<string, List<string>>)JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(name); //
  //  protected List<string>? JsonLsString(string name) => (List<string>)JsonConvert.DeserializeObject<List<string>>(name);
  //protected Dictionary<string, dynamic> jsonToDicStDyn(string name) =>
  //            (Dictionary<string, dynamic>)JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(name); //
  //  protected ConcurrentDictionary<string, object> StartParser(object val)
  //{
  //  ConcurrentDictionary<string, object> basaParams = new();

  //  var lsName = ((JToken)val).Children().ToList().Select(item => ((JProperty)item).Name).ToList();

  //  foreach (var w in lsName.Select(it => ((JToken)val)[it]?.ToString()))
  //    lsName.ForEach(item => basaParams.AddOrUpdate(item, ((JToken)val)[item], (_, _) => ((JToken)val)[item]));
  //  return basaParams;
  //}
  //#endregion   function

  #region PTask
  private void CalcPTask(object val)
  {
    var basaParams = StartParser(val);

    foreach (var (_key, _val) in basaParams)
    {
      var _x = JsonConvert.DeserializeObject<List<string>>(((JToken)val)[_key].ToString());
      if (_x.Count < 2)
        continue;
      DTask.Add(_key, new LTask(_key, _x.ElementAt(0), _x.ElementAt(1), _x.Count == 3 ? _x.ElementAt(2) : ""));
    }
  }
  #endregion

  #region Calibrat
  private void CalcCalibrat(object val)
  {
    var basaParams = StartParser(val);

    foreach (var (_key, _val) in basaParams)
    {
      Dictionary<string, Calibrat> dan = new();
      var val0 = jsonToDicStDyn(_val.ToString());
      foreach (var it in val0)
      {
        var val1 = jsonToDicStDyn(it.Value.ToString());

        dynamic f0(string s) => val1.TryGetValue(s, out dynamic v) ? v : "";

        dan.Add(it.Key, new Calibrat(f0("Model"), it.Key, f0("Val"), f0("Comment")));
      }
      DCalibrat.Add(_key, dan);
    }
  }

  private void writeFestWert()
  {
    foreach (var (nameFile, val) in DCalibrat)
    {
      string _text = "";
      foreach (var (keyd, vald) in val)
        _text += vald.Text;

      if (DPath.TryGetValue(nameFile, out string fullFath))
      {
        var _dir = System.IO.Path.GetDirectoryName(fullFath);
        if (!Directory.Exists(_dir))
          Directory.CreateDirectory(_dir);
        File.WriteAllText(fullFath, _text);
      }
      else
      {
        throw new MyException($"В json InicialParams, не прописан путь к переменным {nameFile}", -3);
      }
    }
  }

  #endregion

}