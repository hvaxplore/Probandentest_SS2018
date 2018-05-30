using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TestDataManager), typeof(SaveLoad))]
public class TestRuntimeManager : MonoBehaviour
{
    public TestState testState;

    public SaveLoad sl;
    private TestDataManager testDataManager;

    public Text textInfo;

    public List<Transform> targetObjects; // TODO: Make serializable class for this
    public static TestRuntimeManager instance; 

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        testState = TestState.none;
        testDataManager = GetComponent<TestDataManager>();
        sl = GetComponent<SaveLoad>();
    }

    // Update is called once per frame
    void Update()
    {
        updateTestInfo(); // Updates the UI, not visible for VR-User

        if(testState != TestState.none) 
        {
            processData(); // If the tests start, do stuff
        }
    }

    public void nextTest()
    {
        SwitchStateEnd();
        testState++;
        testDataManager.testState = testState;
        testDataManager.targetCurrent = targetObjects[(int)testState];
        SwitchStateStart();
    }

    /*
    public void prevTest()
    {
        SwitchStateEnd();
        testState--;
        testDataManager.testState = testState;
        testDataManager.targetCurrent = targetObjects[(int)testState];
        SwitchStateStart();
    }
    */

    public void SwitchStateStart()
    {
        switch(testState)
        {
            case TestState.table:
                TestResultDistanceMeasure table = new TestResultDistanceMeasure();
                table.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testDistance = table;
                break;
            case TestState.spotlightB:
                TestResultSpotlight spotlight = new TestResultSpotlight();
                spotlight.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testSpotlightsBlue = spotlight;
                break;
            case TestState.spotlightG:
                spotlight = new TestResultSpotlight();
                spotlight.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testSpotlightsGreen = spotlight;
                break;
            case TestState.spotlightR:
                spotlight = new TestResultSpotlight();
                spotlight.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testSpotlightsRed = spotlight;
                break;
            case TestState.clockBig:
                TestResultClocks clock = new TestResultClocks();
                clock.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testClocksBig = clock;
                break;
            case TestState.clockNormal:
                clock = new TestResultClocks();
                clock.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testClocksNormal = clock;
                break;
            case TestState.clockSmall:
                clock = new TestResultClocks();
                clock.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testClocksSmall = clock;
                break;
            case TestState.cube:
                TestResultCube cube = new TestResultCube();
                cube.fillStart(testDataManager.testState, testDataManager.timeCurrent, testDataManager.camCyclop);
                sl.proband.testCube = cube;
                break;
        }
    }

    public void SwitchStateEnd()
    {
        switch(testState)
        {
            case TestState.table:
                TestResultDistanceMeasure table = sl.proband.testDistance;
                table.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testDistance = table;
                break;
            case TestState.spotlightB:
                TestResultSpotlight spotlight = new TestResultSpotlight();
                spotlight.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testSpotlightsBlue = spotlight;
                break;
            case TestState.spotlightG:
                spotlight = new TestResultSpotlight();
                spotlight.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testSpotlightsGreen = spotlight;
                break;
            case TestState.spotlightR:
                spotlight = new TestResultSpotlight();
                spotlight.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testSpotlightsRed = spotlight;
                break;
            case TestState.clockBig:
                TestResultClocks clock = new TestResultClocks();
                clock.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testClocksBig = clock;
                break;
            case TestState.clockNormal:
                clock = new TestResultClocks();
                clock.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testClocksNormal = clock;
                break;
            case TestState.clockSmall:
                clock = new TestResultClocks();
                clock.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testClocksSmall = clock;
                break;
            case TestState.cube:
                TestResultCube cube = new TestResultCube();
                cube.fillEnd(testDataManager.timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.proband.testCube = cube;
                break;
        }
    }

    void processData()
    {
        sl.proband.AddStep(testDataManager);
    }

    void saveDataFiles()
    {
        sl.SaveProband();
    }

    void updateTestInfo()
    {
        textInfo.text = testState.ToString() + ": time: " + testDataManager.timeCurrent +  " seconds";
    }
}

public enum TestState
{
	none,

	gist,
	gistIdle,

	table,
	tableIdle,

	spotlightR,
	spotlightRIdle,

	spotlightG,
	spotlightGIdle,

	spotlightB,
	spotlightBIdle,

	clockSmall,
	clockSmallIdle,

	clockNormal,
	clockNormalIdle,

	clockBig,
	clockBigIdle,

	cube,
	cubeIdle,

    testEnded,
}