
using ContextLabCar.Core.Config;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Xml.Linq;

namespace ContextLabCar.Core.Strategies;

public interface IStrategDanJson
{
  void InicialJson(string jsonPant);
}
public class StrategDanJson: IStrategDanJson
{
  private string _jsonPant;

  public StrategDanJson(string jsonPant="")
  {
    _jsonPant = jsonPant;
  }

  protected ConcurrentDictionary<string, dynamic> Params { get; set; }
  protected ConcurrentDictionary<string, dynamic> SetStartStrateg { get; set; }
  protected ConcurrentDictionary<string, IFestWert> FestWert { get; set; }
  protected ConcurrentDictionary<string, DanInput> DDanInput { get; set; }
  protected ConcurrentDictionary<string, DanOutput> DDanOutput { get; set; }
  protected ConcurrentDictionary<string, LTask> DTask { get; set; }


  public void InicialJson(string jsonPant="")
  {
    if (string.IsNullOrEmpty(jsonPant) && string.IsNullOrEmpty(_jsonPant))
      new MyException("Not file JSON", -2);

//    Dictionary<string, dynamic> _ddjson = new();
//    _ddjson.Add("Name", "Swet");
//    _ddjson.Add("Wait0", 1.0);
//    _ddjson.Add("Wait1", 1.5);

//    Dictionary<string, Dictionary<string, dynamic>> _dd0json = new();
//    Dictionary<string, dynamic> _dd0json = new();
//    _dd0json.Add("Params", _ddjson);

    string _pathFile = @"e:\12.json";
//    string json111 = JsonConvert.SerializeObject(_dd0json, Formatting.Indented);
//    File.WriteAllText(_pathFile, json111);
    var xxtxt = File.ReadAllText(_pathFile);
    //    var __xx = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, dynamic>>>(xxtxt);

    var googleSearch1 = JObject.Parse(xxtxt);
    var _za = googleSearch1["Params"].Children().ToList();

//    var _za = googleSearch1.Children().ToList();

    var lsName = _za.Select(item => (string)((JProperty)item).Name).ToList();
    var danJsonBasa = xxtxt.Select(item => lsName.Find(x => x.ToLower().Contains(item))).Where(z => z != null).ToList();

    //var _za1 = ((JToken)_za).Values();

    //    var _za0 = _za.ToObject<Dictionary<string, dynamic>>();

    //    var lsName = zJson.Select(item => (string)((JProperty)item).Name).ToList();

    foreach (var item in _za)
    {
      var _po = ((JProperty)item).Value<Dictionary<string, dynamic>>();
  //  var _ff = ((JToken)item).Last.ToObject<Dictionary<string, dynamic>>();
      //var ppv0 = ((JProperty)item).Name;
      //var ppv01 = ((JProperty)item).First;
      //var ppv1 = ((JToken)item).First;
      //var ppv2 = ((JToken)item).Last;
      //var ppv3 = ((JToken)item.Last).Values<dynamic>();


    }
    var _ls0 = _za.ToList();



    _jsonPant = string.IsNullOrEmpty(jsonPant) ? _jsonPant : string.IsNullOrEmpty(_jsonPant) ? jsonPant : jsonPant;
    
    var _txt = File.ReadAllText(_jsonPant);

//    var _x = JsonConvert.DeserializeObject(_txt);
    JObject googleSearch = JObject.Parse(_txt);

    var zJson0 = googleSearch["STParams"].Children();

    foreach ( var z in zJson0)
    {
      var zz21 = ((JProperty)z).Parent;
      var zz0 = ((JProperty)z).Name;
      var zz1 = ((JProperty)z).First;
    }


//    var zJson = googleSearch["STParams"].Children().ToList();
//    var lsName = zJson.Select(item => (string)((JProperty)item).Name).ToList();



    //    var _params = googleSearch["STParams"].ToObject<Dictionary<string, dynamic>>();
    // var _params = (JsonDictionaryContract)googleSearch["STParams"]["Name"].Values();
    //.Values().ToObject<Dictionary<string, dynamic>>();


  }

}

