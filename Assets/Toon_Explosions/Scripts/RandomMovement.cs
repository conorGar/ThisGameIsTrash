using UnityEngine;
using System.Collections;

public class RandomMovement : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
	this.transform.eulerAngles = new Vector3(0f,Random.Range(0f,360f),0f);
	}
	
	// Update is called once per frame
	void Update () {
	this.transform.position += transform.forward/10f;
	}
}
