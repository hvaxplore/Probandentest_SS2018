using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Contains all var values of the test for easy access
/// </summary>
public class TestDataManager : MonoBehaviour
{
    public TestRuntimeManager runtimeManager;
    public TestState testState;

    public Transform camLeft;
    public Transform camRight;
    public Transform camCyclop; // is the head transform

	public Transform targetCurrent; // 
	public float distanceToTarget;

    public float timeCurrent;

    public float rayScale = 0;

    public bool debugMode;


    // Use this for initialization
    void Start()
    {
        PupilData.calculateMovingAverage = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(debugMode)
        {
            Debug.Log("gaze: " + PupilTools.ConfidenceForDictionary(PupilTools.gazeDictionary));
            Debug.Log("0 " + PupilTools.ConfidenceForDictionary(PupilTools.pupil0Dictionary));
            Debug.Log("1 " + PupilTools.ConfidenceForDictionary(PupilTools.pupil1Dictionary));
        }

        testState = runtimeManager.testState;

        if (PupilTools.IsConnected && PupilTools.DataProcessState == Pupil.EStatus.ProcessingGaze)
        {
            Vector3 gazeLeft = camCyclop.rotation * PupilData._3D.LeftGazeNormal;
            Vector3 gazeRight = camCyclop.rotation * PupilData._3D.RightGazeNormal;

            timeCurrent+= Time.deltaTime;

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
        if (PupilTools.IsConnected && PupilTools.DataProcessState == Pupil.EStatus.ProcessingGaze)
        {
            PupilTools.UnSubscribeFrom("gaze");
            print("We stopped gazing");
        }
    }
}