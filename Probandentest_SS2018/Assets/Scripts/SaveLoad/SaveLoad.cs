using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoad : MonoBehaviour {
	public ProbandTests probandTests;
    public ProbandSteps probandSteps;
    public ProbandMeta probandMeta;

    public InputField id;
    public InputField idp;
    public InputField eyeHeight;
    public Dropdown gender;
    public Dropdown vr;
    public Dropdown sehhilfe;
    public Dropdown sehfehler;
    public Toggle isVRProband;


    private void Start()
    {
        probandTests = new ProbandTests();
        probandSteps = new ProbandSteps();
        probandMeta = new ProbandMeta();
    }

    public void AddFormData()
    {
        probandMeta.Id = int.Parse(id.text);
        //probandMeta.IPD = 42f;
        //probandMeta.EyeHeight = 42f;

        //probandMeta.gender = "42";
        //probandMeta.vraffinity = "42";
        //probandMeta.sehhilfe = "42";
        //probandMeta.sehfehler = "42";
        probandMeta.isVR_Proband = isVRProband.isOn;
    }

    public void SaveAllData()
    {
        string realVr = "";
        if(probandMeta.isVR_Proband)
        {
            realVr = "vr";
        }
        else
        {
            realVr = "rl";
        }
        SaveProbandTasks(realVr);
        SaveProbandSteps(realVr);
        SaveProbandMeta(realVr);
    }

    public void SaveProbandTasks(string _realVr)
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_" + _realVr + "_tasks.json");

        string fileData = JsonUtility.ToJson(probandTests);

        File.WriteAllText(filePath, fileData);
#if UNITY_EDITOR

        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public void SaveProbandSteps(string _realVr)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_" + _realVr + "_steps.json");

        string fileData = JsonUtility.ToJson(probandSteps);

        File.WriteAllText(filePath, fileData);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    public void SaveProbandMeta(string _realVr)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_" + _realVr + "_meta.json");

        string fileData = JsonUtility.ToJson(probandMeta);

        File.WriteAllText(filePath, fileData);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }


    // TODO: Autocreate folder in build
    public string GetPath(string _filename, string _filetype)
    {
		#if UNITY_EDITOR
				return Application.dataPath + "/" + _filename + "." + _filetype;
		#elif UNITY_ANDROID
				return Application.persistentDataPath + "/" + _filename + "." + _filetype;
		#elif UNITY_IPHONE
				return Application.persistentDataPath + "/" + _filename + "." + _filetype;
		#else
				return Application.dataPath + "/" + _filename + "." + _filetype;
		#endif
    }
}
