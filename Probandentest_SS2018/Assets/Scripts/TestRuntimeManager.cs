using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

using System;
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

    public GameObject cubeVR;

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
        //DontDestroyOnLoad(gameObject);
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

        if (Input.GetKey(KeyCode.Escape))
        {
            //SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            nextTest();
        }
        else if (Input.GetKeyDown(KeyCode.S)) // TTODO change key
        {
            prevTest();
        }
        if (Input.GetKeyDown(KeyCode.F) && activeTest != null)
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
        testDataManager.targetCurrent = targetObjects[(int)testDataManager.TestState].position;
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

    private IEnumerator fadeItIn()
    {
        Material fadeMat = fadeMechRenderer.material;
        float alpha = fadeMat.color.a;

        while (alpha < 1)
        {
            alpha += Time.deltaTime * 1.1f;
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
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                tableGuessInput.gameObject.SetActive(true);
                sl.probandTests.tableGuess = (TestResultDistanceMeasure)activeTest;
                break;
            case TestState.spotlightB:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testSpotlightsBlue = (TestResultSpotlight)activeTest;
                break;
            case TestState.spotlightG:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testSpotlightsGreen = (TestResultSpotlight)activeTest;
                break;
            case TestState.spotlightR:
                activeTest = new TestResultSpotlight();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testSpotlightsRed = (TestResultSpotlight)activeTest;
                break;
            case TestState.clockBig:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = "04:20";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testClocksBig = (TestResultClocks)activeTest;
                break;
            case TestState.clockNormal:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = "02:45";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testClocksNormal = (TestResultClocks)activeTest;
                break;
            case TestState.clockSmall:
                clockGuess.gameObject.SetActive(true);
                clockGuess.text = "10:30";
                activeTest = new TestResultClocks();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testClocksSmall = (TestResultClocks)activeTest;
                break;
            case TestState.clockSmallIdle:
                StartCoroutine(fadeItIn());
                if(cubeVR != null)
                    cubeVR.SetActive(true);
                break;
            case TestState.cube:
                StartCoroutine(fadeItOut());
                cubeChosen.gameObject.SetActive(true);
                cubeGiven.gameObject.SetActive(true);
                activeTest = new TestResultCube();
                activeTest.fillStart(testDataManager.TestState, timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye));
                sl.probandTests.testCube = (TestResultCube)activeTest;
                break;
            case TestState.saveData:
                CancelInvoke("processData");
                saveDataFiles();
                break;
            case TestState.testEnded:
                break;
        }
    }

    public void SwitchStateEnd()
    {
        switch (testDataManager.TestState)
        {
            case TestState.tableGuess:
                TestResultDistanceMeasure temp = (TestResultDistanceMeasure)activeTest;
                temp.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                temp.distanceGuess = float.Parse(tableGuessInput.text);
                tableGuessInput.gameObject.SetActive(false);
                temp.deviationInMeter = temp.distanceGuess - temp.distanceToTarget;
                temp.deviationInPercent = temp.deviationInMeter / temp.distanceToTarget * 100;
                sl.probandTests.tableGuess = temp;
                break;
            case TestState.spotlightB:
                TestResultSpotlight spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsBlue = spotlightTemp;
                break;
            case TestState.spotlightG:
                spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsGreen = spotlightTemp;
                break;
            case TestState.spotlightR:
                spotlightTemp = (TestResultSpotlight)activeTest;
                spotlightTemp.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testSpotlightsRed = spotlightTemp;
                break;
            case TestState.clockBig:
                clockGuess.gameObject.SetActive(false);
                TestResultClocks clock = (TestResultClocks)activeTest;
                clock.clockReal = "04:20";
                clock.clockGuessed = clockGuess.text;

                if (clock.clockGuessed.Equals(clock.clockReal))
                    clock.correctGuess = true;

                clock.deviation = GetTimeDeviation(clockToFloat(clock.clockReal), clockToFloat(clock.clockGuessed));

                clock.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testClocksBig = clock;
                break;
            case TestState.clockNormal:
                clockGuess.gameObject.SetActive(false);
                clock = (TestResultClocks)activeTest;
                clock.clockReal = "02:45";
                clock.clockGuessed = clockGuess.text;

                if (clock.clockGuessed.Equals(clock.clockReal))
                    clock.correctGuess = true;

                clock.deviation = GetTimeDeviation(clockToFloat(clock.clockReal), clockToFloat(clock.clockGuessed));
                clock.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testClocksNormal = clock;
                break;
            case TestState.clockSmall:
                clockGuess.gameObject.SetActive(false);
                clock = (TestResultClocks)activeTest;
                clock.clockReal = "10:30";
                clock.clockGuessed = clockGuess.text;

                if (clock.clockGuessed.Equals(clock.clockReal))
                    clock.correctGuess = true;

                clock.deviation = GetTimeDeviation(clockToFloat(clock.clockReal), clockToFloat(clock.clockGuessed));

                clock.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testClocksSmall = clock;
                break;
            case TestState.cube:
                cubeChosen.gameObject.SetActive(false);
                cubeGiven.gameObject.SetActive(false);
                TestResultCube cube = (TestResultCube)activeTest;
                cube.cubeChosen = cubeChosen.options[cubeChosen.value].text;
                cube.cubeGiven = cubeGiven.options[cubeGiven.value].text;

                CubeOptions chosenCube = ((CubeOptions) Enum.Parse(typeof(CubeOptions), cube.cubeChosen));
                CubeOptions givenCube = ((CubeOptions)Enum.Parse(typeof(CubeOptions), cube.cubeGiven));

                float deviationSize = (int)chosenCube - (int)givenCube;
                float deviationPercentage = deviationSize / (float)givenCube;

                cube.deviationCM = deviationSize;
                cube.deviationPercentage = deviationPercentage;

                cube.fillEnd(timeCurrent, InputTracking.GetLocalPosition(XRNode.CenterEye), testDataManager.targetCurrent);
                sl.probandTests.testCube = cube;
                break;
        }
    }

    private float GetTimeDeviation(float[] orig_time, float[] guessed_time)
    {
        float hourDeviation = Mathf.Abs( orig_time[0] - guessed_time[0]);
        float minuteDeviation = Mathf.Abs(orig_time[1] - guessed_time[1]) / 100.0f;
        return hourDeviation + minuteDeviation;
    }

    private float[] clockToFloat(string clockString)
    {
        string[] splittetTimeString = clockString.Split(':');
        float hour = float.Parse(splittetTimeString[0]);
        float minute = float.Parse(splittetTimeString[1]) / 10.0f;
        return new float[] {hour, minute};
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
        sl.SaveAllData();
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

    clockBig,
    clockBigIdle,

    clockNormal,
    clockNormalIdle,

    clockSmall,
    clockSmallIdle,

    cube,
    cubeIdle,

    saveData,
    testEnded
    
}

public enum CubeOptions
{
    CUBE_5_170 = 170,
    CUBE_4_155 = 155,
    CUBE_3_140 = 140,
    CUBE_2_125 = 125,
    CUBE_1_110 = 110
}