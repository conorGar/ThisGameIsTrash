using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMeshSorter : MonoBehaviour {
	public string sortingLayerName;
	public int sortingOrder;


	void Awake(){
		gameObject.GetComponent<MeshRenderer>().sortingLayerName = sortingLayerName;

		gameObject.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
	}
}
