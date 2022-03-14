import sys
import time
import CLR.Etas.Eas.Atcl.Interfaces as AtclInterfaces
import CLR.Etas.Eas.Atcl.Interfaces.Types as AtclTypes
import CLR.Etas.Eas.Atcl.Interfaces.Types.Datalogger as DataloggerTypes
import CLR.System as ClrSystem



class TCManager (AtclInterfaces.TestCase):

    doneTbInit = False
    
    def Init(self):
        self.PortMA().Timeout = 50000
        if ( self.PortMA().IsToolConfigured == False  ):
            self.PortMA().Create()
            self.PortMA().ConfigureTool("ECU",  [""], ["", "IdleControllerHIL", "default", "default"])
            self.doneTbInit = True
        self.PortMA().Configure([""])

    def PortMA(self):
        return self.Factory().GetPortMA("P_MA")

    def Factory(self):
        return AtclInterfaces.Factory.AtclFactory.GetInstance()

    def SARController(self):
        return self.Factory().GetSARHostController()

    def ParameterManager(self):
        return self.Factory().GetParameterManager()

    def Verdict(self):
        return self.Factory().GetVerdictManager()
    

class TCIdleController(TCManager):

    acSwitch = AtclTypes.TypeSutSwitch("StartAirCondition", "StartAirCondition", "",  "Off", "OnOffSwitch")

    acTorque = AtclTypes.TypeSutFloat ("Torque", "AirConditionTorque", "", -10.0, -100.0, 0.0, "")

    def RegisterParameter(self):
        print "Creating parameters ."
        self.ParameterManager().CreateTpaFile()
        self.ParameterManager().Register(self.acTorque)
        print "."
        self.ParameterManager().Save()
        print " Done"

    def Test(self):

        print "IdleControllerTest"
        enginespeed_min = AtclTypes.TypeSutFloat("Minimum engine speed", "", "", 600.0, 0.0, 1000.0, "")  
        enginespeed_max = AtclTypes.TypeSutFloat("Maximum engine speed", "", "",  800.0, 0.0, 1000.0, "")
        log_filename = "enginespeed.mdf"
        print "Receiving parameter for A/C"
        self.acTorque = self.ParameterManager().Parameterise(self.acTorque)
        print "Setting simulation model parameters ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutBaseArray(
            [self.acTorque,
            AtclTypes.TypeSutFloat("Idle", "IIdle", "", 0.0005, 0.0, 1.0, ""),
            AtclTypes.TypeSutFloat("Pdle", "PIdle", "", 0.02, 0.0, 1.0, "")]))
        print "Switching off all aggregates ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutBaseArray(
            [self.acSwitch,
             AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "Off", "OnOffSwitch"),
             AtclTypes.TypeSutSwitch("StartIgnition", "StartIgnition", "",  "Off", "OnOffSwitch")]))

        print "Configuring datalogger ..."
        self.PortMA().ConfigureDataLogger(DataloggerTypes.TypeDLConfigureRecord(
            "log_filename", 10.0,
            [DataloggerTypes.TypeDLSignal("Engine", "P_MA", "AcquisitionTask")]))
        print "Retrieving the mode of the air conditioning torque input signal ..."
        mode = self.PortMA().GetModelValue(AtclTypes.TypeSutSwitch("", "AirConditionTorque.mode", "",  "const", "mode-switch"))
        print "mode = " + mode.Val
        print "Setting the air condition torque input signal to constant value ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("", "AirConditionTorque.mode", "",  "const", "mode-switch"))
        mode = self.PortMA().GetModelValue(AtclTypes.TypeSutSwitch("", "AirConditionTorque.mode", "",  "const", "mode-switch"))
        print "mode = " + mode.Val
        print "Starting measurement ..."
        self.PortMA().Start()
        print "Switching on ignition ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartIgnition", "StartIgnition", "",  "On", "OnOffSwitch"))
        print "Starting engine ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "On", "OnOffSwitch"))
        print "Waiting 5s to reach idle speed ..."
        time.sleep(5)
        print "Switching off starter ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "Off", "OnOffSwitch"))
        print "Starting datalogger ..."
        self.PortMA().StartDataLogger()
        print "Switching on air conditioning ..."
        self.acSwitch.Val = "On"
        self.PortMA().SetModelValue(self.acSwitch)
        print "Waiting for datalogging to complete ..."
        while self.PortMA().GetDataLoggerState() == 3:
            time.sleep(0.1)
            pass
        print "Stopping datalogger ..."
        self.PortMA().StopDataLogger()
        print "Switching off air conditioning ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartAirCondition", "StartAirCondition", "",  "Off", "OnOffSwitch"))
        print "Stopping measurement ..."
        self.PortMA().Stop()
        print "Getting logged signals ..."
        signals = self.PortMA().GetLoggedSignals("log_filename",
            [AtclTypes.TypeSut1DFloatTable("", "Engine","", [0.0,1.0], [0.0,2.0], 0.0, 0.0, 0.0, 0.0,"", "")])
        print "Evaluation ..."
        x = signals[0].X_val
        y = signals[0].Y_val
        ymin = y[0]
        ymax = y[0]
        for yy in y:
            if yy > ymax:
                ymax = yy
            if yy < ymin:
                ymin = yy
        print "min. engine speed = " + str(ymin)
        print "max. engine speed = " + str(ymax)
        if (ymin < enginespeed_min.Val) or (ymax > enginespeed_max.Val):
            print "engine speed is not within the specified bounds (" + str(enginespeed_min.Val) + "," + str(enginespeed_max.Val) + ")"
            self.Verdict().Fail()
        else:
            print "engine speed is within the specified bounds (" + str(enginespeed_min.Val) + "," + str(enginespeed_max.Val) + ")"
            self.Verdict().Pass()
        print "VERDICT = " + self.Verdict().GetVerdict()
        print "Stopping measurement ..."
        self.PortMA().Stop()        
        print "Done"
        if ( self.doneTbInit == True ):
            self.PortMA().Close()
            


if __name__ == "__main__":

    sarController = AtclInterfaces.Factory.AtclFactory.GetInstance("TC_PY_IdleController").GetSARHostController()
    sarStarted = False
    if ( sarController.IsSARProcessRunning() == False):
        raw_input('Press any to start TC')
        print "Initializing LABCAR-AUTOMATION ..."
        sarController.StartSARHostProcess()
        sarConfig = AtclInterfaces.Configuration.SarConfiguration()
        sarConfig.TbcFileName = "C:/ETASData/IdleControllerDemo/TestBenchConfig/LabCar.tbc"
        sarConfig.ReportFileName = "c:/temp"
        sarController.InitializeSAR(sarConfig)
        sarStarted = True

    tc = TCIdleController("TC_PY_IdleController")
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("TCD","Markus Schumacher ITD"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Version","0.9"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Purpose","Idlecontroller in PythonNET for Tutorial "))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Comment","TBInit/NoTbInit TC"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("ChangeLog","Created"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Label","TC_PY_IdleController"))
    try:
        tc.RegisterParameter()
        tc.Init()
        tc.Test()
        tc.Finished()
        if (sarStarted == True):
            sarController.ShutdownSARHostProcess()
            raw_input('Press any key to terminate')
    except ClrSystem.Exception, e:
        tc.Error()
        tc.Finished()
        print e.Message
        tc.Reporting.LogExtension ("Exception occured in Test Case: " + e.Message)
        if (sarStarted == True):
            sarController.ShutdownSARHostProcess()
            raw_input('Press any key to terminate')
        

