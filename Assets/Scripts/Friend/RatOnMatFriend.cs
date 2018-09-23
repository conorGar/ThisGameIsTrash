using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RatOnMatFriend : Friend
{
	public ParticleSystem vanishPS;
	public tk2dCamera mainCam;

	public override void FinishDialogEvent(){
		Debug.Log("Ec finish dialog event activate");
		StartCoroutine("ReturnCam");
	}

	IEnumerator ReturnCam(){
		yield return new WaitForSeconds(.3f);


		vanishPS.Play();

	
		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<MeshRenderer>().enabled =true;
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		mainCam.GetComponent<Ev_MainCameraEffects>().ReturnFromCamEffect();


		gameObject.SetActive(false);

	}
}

