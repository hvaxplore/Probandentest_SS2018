using System;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Contains all var values of the test for easy access
/// </summary>
public class TestDataManager : MonoBehaviour
{
    public TestRuntimeManager runtimeManager;
    private TestState testState;
    public SaveLoad sl;

    public Vector3 headPos;
    public Quaternion headRot;
    public Vector3 eyegaze;
    public Vector3 targetCurrent; // 
    public float distanceToTarget;
    public float rayScale = 0;
    public ReverseGyro realEyeTrackerHelper;
    public Transform hardwareTracker;

    public bool eyeTrackerMode;
    public bool debugMode;
    public Transform testObj;

    public TestState TestState
    {
        get
        {
            return testState;
        }

        set
        {
            testState = (TestState)Mathf.Max(0, Mathf.Min(TestState.GetNames(typeof(TestState)).Length - 1, (int)value));
        }
    }


    // Use this for initialization
    void Start()
    {
        sl = this.GetComponent<SaveLoad>();
        runtimeManager = GetComponent<TestRuntimeManager>();

        if (eyeTrackerMode)
        {
            PupilData.calculateMovingAverage = true;

            PupilTools.IsGazing = true;
            PupilTools.SubscribeTo("gaze");

            PupilGazeTracker.Instance.OnUpdate += OnUpdate;
        }

        InputTracking.nodeAdded += nodeTrackerAdded;
        InputTracking.nodeRemoved += nodeTrackerAdded;
    }

    private void nodeTrackerAdded(XRNodeState obj)
    {
        Vector3 nodePos = Vector3.zero;
        obj.TryGetPosition(out nodePos);
        Debug.Log(obj.nodeType + " Pos: " + nodePos.ToString() + " tracked: " + obj.tracked + " id: " + obj.uniqueID);
    }

    void Update()
    {
        if (runtimeManager.sl.probandMeta.isVR_Proband)
        {
            headPos = InputTracking.GetLocalPosition(XRNode.CenterEye);
            headRot = InputTracking.GetLocalRotation(XRNode.CenterEye);
        }
        else
        {
            headPos = hardwareTracker.position;
            headRot = hardwareTracker.rotation * Quaternion.Euler(90, 0, 0);

            Camera.main.transform.position = headPos;
            Camera.main.transform.rotation = headRot;
        }

        testObj.position = headPos;
        testObj.rotation = headRot;
        Debug.DrawRay(testObj.position, testObj.forward * 100, Color.red);
    }

    void OnUpdate()
    {

        if (PupilTools.IsGazing && PupilTools.CalibrationMode == Calibration.Mode._2D && eyeTrackerMode)
        {
            Ray rightEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.RightEyePosition);
            Ray leftEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.LeftEyePosition);
            Ray gazeEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.GazePosition);

            Vector3 gazeDir = gazeEyeRay.direction;

            eyegaze = gazeDir;

            /// Only do this for the real proband
            if (!sl.probandMeta.isVR_Proband && testState!=TestState.none)
            {
                realEyeTrackerHelper.RecalculateEyeTracker(); // Recalculate the eye tracker rotation onto the hardware tracker
                eyegaze = realEyeTrackerHelper.EyeRot.transform.rotation.eulerAngles; // then get the new rotation instead
            }

            //if (debugMode)
            //{
            //    Debug.DrawRay(rightEyeRay.origin, leftEyeRay.direction * 100, Color.green);
            //    Debug.DrawRay(leftEyeRay.origin, rightEyeRay.direction * 100, Color.blue);
            //    Debug.DrawRay(gazeEyeRay.origin, gazeDir * 100, Color.red);
            //}

        }
    }

    void OnEnable()
    {
        //if (PupilTools.IsConnected && eyeTrackerMode)
        //{
        //    //PupilGazeTracker.Instance.StartVisualizingGaze();
        //}
    }

    void OnDisable()
    {
        if (PupilTools.IsConnected && eyeTrackerMode)
        {
            PupilTools.UnSubscribeFrom("gaze");
            print("We stopped gazing");
        }
    }
}