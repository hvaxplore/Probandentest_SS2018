using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {
	public ProbandTests probandTests;
    public ProbandSteps probandSteps;

    private void Start()
    {
        probandTests = new ProbandTests();
        probandSteps = new ProbandSteps();
    }

    public void SaveProbandTasks()
	{
		string filePath = Path.Combine(Application.streamingAssetsPath, "proband_tasks_" + probandTests.Id + ".json");

        string fileData = JsonUtility.ToJson(probandTests);

        File.WriteAllText(filePath, fileData);
        UnityEditor.AssetDatabase.Refresh();

    }

    public void SaveProbandSteps()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "proband_steps_" + probandSteps.Id + ".json");

        string fileData = JsonUtility.ToJson(probandSteps);

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
