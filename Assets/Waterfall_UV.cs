using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterfall_UV : MonoBehaviour {


	public float WF_speed = 0.75f;
	public Renderer WF_renderer;


	void Start () {
		gameObject.GetComponent<MeshRenderer>().sortingLayerName = "Layer03";
		WF_renderer = GetComponent<Renderer>();
	}


	void Update () {
		float TextureOffset = Time.time * WF_speed;
		WF_renderer.material.SetTextureOffset("_MainTex", new Vector2(0,TextureOffset));
	}
}
