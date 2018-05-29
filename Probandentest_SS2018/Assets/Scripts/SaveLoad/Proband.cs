using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Proband
{
    public string Name;
    public int Id;

	/*
		public string Gender;
		public string Contact;

		public string vrAffinity; // How many times VR was used by the proband

		public string opticalHelp; // The Sehhilfe
		public string opticalErrors; // The Sehfehlers
	*/

    public float IPD;
    public float EyeHeight;

	public TestResultDistanceMeasure testDistance;
	public TestResultSpotlight testSpotlightsRed;
	public TestResultSpotlight testSpotlightsGreen;
	public TestResultSpotlight testSpotlightsBlue;
	public TestResultClocks testClocksSmall;
	public TestResultClocks testClocksNormal;
	public TestResultClocks testClocksBig;
	public TestResultCube testCube;

    public List<TestStep> steps;

    public Proband(string _name, int _id, string _gender, string _contact, string _vr, string _opticalHelp, string _opticalError, float _ipd, float _eyeHeight)
    {
        Name = _name;
        Id = _id;

		/*
        Gender = _gender;
		Contact = _contact;
        vrAffinity = _vr; // How many times VR was used by the proband
        opticalHelp = _opticalHelp; // The Sehhilfe
        opticalErrors = _opticalError; // The Sehfehlers
		*/

        IPD = _ipd;
		EyeHeight = _eyeHeight;

        steps = new List<TestStep>();
    }

    public Proband(Proband _prob)
    {
        Name = _prob.Name;
        Id = _prob.Id;

		/*
        Gender = _prob.Gender;
        vrAffinity = _prob.vrAffinity; // How many times VR was used by the proband
        opticalHelp = _prob.opticalHelp; // The Sehhilfe
        opticalErrors = _prob.opticalErrors; // The Sehfehlers
		*/

        IPD = _prob.IPD;

        steps = new List<TestStep>();
    }

    public void AddStep(TestDataManager _manager)
    {
        TestStep newStep = new TestStep(_manager.testState, _manager.timeCurrent, _manager.camCyclop, _manager.camLeft, _manager.camRight);
        steps.Add(newStep);
    }

    public void AddStep(TestState _taskIndex, float _time, float _conf, float _conv, Transform _head, Transform _eyeLeft, Transform _eyeRight, Transform _handLeft, Transform _handRight)
    {
        TestStep newStep = new TestStep(_taskIndex, _time, _head, _eyeLeft, _eyeRight);
        steps.Add(newStep);
    }
}

[System.Serializable]
public class TestStep
{
	public TestState taskIndexCurrent;
    public float timeCurrent; // Time

    // Saved seperately, if it is necessary to look into all data
	public ObjectPosRot headData; // TODO: Transform instead ObjectPosRot
    public ObjectRot eyeLeft;
    public ObjectRot eyeRight;

    public TestStep(TestState _taskIndex, float _time, Transform _head, Transform _eyeLeft, Transform _eyeRight)
    {
        taskIndexCurrent = _taskIndex; // TODO: Time.time instead 

        timeCurrent = _time;

        headData = new ObjectPosRot(_head);
        eyeLeft = new ObjectRot(_eyeLeft);
        eyeRight = new ObjectRot(_eyeRight);
   }
}

public class TestResults
{
	public string testState;
	public float timeStart;

	public float timeEnd;
	public float testDuration;

	public bool taskFulfilled;

	public Transform positionOnTaskStart;
	public Transform positionOnTaskFinish;
	public float distanceToTarget;

	public virtual void fillStart(TestState _state, float _time, Transform _head)
	{
		testState = _state.ToString(); // TODO: test this
		timeStart = _time;
		positionOnTaskStart = _head;
	}

	public virtual void fillEnd(float _time, Transform _head, Transform _target)
	{
		timeEnd = _time;
		testDuration = timeEnd - timeStart;
		positionOnTaskFinish = _head;
		distanceToTarget = Vector3.Distance(_head.position, _target.position);
	}
}

public class TestResultDistanceMeasure : TestResults
{
	public float distanceGuess;
}

public class TestResultSpotlight : TestResults
{
	public Vector3 spotLightPosition;

	public override void fillEnd(float _time, Transform _head, Transform _target)
	{
		timeEnd = _time;
		testDuration = timeEnd - timeStart;
		positionOnTaskFinish = _head;

		Vector3 head = _head.position;
		head.y = 0;
		Vector3 target = _target.position;
		target.y = 0;
		distanceToTarget = Vector3.Distance(head, target);
	}
}

public class TestResultClocks : TestResults
{
	public string timeReal;
	public string timeGuess;
}

public class TestResultCube : TestResults
{
	public int cubeGiven;
	public int cubeChosen;
}

public class ObjectPosRot
{
	public Vector3 objPos;
	public Quaternion objRot;

	/// <summary>
	/// Saves an objects global pos/rot
	/// </summary>
	public ObjectPosRot(Transform _target)
	{
		objPos = _target.position;
		objRot = _target.rotation;
	}

	public ObjectPosRot(Vector3 _handPos, Quaternion _handRot)
	{
		objPos = _handPos;
		objRot = _handRot;
	}
}
public class ObjectRot
{
	public Quaternion objRot;

	/// <summary>
	/// Saves an objects global rot
	/// </summary>
	public ObjectRot(Transform _target)
	{
		objRot = _target.rotation;
	}

    public ObjectRot(Quaternion _eyeRot)
    {
		objRot = _eyeRot;
    }
}
/*
public class SerializableRotation
{
	public float xRot;
	public float yRot;
	public float zRot;
	public float wRot;

	public SerializableRotation(Quaternion rot)
	{
		xRot = rot.x;
		yRot = rot.y;
		zRot = rot.z;
		wRot = rot.w;
	}
}
public class SerializablePosition
{
	public float xPos;
	public float yPos;
	public float zPos;

	public SerializablePosition(Vector3 pos)
	{
		xPos = pos.x;
		yPos = pos.y;
		zPos = pos.z;
	}
}
*/