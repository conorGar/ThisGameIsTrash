using UnityEngine;
using System.Collections;

public class EnemyKillBlockManager : MonoBehaviour
{
	[HideInInspector]
	public int neededKillCount = 0; //increased by Room.cs at enemy spawn

	public GameObject doorGate;


	int activatedSwitchCount;
	public void IncrementActivatedCount(){
		activatedSwitchCount++;
		if(activatedSwitchCount >= neededKillCount){
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

