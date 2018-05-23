using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Proband
{
    public string Name;
    public int Id;
    public string Gender;

    public string vrAffinity; // How many times VR was used by the proband

    public string opticalHelp; // The Sehhilfe
    public string opticalErrors; // The Sehfehlers

    public float IPD;
    public float EyeHeight;

    public List<TestStep> steps;

	public TestResultDistanceMeasure testDistance; // only a single test
	public List<TestResultSpotlight> testSpotlights; // multiple tests
	public List<TestResultClocks> testClocks; // multiple tests
	public TestResultCube testCube; // only a single test

    public Proband(string _name, int _id, string _gender, string _vr, string _opticalHelp, string _opticalError, float _ipd, float _eyeHeight)
    {
        Name = _name;
        Id = _id;
        Gender = _gender;
        vrAffinity = _vr; // How many times VR was used by the proband
        opticalHelp = _opticalHelp; // The Sehhilfe
        opticalErrors = _opticalError; // The Sehfehlers
        IPD = _ipd;
		EyeHeight = _eyeHeight;

        steps = new List<TestStep>();
    }

    public Proband(Proband _prob)
    {
        Name = _prob.Name;
        Id = _prob.Id;
        Gender = _prob.Gender;
        vrAffinity = _prob.vrAffinity; // How many times VR was used by the proband
        opticalHelp = _prob.opticalHelp; // The Sehhilfe
        opticalErrors = _prob.opticalErrors; // The Sehfehlers
        IPD = _prob.IPD;

        steps = new List<TestStep>();
    }

    public void AddStep(int _taskIndex, bool _taskFinished, int _order, float _time, float _tick, float _conf, float _conv, Transform _head, Transform _eyeLeft, Transform _eyeRight, Transform _handLeft, Transform _handRight)
    {
        TestStep newStep = new TestStep(_taskIndex, _taskFinished, _order, _time, _tick, _conf , _conv, _head, _eyeLeft, _eyeRight, _handLeft, _handRight);
        steps.Add(newStep);
    }

}

[System.Serializable]
public class TestStep
{
	public int taskIndex;
	public bool taskFinished; // If the current task is finished
    public int order; // only applicable if the current task has a order, else 0
    public float timeCurrent; // Time
    public float tickCurrent; // Frame

    public float confidence;

    public float convergence;

    // Saved seperately, if it is necessary to look into all data
	public ObjectPosRot headData;
    public ObjectRot eyeLeft;
    public ObjectRot eyeRight;
    public ObjectPosRot handLeft;
    public ObjectPosRot handRight;

    public TestStep(int _taskIndex, bool _taskFinished, int _order, float _time, float _tick, float _conf, float _conv, Transform _head, Transform _eyeLeft, Transform _eyeRight, Transform _handLeft, Transform _handRight)
    {
        taskIndex = _taskIndex;
		taskFinished = _taskFinished;
        order = _order;

        timeCurrent = _time;
        tickCurrent = _tick;

        confidence = _conf;
        convergence = _conv;

        headData = new ObjectPosRot(_head);
        eyeLeft = new ObjectRot(_eyeLeft);
        eyeRight = new ObjectRot(_eyeRight);
        handLeft = new ObjectPosRot(_handLeft);
        handRight = new ObjectPosRot(_handRight);
    }
}

[System.Serializable]
public class TestResults
{
	public int taskIndex;
	public int taskOrder; 
	public bool taskFulfilled;
}

[System.Serializable]
public class TestResultDistanceMeasure : TestResults
{
	public float timeStart;
	public float distance;	
}
[System.Serializable]
public class TestResultSpotlight : TestResults
{
	public float timeReached;
	public SerializablePosition position;
	public float fulfilled;
}
[System.Serializable]
public class TestResultClocks : TestResults
{
	public float timeStart;
	public float timeEnd;
	public SerializablePosition position;
	public bool correct;
}
[System.Serializable]
public class TestResultCube : TestResults
{
	public float timeLookStart;
	public float timeLookEnd;
	public float timeChoseStart;
	public float timeChoseEnd;
	public int cubeGiven;
	public int cubeChosen;
}

[System.Serializable]
public class ObjectPosRot
{
	public SerializablePosition objPos;
	public SerializableRotation objRot;

	/// <summary>
	/// Saves an objects global pos/rot
	/// </summary>
	public ObjectPosRot(Transform _target)
	{
		objPos = new SerializablePosition(_target.position);
		objRot = new SerializableRotation(_target.rotation);
	}

	public ObjectPosRot(Vector3 _handPos, Quaternion _handRot)
	{
		objPos = new SerializablePosition(_handPos);
		objRot = new SerializableRotation(_handRot);
	}
}

[System.Serializable]
public class ObjectRot
{
	public SerializableRotation objRot;

	/// <summary>
	/// Saves an objects global rot
	/// </summary>
	public ObjectRot(Transform _target)
	{
		objRot = new SerializableRotation(_target.rotation);
	}

    public ObjectRot(Quaternion _eyeRot)
    {
		objRot = new SerializableRotation(_eyeRot);
    }
}

[System.Serializable]
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

[System.Serializable]
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