using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class showXREye : MonoBehaviour {
    GameObject left;
    GameObject right;

    // Use this for initialization
    void Start () {
        left = transform.GetChild(0).gameObject;
        right = transform.GetChild(1).gameObject;

        PupilTools.IsGazing = true;
        PupilTools.SubscribeTo("gaze");

        PupilGazeTracker.Instance.OnUpdate += OnUpdate;
    }

    // Update is called once per frame
    void Update () {
    }
    
    void OnUpdate()
    {
        if (PupilTools.IsGazing && PupilTools.CalibrationMode == Calibration.Mode._2D)
        {
            Vector3 headpos = InputTracking.GetLocalPosition(XRNode.CenterEye);
            left.transform.position = InputTracking.GetLocalPosition(XRNode.LeftEye);
            right.transform.position = InputTracking.GetLocalPosition(XRNode.RightEye);

            Ray rightEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.RightEyePosition);
            Ray leftEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.LeftEyePosition);
            Ray gazeEyeRay = Camera.main.ViewportPointToRay(PupilData._2D.GazePosition);

            Debug.DrawRay(rightEyeRay.origin, leftEyeRay.direction * 100, Color.green);
            Debug.DrawRay(leftEyeRay.origin, rightEyeRay.direction * 100, Color.blue);
            Debug.DrawRay(gazeEyeRay.origin, gazeEyeRay.direction * 100, Color.red);
        }
    }
}
