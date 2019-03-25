using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
	public Room myRoom;
	void Start ()
	{
	
	}
	
	void Update ()
	{
	
	}

	void OnTriggerEnter2D(Collider2D collider){
		if(CheckpointManager.Instance.lastCheckpoint != this){
			Debug.Log("Checkpoint Activated");
			CheckpointManager.Instance.lastCheckpoint = this;
			GUIManager.Instance.checkPointDisplay.SetActive(true);
			StartCoroutine("DisplayStop");
		}
	}

	IEnumerator DisplayStop(){
		yield return new WaitForSeconds(1f);
		GUIManager.Instance.checkPointDisplay.SetActive(false);

	}

}

