using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
        transform.LookAt(target);
    }

    private void Update()
    {
        transform.LookAt(target);

    }
}
