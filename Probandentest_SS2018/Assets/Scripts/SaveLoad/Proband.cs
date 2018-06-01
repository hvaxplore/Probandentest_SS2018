using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public Quaternion eyeLeft;
    public Quaternion eyeRight;

    public TestStep(TestState _taskIndex, float _time, Transform _head, Transform _eyeLeft, Transform _eyeRight)
    {
        taskIndexCurrent = _taskIndex; // TODO: Time.time instead 

        timeCurrent = _time;

        //headData = new ObjectPosRot(_head);
        //eyeLeft = new ObjectRot(_eyeLeft);
        //eyeRight = new ObjectRot(_eyeRight);
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

    public virtual void fillStart(TestState _state, float _time, Transform _head)
    {
        testState = _state.ToString(); // TODO: test this
        timeStart = _time;
        taskFulfilled = true;
        positionOnTaskStart = _head.position;
    }

    public virtual void fillEnd(float _time, Transform _head, Transform _target)
    {
        timeEnd = _time;
        testDuration = timeEnd - timeStart;
        positionOnTaskFinish = _head.position;
        if (_target != null)
            distanceToTarget = Vector3.Distance(_head.position, _target.position);
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

    public override void fillEnd(float _time, Transform _head, Transform _target)
    {
        timeEnd = _time;
        testDuration = timeEnd - timeStart;
        positionOnTaskFinish = _head.position;

        Vector3 head = _head.position;
        head.y = 0;

        spotLightPosition = _target.position;

        if (_target != null)
        {
            Vector3 target = _target.position;
            target.y = 0;
            distanceToTarget = Vector3.Distance(head, target);
        }
    }
}
[System.Serializable]
public class TestResultClocks : TestResults
{
    public string timeReal;
    public string timeGuess;
    public bool correctGuess;
}
[System.Serializable]
public class TestResultCube : TestResults
{
    public string cubeGiven;
    public string cubeChosen;
}




[System.Serializable]
public class ProbandTests
{
    public string Name;
    public int Id;

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
public class ProbandSteps
{
    public string Name;
    public int Id;
    public float IPD;
    public float EyeHeight;

    public List<TestStep> steps;

    public ProbandSteps()
    {
        steps = new List<TestStep>();
    }

    public ProbandSteps(string name, int id, float iPD, float eyeHeight, List<TestStep> steps)
    {
        Name = name;
        Id = id;
        IPD = iPD;
        EyeHeight = eyeHeight;
        this.steps = steps;
    }

    public void AddStep(TestDataManager _manager)
    {
        TestStep newStep = new TestStep(_manager.TestState, _manager.runtimeManager.timeCurrent, _manager.camCyclop, _manager.camLeft, _manager.camRight);
        steps.Add(newStep);
    }

    public void AddStep(TestState _taskIndex, float _time, float _conf, float _conv, Transform _head, Transform _eyeLeft, Transform _eyeRight, Transform _handLeft, Transform _handRight)
    {
        TestStep newStep = new TestStep(_taskIndex, _time, _head, _eyeLeft, _eyeRight);
        steps.Add(newStep);
    }
}