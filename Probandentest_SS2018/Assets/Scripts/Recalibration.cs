using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Recalibration : MonoBehaviour {
    public CalibrationData dat;
    public GameObject target;

    public InputField posX;
    public InputField posY;
    public InputField posZ;

    public InputField rotX;
    public InputField rotY;
    public InputField rotZ;

    public InputField scaleX;
    public InputField scaleY;
    public InputField scaleZ;

    void Start()
    {
        if(File.Exists(Path.Combine(Application.streamingAssetsPath, "Calibration.json")))
        {
            LoadCalibration();
        } else
        {
            LoadGOToUIPos();
            LoadGOToUIRot();
            LoadGOToUIScale();
        }
    }

    // Update is called once per frame
    void Update () {
        TransferUIToGOPos();
        TransferUIToGORot();
        TransferUIToGOScale();
    }

    /// <summary>
    /// Will be called to Load Dat to UI
    /// </summary>
    public void LoadDatToUITransferPos()
    {
        posX.text = dat.pos.x.ToString();
        posY.text = dat.pos.y.ToString();
        posZ.text = dat.pos.z.ToString();
    }
    public void LoadDatToUITransferRot()
    {
        Vector3 eulers = dat.rot.eulerAngles;
        rotX.text = eulers.x.ToString();
        rotY.text = eulers.y.ToString();
        rotZ.text = eulers.z.ToString();
    }
    public void LoadDatToUITransferScale()
    {
        scaleX.text = dat.scale.x.ToString();
        scaleY.text = dat.scale.y.ToString();
        scaleZ.text = dat.scale.z.ToString();
    }

    /// <summary>
    /// Will be called to transfer current UI to GO
    /// </summary>
    public void TransferUIToGOPos()
    {
        Vector3 newPos = new Vector3(float.Parse(posX.text), float.Parse(posY.text), float.Parse(posZ.text));
        target.transform.position = newPos;
    }
    public void TransferUIToGORot()
    {
        Vector3 newRot = new Vector3(float.Parse(rotX.text), float.Parse(rotY.text), float.Parse(rotZ.text));
        target.transform.rotation = Quaternion.Euler(newRot);
    }
    public void TransferUIToGOScale()
    {
        Vector3 newScale = new Vector3(float.Parse(scaleX.text), float.Parse(scaleY.text), float.Parse(scaleZ.text));
        target.transform.localScale = newScale;
    }

    /// <summary>
    /// Will be called if nothing could be loaded, transfers current GO transform into UI
    /// </summary>
    public void LoadGOToUIPos()
    {
        posX.text = target.transform.position.x.ToString();
        posY.text = target.transform.position.y.ToString();
        posZ.text = target.transform.position.z.ToString();
    }
    public void LoadGOToUIRot()
    {
        Vector3 eulers = target.transform.rotation.eulerAngles;
        rotX.text = eulers.x.ToString();
        rotY.text = eulers.y.ToString();
        rotZ.text = eulers.z.ToString();
    }
    public void LoadGOToUIScale()
    {
        scaleX.text = target.transform.localScale.x.ToString();
        scaleY.text = target.transform.localScale.y.ToString();
        scaleZ.text = target.transform.localScale.z.ToString();
    }

    public void SaveCalibration()
    {
        dat.SetValues(target);

        string filePath = Path.Combine(Application.streamingAssetsPath, "Calibration.json");
        string fileData = JsonUtility.ToJson(dat);

        if(File.Exists(filePath))
            File.Delete(filePath);

        File.WriteAllText(filePath, fileData);

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public void LoadCalibration()
    {
        if (!File.Exists(Path.Combine(Application.streamingAssetsPath, "Calibration.json")))
        {
            return;
        }

        string filePath = Path.Combine(Application.streamingAssetsPath, "Calibration.json");
        string fileData = File.ReadAllText(filePath);

        dat = JsonUtility.FromJson<CalibrationData>(fileData);

        LoadDatToUITransferPos();
        LoadDatToUITransferRot();
        LoadDatToUITransferScale();
        dat.SetValues(target);
    }
}

[System.Serializable]
public class CalibrationData
{
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    /// <summary>
    /// Set Dat value to target GO values
    /// </summary>
    /// <param name="_target"></param>
    public void SetValues(GameObject _target)
    {
        pos = _target.transform.position;
        rot = _target.transform.rotation;
        scale = _target.transform.localScale;
    }

    /// <summary>
    /// Transfer current dat to GO
    /// </summary>
    /// <param name="_target"></param>
    public void TransferCalibration(GameObject _target)
    {
        _target.transform.position = pos;
        _target.transform.rotation = rot;
        _target.transform.localScale = scale;
    }
}