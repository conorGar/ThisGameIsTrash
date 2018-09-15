using UnityEngine;
using System.Collections;

public class BossFriendEx : Friend
{

	//*** For now using for all 3 of the trio, change if needed at any point
	
	public tk2dCamera mainCam;
	public ParticleSystem myTeleportPS;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void FinishDialogEvent(){
		Debug.Log("Ec finish dialog event activate");
		StartCoroutine("ReturnCam");
	}

	IEnumerator ReturnCam(){
		yield return new WaitForSeconds(.3f);
		gameObject.GetComponent<MeshRenderer>().enabled =false;//hide sprite
		myTeleportPS.gameObject.SetActive(true);
		myTeleportPS.Play();
		yield return new WaitForSeconds(1f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();
		gameObject.SetActive(false);

	}
}

