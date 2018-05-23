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
    public Transform marker;
    public Transform camLeft;
    public Transform camRight;
    public Transform camCyclop; // is the head transform

	/*
    public Transform head;
    public Transform eyeLeft;
    public Transform eyeRight;
	*/
    public Transform handLeft;
    public Transform handRight;

	public Transform targetCurrent;
	public float distanceToTarget;

    public float rayScale = 0;

    public float convergenceAngle;
    public float convergenceAngleL;
    public float convergenceAngleR;
    public float convergenceAngle_plane;
    public float convergenceDistance;

    //public AbsoluteParallax targetObj;

    public bool debugMode;

    private float times;

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

        if (PupilTools.IsConnected && PupilTools.DataProcessState == Pupil.EStatus.ProcessingGaze)
        {
            Vector3 gazeLeft = camCyclop.rotation * PupilData._3D.LeftGazeNormal;
            Vector3 gazeRight = camCyclop.rotation * PupilData._3D.RightGazeNormal;

            if(debugMode)
            {
                Debug.DrawRay(Eyes.instance.leftEyeCam.transform.position, gazeLeft * rayScale, Color.red);
                Debug.DrawRay(Eyes.instance.rightEyeCam.transform.position, gazeRight * rayScale, Color.blue);
            }

            convergenceAngle = Vector3.Angle(gazeLeft, gazeRight);
            convergenceAngleL = Vector3.Angle(gazeLeft, camCyclop.transform.forward);
            convergenceAngleR = Vector3.Angle(gazeRight, camCyclop.transform.forward);
            convergenceAngle_plane = Vector3.Angle(new Vector3(gazeLeft.x, 0, gazeLeft.z), new Vector3(gazeRight.x, 0, gazeRight.z));
            convergenceDistance = (Eyes.instance.interPupillarDistance / 2.0f) / Mathf.Tan(convergenceAngle / 2.0f);
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
