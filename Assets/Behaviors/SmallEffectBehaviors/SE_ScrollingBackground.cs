using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_ScrollingBackground : MonoBehaviour {

	public float scrollSpeed;

	void Start () {
		
	}
	
	void Update () {
		float x = Mathf.Repeat (Time.time* scrollSpeed,1);
		Vector2 offSet = new Vector2(x,0); //NOTE: only horizontal scroll currently
		gameObject.GetComponent<Renderer>().sharedMaterial.SetTextureOffset ("_MainTex", offSet);
	}
}
