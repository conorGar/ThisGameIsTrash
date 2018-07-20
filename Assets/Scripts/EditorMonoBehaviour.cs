using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorMonoBehaviour : MonoBehaviour {
    [ExecuteInEditMode]
    // Use this for initialization
    void OnEnable () {
		if (Application.isPlaying)
        {
            gameObject.GetComponent<Renderer>().enabled = false;  
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
