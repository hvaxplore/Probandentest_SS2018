using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestRuntimeManager : MonoBehaviour
{
    public TestState testState;

    public SaveLoader sl;
    private TestDataManager testDataManager;

    public Text textInfo;

    public List<Transform> targetObjects;
    

    // Use this for initialization
    void Start()
    {
        testState = TestState.none;
        testDataManager = GetComponent<TestDataManager>();
        sl = GetComponent<SaveLoader>();
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
        testState++;  
    }
    public void prevTest()
    {
        testState--;
    }

    void processData()
    {
        sl.Proband.AddStep(testDataManager);
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

