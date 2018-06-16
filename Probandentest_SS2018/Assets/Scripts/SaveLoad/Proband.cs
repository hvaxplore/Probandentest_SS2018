using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//[System.Serializable]
//public class Proband
//{
//    public string Name;
//    public int Id;

//    /*
//		public string Gender;
//		public string Contact;

//		public string vrAffinity; // How many times VR was used by the proband

//		public string opticalHelp; // The Sehhilfe
//		public string opticalErrors; // The Sehfehlers
//	*/

//    public float IPD;
//    public float EyeHeight;

//    public TestResultDistanceMeasure tableGuess;
//    public TestResultSpotlight testSpotlightsRed;
//    public TestResultSpotlight testSpotlightsGreen;
//    public TestResultSpotlight testSpotlightsBlue;
//    public TestResultClocks testClocksSmall;
//    public TestResultClocks testClocksNormal;
//    public TestResultClocks testClocksBig;
//    public TestResultCube testCube;

//    [HideInInspector]
//    public List<TestStep> steps;

//    public Proband()
//    {
//        steps = new List<TestStep>();
//    }

//    public Proband(Proband _proband, bool withSteps = true)
//    {
//        Name = _proband.Name;
//        Id = _proband.Id;

//        IPD = _proband.IPD;
//        EyeHeight = _proband.EyeHeight;

//        tableGuess = _proband.tableGuess;
//        testSpotlightsRed = _proband.testSpotlightsRed;
//        testSpotlightsGreen = _proband.testSpotlightsGreen;
//        testSpotlightsBlue = _proband.testSpotlightsBlue;
//        testClocksSmall = _proband.testClocksSmall;
//        testClocksNormal = _proband.testClocksNormal;
//        testClocksBig = _proband.testClocksNormal;
//        testCube = _proband.testCube;
//        steps = new List<TestStep>();
//    }

//    public Proband(string _name, int _id, string _gender, string _contact, string _vr, string _opticalHelp, string _opticalError, float _ipd, float _eyeHeight)
//    {
//        Name = _name;
//        Id = _id;

//        /*
//        Gender = _gender;
//		Contact = _contact;
//        vrAffinity = _vr; // How many times VR was used by the proband
//        opticalHelp = _opticalHelp; // The Sehhilfe
//        opticalErrors = _opticalError; // The Sehfehlers
//		*/

//        IPD = _ipd;
//        EyeHeight = _eyeHeight;

//        steps = new List<TestStep>();
//    }

//    public Proband(Proband _prob)
//    {
//        Name = _prob.Name;
//        Id = _prob.Id;

//        /*
//        Gender = _prob.Gender;
//        vrAffinity = _prob.vrAffinity; // How many times VR was used by the proband
//        opticalHelp = _prob.opticalHelp; // The Sehhilfe
//        opticalErrors = _prob.opticalErrors; // The Sehfehlers
//		*/

//        IPD = _prob.IPD;

//        steps = new List<TestStep>();
//    }

//}

[System.Serializable]
public class TestStep
{
    public TestState taskIndexCurrent;
    public float timeCurrent; // Time

    // Saved seperately, if it is necessary to look into all data
    public Vector3 headDataPos; // TODO: Transform instead ObjectPosRot
    public Quaternion headDataRot; // TODO: Transform instead ObjectPosRot
    public Vector3 eyeGaze;

    public TestStep(TestState taskIndexCurrent, float timeCurrent, Vector3 headDataPos, Quaternion headDataRot, Vector3 _eyeGabe)
    {
        this.taskIndexCurrent = taskIndexCurrent;
        this.timeCurrent = timeCurrent;
        this.headDataPos = headDataPos;
        this.headDataRot = headDataRot;
        this.eyeGaze = _eyeGabe;
    }
}
[System.Serializable]
public class TestResults
{
    public string testState;
    public float timeStart;

    public float timeEnd;
    public float testDuration;

    public bool taskFulfilled;

    public Vector3 positionOnTaskStart;
    public Vector3 positionOnTaskFinish;
    public float distanceToTarget;

    public virtual void fillStart(TestState _state, float _time, Vector3 _head)
    {
        testState = _state.ToString(); // TODO: test this
        timeStart = _time;
        taskFulfilled = true;
        positionOnTaskStart = _head;
    }

    public virtual void fillEnd(float _time, Vector3 _head, Vector3 _target)
    {
        timeEnd = _time;
        testDuration = timeEnd - timeStart;
        positionOnTaskFinish = _head;
        distanceToTarget = Vector3.Distance(_head, _target);
    }
}

[System.Serializable]
public class TestResultDistanceMeasure : TestResults
{
    public float distanceGuess;
    public float deviationInMeter;
    public float deviationInPercent;
}

[System.Serializable]
public class TestResultSpotlight : TestResults
{
    public Vector3 spotLightPosition;

    public override void fillEnd(float _time, Vector3 _head, Vector3 _target)
    {
        timeEnd = _time;
        testDuration = timeEnd - timeStart;
        positionOnTaskFinish = _head;

        Vector3 head = _head;
        head.y = 0;

        spotLightPosition = _target;

        if (_target != null)
        {
            Vector3 target = _target;
            target.y = 0;
            distanceToTarget = Vector3.Distance(head, target);
        }
    }
}
[System.Serializable]
public class TestResultClocks : TestResults
{
    public string clockReal;
    public string clockGuessed;
    public bool correctGuess;
    public float deviation;
}
[System.Serializable]
public class TestResultCube : TestResults
{
    public string cubeGiven;
    public string cubeChosen;
    public float deviationPercentage;
    public float deviationCM;
}

[System.Serializable]
public class ProbandTests
{
    public TestResultDistanceMeasure tableGuess;
    public TestResultSpotlight testSpotlightsRed;
    public TestResultSpotlight testSpotlightsGreen;
    public TestResultSpotlight testSpotlightsBlue;
    public TestResultClocks testClocksSmall;
    public TestResultClocks testClocksNormal;
    public TestResultClocks testClocksBig;
    public TestResultCube testCube;

    public ProbandTests()
    {
    }
}


[System.Serializable]
public class ProbandMeta
{
    //public string Name;
    public int Id;
    //public float IPD;
    //public float EyeHeight;
    //public string gender;
    //public string vraffinity;
    //public string sehhilfe;
    //public string sehfehler;
    public bool isVR_Proband;

    public ProbandMeta()
    {

    }

    public ProbandMeta(int id, bool isVR_Proband)
    {
        Id = id;
        this.isVR_Proband = isVR_Proband;
    }

    //public ProbandMeta(string name, int id, float iPD, float eyeHeight)
    //{
    //    //Name = name;
    //    Id = id;
    //    //IPD = iPD;
    //    //EyeHeight = eyeHeight;
    //}
}


[System.Serializable]
public class ProbandSteps
{
    [HideInInspector]
    public List<TestStep> steps;

    public ProbandSteps()
    {
        steps = new List<TestStep>();
    }

    public ProbandSteps(List<TestStep> steps)
    {
        this.steps = steps;
    }

    public void AddStep(TestDataManager _manager)
    {
        TestStep newStep = new TestStep(
            _manager.TestState,
            _manager.runtimeManager.timeCurrent,
            _manager.headPos,
            _manager.headRot,
            _manager.eyegaze
            );
        //TestStep newStep = new TestStep(_manager.TestState, _manager.runtimeManager.timeCurrent, _manager.camCyclop, _manager.camLeft, _manager.camRight);
        steps.Add(newStep);
    }
}