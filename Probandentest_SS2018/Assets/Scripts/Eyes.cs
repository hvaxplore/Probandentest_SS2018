using UnityEngine;
using UnityEngine.XR;
using UnityEditor;

public class Eyes : MonoBehaviour
{
    public static Eyes instance;

    [Range(0.0f, 1f)]
    public float interPupillarDistance;
    [Range(0, 90)]
    public float convergenceAngle;

    public bool fixPosition;
    //public bool lockRotation;
    public bool getStereobasisFromVive;
    public bool debugMode;

    public Camera leftEyeCam;
    public Camera rightEyeCam;
    public Camera cyclopCam;

    private Transform leftEyeWrapper;
    private Transform rightEyeWrapper;
    private Transform leftEyeTarget;
    private Transform rightEyeTarget;

    public float viveStereoseparation;

    private void Awake()
    {
        instance = this;

        Camera[] cams = transform.GetComponentsInChildren<Camera>();
        leftEyeCam = cams[0];
        rightEyeCam = cams[1];
        cyclopCam = cams[2];

        if (PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass)
        {
            leftEyeCam.stereoTargetEye = StereoTargetEyeMask.Left;
            rightEyeCam.stereoTargetEye = StereoTargetEyeMask.Right;
        } else
        {
            leftEyeCam.stereoTargetEye = StereoTargetEyeMask.Both;
            rightEyeCam.stereoTargetEye = StereoTargetEyeMask.None;
        }

        leftEyeWrapper = leftEyeCam.transform.parent;
        rightEyeWrapper = rightEyeCam.transform.parent;

        transform.localPosition = Vector3.zero;
        InputTracking.disablePositionalTracking = fixPosition;

        leftEyeTarget = cyclopCam.transform.GetChild(0).transform;
        rightEyeTarget = cyclopCam.transform.GetChild(1).transform;
    }

    // Update is called once per frame
    void Update()
    {
        //InputTracking.disablePositionalTracking = fixPosition;

        if (getStereobasisFromVive)
        {
            viveStereoseparation = Vector3.Distance(InputTracking.GetLocalPosition(XRNode.LeftEye), InputTracking.GetLocalPosition(XRNode.RightEye));
            interPupillarDistance = viveStereoseparation;
        }

        if(fixPosition)
        {
            cyclopCam.transform.position = Vector3.zero;

            float IPD = Vector3.Distance(leftEyeCam.transform.position, rightEyeCam.transform.position);

            /*
            float cam_euler_y = InputTracking.GetLocalRotation(XRNode.LeftEye).eulerAngles.y + 90;
            float cam_euler_z = InputTracking.GetLocalRotation(XRNode.LeftEye).eulerAngles.z;

            //float newX = (Mathf.Sin(Mathf.Deg2Rad * cam_euler_y) * interPupillarDistance / 2.0f) + (Mathf.Cos(Mathf.Deg2Rad * cam_euler_z) * interPupillarDistance / 2.0f);
            //float newY = -(interPupillarDistance / 2.0f) - Mathf.Sin(Mathf.Deg2Rad * cam_euler_z) * interPupillarDistance / 2.0f;
            //float newZ = Mathf.Cos(Mathf.Deg2Rad * cam_euler_y) * interPupillarDistance / 2.0f;

            float newX = (Mathf.Sin(Mathf.Deg2Rad * cam_euler_y) * interPupillarDistance / 2.0f) - (interPupillarDistance / 2.0f) - (Mathf.Cos(Mathf.Deg2Rad * cam_euler_z) * interPupillarDistance / 2.0f);
            float newY = Mathf.Sin(Mathf.Deg2Rad * cam_euler_z) * interPupillarDistance / 2.0f;
            float newZ = Mathf.Cos(Mathf.Deg2Rad * cam_euler_y) * interPupillarDistance / 2.0f;
            Vector3 camTransform = new Vector3(newX, newY, newZ);

            leftEyeCam.transform.position = -camTransform;
            rightEyeCam.transform.position = camTransform;

            leftEyeCam.transform.localPosition = new Vector3(-interPupillarDistance / 2.0f, 0, 0);
            rightEyeCam.transform.localPosition = new Vector3(interPupillarDistance / 2.0f, 0, 0);
            */

            //leftEyeWrapper.transform.localPosition = leftEyeTarget.transform.position - cyclopCam.transform.right * interPupillarDistance / 2.0f;
            //rightEyeWrapper.transform.localPosition = rightEyeTarget.transform.position + cyclopCam.transform.right * interPupillarDistance / 2.0f;

            //leftEyeCam.transform.rotation = leftEyeTarget.transform.rotation;
            //rightEyeCam.transform.rotation = rightEyeTarget.transform.rotation;
        }
        else
        {
            //float cam_euler_y = InputTracking.GetLocalRotation(XRNode.LeftEye).eulerAngles.y + 90;
            //float cam_euler_z = InputTracking.GetLocalRotation(XRNode.LeftEye).eulerAngles.z;

            //float newX = Mathf.Sin(Mathf.Deg2Rad * cam_euler_y * 1) * interPupillarDistance / 2.0f;
            //float newZ = Mathf.Cos(Mathf.Deg2Rad * cam_euler_y * 1) * interPupillarDistance / 2.0f;
            //Vector3 camTransform = new Vector3(newX, 0, newZ);
            //leftEyeCam.transform.position += -camTransform;
            //rightEyeCam.transform.position += camTransform;
        }


        leftEyeWrapper.transform.localPosition = leftEyeTarget.transform.position - cyclopCam.transform.right * interPupillarDistance / 2.0f;
        rightEyeWrapper.transform.localPosition = rightEyeTarget.transform.position + cyclopCam.transform.right * interPupillarDistance / 2.0f;

        leftEyeWrapper.transform.localRotation = Quaternion.Euler(leftEyeTarget.transform.up * convergenceAngle / 2.0f) * leftEyeTarget.transform.rotation;
        rightEyeWrapper.transform.localRotation = Quaternion.Euler(-leftEyeTarget.transform.up * convergenceAngle / 2.0f) * rightEyeTarget.transform.rotation;


        //if (PlayerSettings.stereoRenderingPath == StereoRenderingPath.SinglePass)
        //{
        //    leftEyeWrapper.transform.localPosition = new Vector3(-interPupillarDistance / 2.0f, 0, 0);
        //    rightEyeWrapper.transform.localPosition = new Vector3(interPupillarDistance / 2.0f, 0, 0);
        //}
        //else
        //{
        //    leftEyeWrapper.transform.localPosition = new Vector3(-interPupillarDistance / 2.0f, 0, 0);
        //    rightEyeWrapper.transform.localPosition = new Vector3(interPupillarDistance / 2.0f, 0, 0);
        //}

        //if (fixPosition)
        //{
        //    _campos_l = new Vector3(0, 0, 0);
        //    _campos_r = new Vector3(0, 0, 0);
        //}
        //else
        //{
        //    _campos_l = InputTracking.GetLocalPosition(XRNode.LeftEye);
        //    _campos_r = InputTracking.GetLocalPosition(XRNode.RightEye);
        //}

        //leftEyeCam.transform.localPosition = _campos_l;
        //rightEyeCam.transform.localPosition = _campos_r;

        //if (lockRotation)
        //{
        //    //leftEyeWrapper.transform.localRotation = Quaternion.Inverse(leftEyeCam.transform.localRotation * Quaternion.Euler(new Vector3(0, -convergenceAngle / 2.0f, 0)));
        //    //rightEyeWrapper.transform.localRotation = Quaternion.Inverse(rightEyeCam.transform.localRotation * Quaternion.Euler(new Vector3(0, convergenceAngle / 2.0f, 0)));

        //    leftEyeWrapper.transform.localRotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.LeftEye));
        //    rightEyeWrapper.transform.localRotation = Quaternion.Inverse(InputTracking.GetLocalRotation(XRNode.RightEye));
        //}
        //else
        //{
        //    leftEyeWrapper.transform.localRotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, convergenceAngle / 2.0f, 0));
        //    rightEyeWrapper.transform.localRotation = Quaternion.identity * Quaternion.Euler(new Vector3(0, -convergenceAngle / 2.0f, 0));
        //}

        //leftEyeCam.transform.localPosition = InputTracking.GetLocalPosition(XRNode.LeftEye);
        //rightEyeCam.transform.localPosition = InputTracking.GetLocalPosition(XRNode.RightEye);

        //leftEyeCam.transform.rotation = InputTracking.GetLocalRotation(XRNode.LeftEye);
        //rightEyeCam.transform.rotation = InputTracking.GetLocalRotation(XRNode.RightEye);

        //Debug.Log(Vector3.Distance(InputTracking.GetLocalPosition(XRNode.LeftEye), InputTracking.GetLocalPosition(XRNode.RightEye)) + " POS left: " + InputTracking.GetLocalPosition);


        if (debugMode)
        {
            Ray leftEyeRay = new Ray(leftEyeCam.transform.position, leftEyeCam.transform.forward);
            Ray rightEyeRay = new Ray(rightEyeCam.transform.position, rightEyeCam.transform.forward);
            Debug.DrawRay(leftEyeRay.origin, leftEyeRay.direction * 100, Color.red);
            Debug.DrawRay(rightEyeRay.origin, rightEyeRay.direction * 100, Color.green);
        }
    }
}
