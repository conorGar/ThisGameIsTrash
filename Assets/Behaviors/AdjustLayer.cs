using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustLayer : MonoBehaviour {

	public string bottomLayer = "Background";
	public string topLayer = "Layer04";
	public float yAdjustment = 0f;

	float yThreshold;
	SpriteRenderer myRenderer;
	//float xThreshold;
	// Use this for initialization
	void Start () {
		yThreshold = gameObject.transform.position.y + gameObject.transform.lossyScale.y/2;
		myRenderer = gameObject.GetComponent<SpriteRenderer>();
		//xThreshold = gameObject.transform.lossyScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerManager.Instance.player.transform.position.y > yThreshold && myRenderer.sortingLayerName != topLayer){
			myRenderer.sortingLayerName = topLayer;
		}else if(PlayerManager.Instance.player.transform.position.y < yThreshold && myRenderer.sortingLayerName != bottomLayer){
			myRenderer.sortingLayerName = bottomLayer;
		}
	}
}
