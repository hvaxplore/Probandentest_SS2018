using UnityEngine;

/// <summary>
/// Contains all var values of the test for easy access
/// </summary>
public class TestDataManager : MonoBehaviour
{
    public TestRuntimeManager runtimeManager;
    private TestState testState;

    public Transform camLeft;
    public Transform camRight;
    public Transform camCyclop; // is the head transform

	public Transform targetCurrent; // 
	public float distanceToTarget;

    public float rayScale = 0;

    public bool debugMode;

    public TestState TestState
    {
        get
        {
            return testState;
        }

        set
        {
            testState = (TestState) Mathf.Max(0, Mathf.Min(TestState.GetNames(typeof(TestState)).Length - 1, (int)value));
        }
    }


    // Use this for initialization
    void Start()
    {
        PupilData.calculateMovingAverage = true;
        runtimeManager = GetComponent<TestRuntimeManager>();

        camLeft = Eyes.instance.leftEyeCam.transform;
        camRight = Eyes.instance.rightEyeCam.transform;
        camCyclop = Eyes.instance.cyclopCam.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (PupilTools.IsConnected)
        {
            Vector3 gazeLeft = camCyclop.rotation * PupilData._3D.LeftGazeNormal;
            Vector3 gazeRight = camCyclop.rotation * PupilData._3D.RightGazeNormal;

            if(debugMode)
            {
                Debug.DrawRay(Eyes.instance.leftEyeCam.transform.position, gazeLeft * rayScale, Color.red);
                Debug.DrawRay(Eyes.instance.rightEyeCam.transform.position, gazeRight * rayScale, Color.blue);
            }
        }
    }

    void OnEnable()
    {
        if (PupilTools.IsConnected)
        {
            PupilGazeTracker.Instance.StartVisualizingGaze();
        }
    }

    void OnDisable()
    {
        if (PupilTools.IsConnected)
        {
            PupilTools.UnSubscribeFrom("gaze");
            print("We stopped gazing");
        }
    }
}