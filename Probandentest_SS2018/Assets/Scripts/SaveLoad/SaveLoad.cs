using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class SaveLoad : MonoBehaviour {
	public Proband proband;

	public void SaveProband()
	{
		string filePath = "proband" + proband.Id;
        filePath = GetPath(filePath, "lol"); // Filetype does not matter

        FileStream file;

        if (File.Exists(filePath))
        {
            Debug.Log("deleted prev file");
            File.Delete(filePath);
        }
        file = File.Create(filePath);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, proband);
        file.Close();
        Debug.Log("SAVED");
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
