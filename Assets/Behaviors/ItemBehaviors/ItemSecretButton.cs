using UnityEngine;
using System.Collections;

public class ItemSecretButton : MonoBehaviour
{
	public ItemSwitchManager myManager;
	bool isTriggered;
	// Use this for initialization
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.tag == "Player" && !isTriggered){
			myManager.IncrementActivatedCount();
			isTriggered = true;
		}
	}
}

