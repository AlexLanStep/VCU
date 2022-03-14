#++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
# Test Case Template for Generating a Python script fully compliant to
# LABCAR-AUTOMATION V3
#
# The template consists of 2 Class Definitions 
# The first one (TCManager) contains functions for accessing and starting the connections to LABCAR-AUTOMATION.
# This one needs no user changes.
# The second one is the TestCase itself, which derives from the first one and contains the
# basic test functionality
#
# The "main" function is the entry point for execution.
# No change is required here (except for Test Case names)
#
#+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


#+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
# used libraries (including ATCL and Python.NET)
#   libraries may be added on demand
#
import sys
import time
import CLR.Etas.Eas.Atcl.Interfaces as AtclInterfaces           
import CLR.Etas.Eas.Atcl.Interfaces.Types as AtclTypes
import CLR.Etas.Eas.Atcl.Interfaces.Verdicts as AtclVerdict
import CLR.System as ClrSystem



#+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
# TCManager 
#   contains all functionality required generally,
#   you should not change the general structure here
#
class TCManager (AtclInterfaces.TestCase):

    doneTbInit = False  # internally used to indicate the state of "Test Bench initialization"

    # Initialisation of ports in case the Test Handler or a TB Init has not worked    
    def Init(self):
        self.PortMA().Timeout = 50000   #Time allowed for executing a signature.
        if ( self.PortMA().IsToolConfigured == False):
            self.PortMA().Create()
            self.PortMA().ConfigureTool("ECU",  [""], ["", "default", "default", "default"])
            self.doneTbInit = True
        self.PortMA().Configure([""])

    def PortMA(self):
        return self.Factory().GetPortMA("P_MA")  # retrieve a port instance for use in the test

    def Factory(self):
        return AtclInterfaces.Factory.AtclFactory.GetInstance()  # get an ATCL instance

    def SARController(self):
        return self.Factory().GetSARHostController()        # The Core Automation function for distributing signatures to tools

    def ParameterManager(self):
        return self.Factory().GetParameterManager()

    def ReportingManager(self):
        return self.Factory().GetReportManager().GetReportEngine()

    def Verdict(self):
        return self.Factory().GetVerdictManager()
    




#+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
# TC_myTestCase 
#   The Test Case functionality
#   insert your code here 
#   Reuse the methods RegisterParameter(), Test() and Teardown() for internal structuring
class TC_myTestCase(TCManager):

        

    acSwitch = AtclTypes.TypeSutSwitch("StartAirCondition", "StartAirCondition", "",  "Off", "2pol-Schalter")

    daOfTorqueSets = AtclTypes.TypeSutDynamicArray("List of TorqueSets", "Dynamic Array List of Torque sets",
                                                   AtclTypes.TypeSutUserRecord("TorqueSets","Set of orque elements with limits",
                                                                   [
                                                                     AtclTypes.TypeSutFloat ("Torque", "AirConditionTorque", "The torque of the A/C", -10.0, -100.0, 0.0, ""),
                                                                     AtclTypes.TypeSutFloat ("MinRPMValue", "RPMMinValue", "The minimum rpm to pass the test", 650.0, 300.0, 700.0, ""),
                                                                     AtclTypes.TypeSutFloat ("MaxRPMValue", "RPMMaxValue", "The maximum rpm to pass the test", 750.0, 500.0, 1000.0, "")
                                                                    ]
                                                                    ))
    
    urOfIdleControllerParameters = AtclTypes.TypeSutUserRecord("PIDParams","Set of idlecontoller parameters")

    def RegisterParameter(self):
        print "Creating parameters ."
        self.ParameterManager().CreateTpaFile()
        self.ParameterManager().Register(self.daOfTorqueSets)
        print "."
        self.urOfIdleControllerParameters.Items.Add(AtclTypes.TypeSutFloat("Idle", "IIdle", "", 0.0005, 0.0, 1.0, ""))
        print "."
        self.urOfIdleControllerParameters.Items.Add(AtclTypes.TypeSutFloat("Pdle", "PIdle", "", 0.02, 0.0, 1.0, ""))
        print "."
        self.ParameterManager().Register(self.urOfIdleControllerParameters)
        print "."
        self.ParameterManager().Save()
        print " Done"

    def Test(self):
        print "IdleControllerTest"
        print "Receiving parameter for A/C"

        # Initializing some Report Functions
        ReportingEngine = self.ReportingManager().GetReportEngine();
        reportPath = "c:/temp/reports";
        self.ReportingManager().ReportPath = reportPath();
        self.ReportingManager().TCReportPath = "report";

        # Doing some report functions
        ReportingEngine.TestCaseCalled("TC_myTestCase", "TC_myTestCase");
        ReportingEngine.SectionBegin("My First Section");
        ReportingEngine.SetText(0,0,"I am reporting hello world!",0);
        ReportingEngine.SaveReport();
        ReportingEngine.SectionFinished("My First Section", "FAIL");


        
##        self.daOfTorqueSets = self.ParameterManager().Parameterise(self.daOfTorqueSets)
##        self.urOfIdleControllerParameters = self.ParameterManager().Parameterise(self.urOfIdleControllerParameters)
##
##        
##        print "Setting simulation model parameters ..."
##        self.PortMA().SetModelValue(AtclTypes.TypeSutBaseArray(
##            [
##            self.urOfIdleControllerParameters.Items.GetItem("Idle"),
##            self.urOfIdleControllerParameters.Items.GetItem("Pdle")
##            ]))
##        i = 0
##        for ur in self.daOfTorqueSets.Items:
##            print ""
##            print "Loop "+str(i)+" started"
##            self.Reporting.SectionBegin ("Test of IdleController with DA entry "+str(i))
##            sectionverdict = self.Evaluate (ur,i)
##            self.Reporting.SectionFinished ("Test of IdleController with DA entry "+str(i),sectionverdict)
##            self.Verdict().SetTestCaseVerdict (sectionverdict)
##            i+=1
##            print "Loop "+str(i)+" finished with verdict "+ sectionverdict.ToString()
##            print ""
##            
##        print "Verdict of test run is: "+self.Verdict().GetVerdict()

    def Teardown(self):            
        if ( self.doneTbInit == True ):
            print "Close MA port"
            self.PortMA().Close()
            


    def Evaluate(self, urParamset,index):
        evaluationVerdict = AtclVerdict.Verdict(AtclVerdict.VerdictCode.None)
        torque = urParamset.Items.GetItem("Torque");
        minRpm = urParamset.Items.GetItem("MinRPMValue");
        maxRpm = urParamset.Items.GetItem("MaxRPMValue");

        print "Set the torque to "+ str(torque.Val)+ " ..."
        self.PortMA().SetModelValue(torque)

        print "Switching off all aggregates ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutBaseArray(
                [
                AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "Off", "2pol-Schalter"),
                AtclTypes.TypeSutSwitch("StartIgnition", "StartIgnition", "",  "Off", "2pol-Schalter"),
                self.acSwitch
                ]
            )
        )
        print "Configuring datalogger ..."
        logFileName = "log_file_"+str(index)
        print "Logger file name: "+logFileName
        self.PortMA().ConfigureDataLogger(AtclTypes.TypeDLConfigureRecord(
            logFileName,
            10.0,
            [AtclTypes.TypeSelectSignalRecord("Engine","AcquisitionTask")],
            AtclTypes.TypeDLTriggerRecord(
            AtclTypes.TypeDLGeneralTriggerUnion(AtclTypes.TypeDLNoneTriggerRecord()),
            AtclTypes.TypeDLGeneralTriggerUnion(AtclTypes.TypeDLNoneTriggerRecord()),
            0.0,
            0.0)))
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
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartIgnition", "StartIgnition", "",  "On", "2pol-Schalter"))
        print "Starting engine ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "On", "2pol-Schalter"))
        print "Waiting 5s to reach idle speed ..."
        time.sleep(5)
        print "Switching off starter ..."
        print "Switching off starter ..."       
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "Off", "2pol-Schalter"))
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
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartAirCondition", "StartAirCondition", "",  "Off", "2pol-Schalter"))
        print "Getting logged signals ..."
        signals = self.PortMA().GetLoggedSignals(logFileName,
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
        self.Reporting.SetInfoText(0, "min. engine speed = " + str(ymin),9)
        self.Reporting.SetInfoText(0, "max. engine speed = " + str(ymax),9)
        if (ymin < minRpm.Val) or (ymax > maxRpm.Val):
            print "engine speed is not within the specified bounds (" + str(minRpm.Val) + "," + str(maxRpm.Val) + ")"
            self.Reporting.SetErrorText(0,"engine speed is not within the specified bounds (" + str(minRpm.Val) + "," + str(maxRpm.Val) + ")",0)
            evaluationVerdict.Fail()
        else:
            print "engine speed is within the specified bounds (" + str(minRpm.Val) + "," + str(maxRpm.Val) + ")"
            self.Reporting.SetResultText(0,"engine speed is within the specified bounds (" + str(minRpm.Val) + "," + str(maxRpm.Val) + ")",0)
            evaluationVerdict.Pass()
        self.acSwitch.Val = "Off"
        self.PortMA().SetModelValue(self.acSwitch)
        print "Switching on ignition ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartIgnition", "StartIgnition", "",  "Off", "2pol-Schalter"))
        print "Starting engine ..."
        self.PortMA().SetModelValue(AtclTypes.TypeSutSwitch("StartEngine", "StartEngine", "",  "Off", "2pol-Schalter"))
        print "Stopping measurement ..."
        self.PortMA().Stop()
        print "VERDICT = " + evaluationVerdict.ToString()

        #finsih the reporting        
        ReportingEngine.TestCaseFinished("PASS", "0", "100");
        ReportingEngine.SaveReport();
        
        return evaluationVerdict

#+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
# main() function  
#
if __name__ == "__main__":

    sarController = AtclInterfaces.Factory.AtclFactory.GetInstance("TC_PY_Template").GetSARHostController()
    sarStarted = False
    if ( sarController.IsSARProcessRunning() == False):
        raw_input('Press any to start the test case execution...')  # inserted only to have the DOS prompt on top
        print "Initializing LABCAR-AUTOMATION ..."
        sarController.StartSARHostProcess()
        sarConfig = AtclInterfaces.Configuration.SarConfiguration()
        sarConfig.TbcFileName = "D:/etasdata/LABCAR-AUTOMATION3.0/Tutorial/TestBenchConfig/LabCar.tbc"
        sarConfig.ReportFileName = "c:/temp"
        sarController.InitializeSAR(sarConfig)
        sarStarted = True

    tc = TC_myTestCase("TC_PY_Template")
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("TCD","Ulrich Wolters, ETAS "))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Version","0.1"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Purpose","Framework Test Case for GM purposes"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Comment","TBInit/NoTbInit Testcase"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("ChangeLog","Created"))
    tc.AddMetaData(AtclInterfaces.MetaData.TCMetaData("Label","TC_PY_Template"))
    try:
        tc.RegisterParameter()
        tc.Init()
        tc.Test()
        tc.Teardown()
        tc.Finished()
        if (sarStarted == True):
            sarController.ShutdownSARHostProcess()
            raw_input('Press any key to terminate after completed test execution')
    except:
        tc.Error()
        tc.Finished()
        if (sarStarted == True):
            sarController.ShutdownSARHostProcess()
            raw_input('Press any key to terminate the tests case after an error occured')
        else:
            exit (-1)

