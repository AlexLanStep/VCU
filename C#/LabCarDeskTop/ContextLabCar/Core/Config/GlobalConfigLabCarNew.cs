
namespace ContextLabCar.Core.Config;

public class GlobalConfigLabCarNew
{
  public GlobalConfigLabCarNew()
  {
  }

  public Dictionary<string, string> PathLabCar { get; set; } = new Dictionary<string, string>();
  public Dictionary<string, TaskJsonLoad> LabCarTask { get; set; } = new Dictionary<string, TaskJsonLoad>();
}


/*
 
   "Task": {
    "VCU_DesInvMode": [ "TEST/Low_Beam_Test/VCU_DesInvMode", "Acquisition", "Test 001" ],
    "Low_Beam_Req": [ "TEST/Low_Beam_Test/Low_Beam_Req", "Acquisition" ],
    "Low_Beam_State": [ "TEST/Low_Beam_Test/Low_Beam_State", "Acquisition", "Test 1" ]
  },
 
 */