using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRuntimeManager : MonoBehaviour
{
    public SaveLoader sl;

    private TestDataManager testDataManager;

    public TestState testState;

	/*
		public List<float> originalDistanceData;
		public List<float> remainingDistanceData;
		public List<int> indexes;
	*/

	public int taskIndex;
	public bool taskFinished; // If the current task is finished
    public int order; // only applicable if the current task has a order, else 0


    public float initTime;
    public float timePerTest;
    public float elapsedTime;
    private float elapsedTicks;
    public float confidence;
    public float confidenceThreshold = 0.7f;

    public GameObject canvas;

    // Use this for initialization
    void Start()
    {
        testState = TestState.stopped;
        testDataManager = GetComponent<TestDataManager>();
        resetTest();
        for(int i = 0; i < remainingDistanceData.Count; i++)
        {
            indexes.Add(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N) && testState == TestState.stopped)
        {
            CancelInvoke("startTest");
            randomTest();
            Invoke("startTest", initTime);
        }
        else if (Input.GetKeyDown(KeyCode.S) && testState != TestState.testEnded)
        {
            randomTest();
            startTest();
        }
        else if (Input.GetKeyDown(KeyCode.E) && testState != TestState.testEnded)
        {
            stopTest();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            resetTest();
        }

        if (testState == TestState.waiting)
        {
            // randomTest();
            nextTest();
            startTest();
        }

        if (testState == TestState.running)
        {
            if (elapsedTime < timePerTest)
            {
                elapsedTime += Time.deltaTime;
                elapsedTicks++;
                processData();
            }
            else
            {
                elapsedTime = 0;
                elapsedTicks = 0;
                testState = TestState.waiting;
                finalizeSingleData();
            }
        }
    }

    public void startautonomTest()
    {
        randomTest();
        startTest();
    }

    void resetTest()
    {
        testState = TestState.stopped;
        remainingDistanceData = new List<float>(originalDistanceData);

        taskIndex = 0;
		taskFinished = false;
		order = 0;

        elapsedTime = 0;
        elapsedTicks = 0;
    }

    void nextTest()
    {
        if (indexes.Count > 0)
        {
            activeTestID = indexes[indexes.Count-1];
            activeDistance = originalDistanceData[activeTestID];
            elapsedTime = 0;
            elapsedTicks = 0;
            testDataManager.targetObj.dist_Z = activeDistance;
        }
    }

    void randomTest()
    {
        if (indexes.Count > 0)
        {
            activeTestID = indexes[Random.Range(0, indexes.Count)];
            activeDistance = originalDistanceData[activeTestID];
            elapsedTime = 0;
            elapsedTicks = 0;
            testDataManager.targetObj.dist_Z = activeDistance;
        }
    }

    void finalizeSingleData()
    {
        //remainingDistanceData.RemoveAt(activeTestID);
        indexes.Remove(activeTestID);
        orderID++;
        if (indexes.Count <= 0)
        {
            FinalizeWholeData();
        }
    }

    void FinalizeWholeData()
    {
        Debug.Log("Test Ended. pls restart");
        testState = TestState.testEnded;
        saveDataFiles();
    }

    void processData()
    {
        //Debug.Log(sl);
        confidence = PupilTools.ConfidenceForDictionary(PupilTools.gazeDictionary);
        if (confidence > confidenceThreshold)
        {
            sl.Proband.AddStep(activeTestID, orderID, activeDistance, testDataManager.targetObj.absParallax, elapsedTime, elapsedTicks, confidence, testDataManager.convergenceAngle, testDataManager.convergenceAngleL, true, testDataManager.convergenceAngleR, true);
        }
    }

    void saveDataFiles()
    {
        sl.SaveProband();
    }

    void stopTest()
    {
        CancelInvoke("startTest");
        elapsedTime = 0;
        elapsedTicks = 0;
        testState = TestState.stopped;
    }

    void startTest()
    {
        elapsedTime = 0;
        elapsedTicks = 0;
        testState = TestState.running;
    }
}

public enum TestState
{
    running,
    waiting,
    finalize,
    stopped,
    testEnded
}