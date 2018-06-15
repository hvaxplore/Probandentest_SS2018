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
        probandMeta.IPD = float.Parse(idp.text);
        probandMeta.EyeHeight = float.Parse(eyeHeight.text);

        probandMeta.gender = gender.options[gender.value].text;
        probandMeta.vraffinity = vr.options[vr.value].text;
        probandMeta.sehhilfe = sehhilfe.options[sehhilfe.value].text;
        probandMeta.sehfehler = sehfehler.options[sehfehler.value].text;
        probandMeta.isVR_Proband = isVRProband.isOn;
    }

    public void SaveProbandTasks()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_tasks.json");

        string fileData = JsonUtility.ToJson(probandTests);

        File.WriteAllText(filePath, fileData);
        UnityEditor.AssetDatabase.Refresh();

    }

    public void SaveProbandSteps()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_steps.json");

        string fileData = JsonUtility.ToJson(probandSteps);

        File.WriteAllText(filePath, fileData);
        UnityEditor.AssetDatabase.Refresh();
    }

    public void SaveProbandMeta()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "proband_" + int.Parse(id.text) + "_meta.json");

        string fileData = JsonUtility.ToJson(probandMeta);

        File.WriteAllText(filePath, fileData);
        UnityEditor.AssetDatabase.Refresh();
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
