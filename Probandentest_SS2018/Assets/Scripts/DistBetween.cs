using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DistBetween : MonoBehaviour {

    public float dist;
    public Transform obj1;
    public Transform obj2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CalcDist()
    {
        dist = Vector3.Distance(obj1.position, obj2.position);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DistBetween))]
public class DistBetweenInspector : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DistBetween myScript = (DistBetween)target;
        if (GUILayout.Button("Calc Dist"))
        {
            myScript.CalcDist();
        }
    }

}
#endif