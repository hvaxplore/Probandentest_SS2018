using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KillChildsOnEnable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTransformChildrenChanged()
    {
        try
        {
            Destroy(transform.GetChild(1).gameObject);
        }
        catch(Exception e)
        {

        }
        
    }
}
