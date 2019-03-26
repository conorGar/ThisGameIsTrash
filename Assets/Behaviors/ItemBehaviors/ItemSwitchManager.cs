using UnityEngine;
using System.Collections;

public class ItemSwitchManager : MonoBehaviour
{
	/// <summary>
	/// Should be local to a specific room, interacts with the various switches in the room and is in charge of counting which ones are activated, and what to do once activated.
	/// </summary>
	public int neededSwitchCount = 0;
	public GameObject doorGate;


	int activatedSwitchCount;
	

	public void IncrementActivatedCount(){
		activatedSwitchCount++;
		if(activatedSwitchCount >= neededSwitchCount){
			StartCoroutine("OpenDoor");
		}
	}

	IEnumerator OpenDoor(){
		GameStateManager.Instance.PushState(typeof(MovieState));

		CamManager.Instance.mainCamEffects.CameraPan(doorGate.transform.position,"");
		yield return new WaitUntil(() => (Vector2.Distance( CamManager.Instance.mainCam.transform.position,doorGate.transform.position) <5f));
		yield return new WaitForSeconds(.5f);

		ObjectPool.Instance.GetPooledObject("effect_smokeBurst",doorGate.transform.position);
		doorGate.SetActive(false);
		yield return new WaitForSeconds(.5f);
		GameStateManager.Instance.PopState();
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
	}

}

