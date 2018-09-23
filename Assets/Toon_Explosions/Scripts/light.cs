using UnityEngine;
using System.Collections;

public class light : MonoBehaviour {
	public float max_intensity;
	// Use this for initialization
	void Start () {
	this.GetComponent<Light>().intensity = max_intensity;
	}
	
	// Update is called once per frame
	void Update () {
	this.GetComponent<Light>().intensity-= this.GetComponent<Light>().intensity/30f;
	}
}
