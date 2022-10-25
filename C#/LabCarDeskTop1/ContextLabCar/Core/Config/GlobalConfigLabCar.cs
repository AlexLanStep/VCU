
namespace ContextLabCar.Core.Config;

public class LabCarTaskName
{
  public string PathTask { get; set; }
  public string NameInLabCar { get; set; }
  public string Comment { get; set; }
}
public class GlobalConfigLabCar
{
  public GlobalConfigLabCar()
  {
  }

  public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
  public Dictionary<string, LabCarTaskName> LabCarTask { get; set; } = new Dictionary<string, LabCarTaskName>();
}


/*
 
   "Task": {
    "VCU_DesInvMode": [ "TEST/Low_Beam_Test/VCU_DesInvMode", "Acquisition", "Test 001" ],
    "Low_Beam_Req": [ "TEST/Low_Beam_Test/Low_Beam_Req", "Acquisition" ],
    "Low_Beam_State": [ "TEST/Low_Beam_Test/Low_Beam_State", "Acquisition", "Test 1" ]
  },
 
 */