using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TestDataManager), typeof(SaveLoad))]
public class TestRuntimeManager : MonoBehaviour
{
    public SaveLoad sl;
    private TestDataManager testDataManager;

    public Text textInfo;
    public MeshRenderer fadeMechRenderer;
    public InputField tableGuessInput;
    public Toggle failedToggle;
    public Dropdown cubeChosen;
    public Dropdown cubeGiven;
    public InputField clockGuess;

    public float timeCurrent;

    public List<Transform> targetObjects; // TODO: Make serializable class for this
    public static TestRuntimeManager instance;
    private TestResults activeTest;
    private CubeOptions cubeOptions;

    public CubeOptions CubeOptions
    {
        get
        {
            return cubeOptions;
        }

        set
        {
            cubeOptions = value;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start()
    {
        testDataManager = GetComponent<TestDataManager>();
        sl = GetComponent<SaveLoad>();

        cubeChosen.ClearOptions();
        cubeGiven.ClearOptions();
        cubeChosen.AddOptions(new List<string>(CubeOptions.GetNames(typeof(CubeOptions))));
        cubeGiven.AddOptions(cubeChosen.options);
    }

    // Update is called once per frame
    void Update()
    {
        updateTestInfo(); // Updates the UI, not visible for VR-User

        if ((int)testDataManager.TestState >= 2 && testDataManager.TestState != TestState.testEnded)
        {
            timeCurrent += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextTest();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            prevTest();
        }
        if (Input.GetKeyDown(KeyCode.Escape) && activeTest != null)
        {
            activeTest.taskFulfilled = !activeTest.taskFulfilled;
            failedToggle.isOn = !activeTest.taskFulfilled;
        }

    }

    public void nextTest()
    {
        SwitchStateEnd();
        testDataManager.TestState++;
        setTargetObjectForTest((int)testDataManager.TestState);
        SwitchStateStart();
    }

    public void prevTest()
    {
        SwitchStateEnd();
        testDataManager.TestState--;
        setTargetObjectForTest((int)testDataManager.TestState);
        SwitchStateStart();
    }

    private void setTargetObjectForTest(int i)
    {
        testDataManager.targetCurrent = targetObjects[(int)testDataManager.TestState];
    }

    private IEnumerator fadeItOut()
    {
        Material fadeMat = fadeMechRenderer.material;
        float alpha = fadeMat.color.a;

        while (alpha > 0)
        {

            alpha -= Time.deltaTime;
            fadeMat.color = new Color(fadeMat.color.r, fadeMat.color.g, fadeMat.color.b, alpha);
            fadeMechRenderer.material = fadeMat;
            yield return alpha;
        }
    }

    public void SwitchStateStart()
    {
        switch (testDataManager.TestState)
        {
            case TestState.fadeState:
                StartCoroutine(fadeItOut());
                break;
            case TestState.gist:
                CancelInvoke("processData");
                InvokeRepeating("processData", 0f, 0.025f);
                break;
            case TestState.tableGuess:
                activeTest = new TestResultDistanceMeasure();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                tableGuessInput.gameObject.SetActive(true);
                sl.probandTests.tableGuess = (TestResultDistanceMeasure)activeTest;
                break;
            case TestState.spotlightB:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testSpotlightsBlue = (TestResultSpotlight)activeTest;
                break;
            case TestState.spotlightG:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testSpotlightsGreen = (TestResultSpotlight)activeTest;
                break;
            case TestState.spotlightR:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testSpotlightsRed = (TestResultSpotlight)activeTest;
                break;
            case TestState.clockBig:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = ":";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testClocksBig = (TestResultClocks)activeTest;
                break;
            case TestState.clockNormal:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = ":";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testClocksNormal = (TestResultClocks)activeTest;
                break;
            case TestState.clockSmall:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = ":";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testClocksSmall = (TestResultClocks)activeTest;
                break;
            case TestState.cube:
                cubeChosen.gameObject.SetActive(true);
                cubeGiven.gameObject.SetActive(true);
                activeTest = new TestResultCube();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, testDataManager.camCyclop);
                sl.probandTests.testCube = (TestResultCube)activeTest;
                break;
            case TestState.testEnded:
                CancelInvoke("processData");
                saveDataFiles();
                break;
        }
    }

    public void SwitchStateEnd()
    {
        switch (testDataManager.TestState)
        {
            case TestState.tableGuess:
                TestResultDistanceMeasure temp = (TestResultDistanceMeasure)activeTest;
                temp.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                temp.distanceGuess = float.Parse(tableGuessInput.text);
                tableGuessInput.gameObject.SetActive(false);
                temp.deviationInMeter = temp.distanceGuess - temp.distanceToTarget;
                temp.deviationInPercent = temp.deviationInMeter / temp.distanceToTarget * 100;
                sl.probandTests.tableGuess = temp;
                break;
            case TestState.spotlightB:
                TestResultSpotlight spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsBlue = spotlightTemp;
                break;
            case TestState.spotlightG:
                spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsGreen = spotlightTemp;
                break;
            case TestState.spotlightR:
                spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsRed = spotlightTemp;
                break;
            case TestState.clockBig:
                clockGuess.gameObject.SetActive(false);
                TestResultClocks clock = (TestResultClocks)activeTest;
                clock.timeReal = "6:66";
                clock.timeGuess = clockGuess.text;

                if (clock.timeGuess.Equals(clock.timeReal))
                    clock.correctGuess = true;

                clock.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testClocksBig = clock;
                break;
            case TestState.clockNormal:
                clockGuess.gameObject.SetActive(false);
                clock = (TestResultClocks)activeTest;
                clock.timeReal = "6:66";
                clock.timeGuess = clockGuess.text;

                if (clock.timeGuess.Equals(clock.timeReal))
                    clock.correctGuess = true;

                clock.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testClocksNormal = clock;
                break;
            case TestState.clockSmall:
                clockGuess.gameObject.SetActive(false);
                clock = (TestResultClocks)activeTest;
                clock.timeReal = "6:66";
                clock.timeGuess = clockGuess.text;

                if (clock.timeGuess.Equals(clock.timeReal))
                    clock.correctGuess = true;

                clock.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testClocksSmall = clock;
                break;
            case TestState.cube:
                cubeChosen.gameObject.SetActive(false);
                cubeGiven.gameObject.SetActive(false);
                TestResultCube cube = (TestResultCube)activeTest;
                cube.cubeChosen = cubeChosen.options[cubeChosen.value].text;
                cube.cubeGiven = cubeGiven.options[cubeGiven.value].text;
                cube.fillEnd(timeCurrent, testDataManager.camCyclop, testDataManager.targetCurrent);
                sl.probandTests.testCube = cube;
                break;
        }
    }

    void processData()
    {
        if (testDataManager.TestState != TestState.none)
        {
            sl.probandSteps.AddStep(testDataManager);
        }
    }

    void saveDataFiles()
    {
        sl.SaveProbandSteps();
        sl.SaveProbandTasks();
    }

    void updateTestInfo()
    {
        textInfo.text = testDataManager.TestState.ToString() + " || time: " + (int)timeCurrent + " seconds || " + sl.probandSteps.steps.Count + " Steps";
    }
}

public enum TestState
{
    none,

    fadeState,

    gist,
    gistIdle,

    tableGuess,
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

public enum CubeOptions
{
    CUBE_5_170,
    CUBE_4_155,
    CUBE_3_140,
    CUBE_2_125,
    CUBE_1_110
}