using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used for reversing the pseudo-gyroscope of the reality pupil labs eyetracker
/// </summary>
public class ReverseGyro : MonoBehaviour {
    public TestDataManager testData;

    public GameObject obj1;
    public Transform HardWareTracker;
    public GameObject obj2;

    public GameObject hardWarePosDir;
    public GameObject EyeRot;

    // Update is called once per frame
    public void RecalculateEyeTracker ()
    {
        Vector3 newGaze = testData.eyegaze;
        newGaze = new Vector3(newGaze.x, newGaze.y, newGaze.z);
        obj1.transform.localRotation = Quaternion.LookRotation(newGaze);
        obj2.transform.rotation = HardWareTracker.rotation;
        hardWarePosDir.transform.position = HardWareTracker.position;
        hardWarePosDir.transform.rotation = HardWareTracker.rotation;
        EyeRot.transform.localRotation = obj1.transform.rotation;
    }
}
