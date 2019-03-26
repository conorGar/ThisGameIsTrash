using UnityEngine;
using System.Collections;

public class Ev_TrashBlockadeDoor : MonoBehaviour
{
	public int trashNeeded;
	public GameObject door;
	public AudioClip triggerSound;
	public GlobalVariableManager.TRASHDOORS myDoor;

	bool triggered;

	//TODO: Destroyed Doors stay destroyed between days

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerEnter2D(Collider2D collider){
		Debug.Log("OnTriggerEnter");
		if(collider.gameObject.tag == "Player"){
			if(!triggered && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= trashNeeded){
				GameStateManager.Instance.PushState(typeof(MovieState));
				SoundManager.instance.PlaySingle(triggerSound);
				Debug.Log("Tiggered Open Door event");
				StartCoroutine("OpenEvent");
				triggered = true;
			}
		}
	}
	void OnCollisionEnter2D(Collision2D collider){
		Debug.Log("OnCollisionEnter");
		if(collider.gameObject.tag == "Player"){

			if(!triggered && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] >= trashNeeded){
				SoundManager.instance.PlaySingle(triggerSound);
				Debug.Log("Collision Open Door event");
				StartCoroutine("OpenEvent");
				triggered = true;
			}
		}
	}

	IEnumerator OpenEvent(){
		
		CamManager.Instance.mainCamEffects.CameraPan(door.transform.position,"");
		yield return new WaitUntil(() => Vector2.Distance(CamManager.Instance.mainCam.gameObject.transform.position,door.transform.position) < 1f);
		yield return new WaitForSeconds(1f);
		ObjectPool.Instance.GetPooledObject("effect_smokeBurst",door.transform.position);
		door.SetActive(false);
		yield return new WaitForSeconds(.5f);
		GlobalVariableManager.Instance.BROKEN_TRASH_DOORS |= myDoor;
		GameStateManager.Instance.PopState();
		CamManager.Instance.mainCamEffects.ReturnFromCamEffect();
	}
}

