using UnityEngine;
using System.Collections;

public class IceCreamMayorFriend :  Friend
{

	public GameObject[] townFolk;
	public GameObject squiddyPanPos;
	public GameObject seaPanPos;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		OnUpdate();
	}


	void OnUpdate(){
		GetComponent<ActivateDialogWhenClose>().Execute();
	}

	public void TownFolkPan(){
		StartCoroutine("TownFolkPanSequence");
	}

	IEnumerator TownFolkPanSequence(){

		CamManager.Instance.mainCamEffects.CameraPan(townFolk[0].transform.position, "");

		yield return new WaitForSeconds(2f);
		DialogManager.Instance.ReturnFromAction(true);

		CamManager.Instance.mainCamEffects.CameraPan(townFolk[1].transform.position, "");

		yield return new WaitForSeconds(2f);

		CamManager.Instance.mainCamEffects.CameraPan(townFolk[2].transform.position, "");


	}

	public void SquiddyPan(){
		CamManager.Instance.mainCamEffects.CameraPan(squiddyPanPos.transform.position, "");

	}

	public void SeaPan(){
		CamManager.Instance.mainCamEffects.CameraPan(seaPanPos.transform.position, "");

	}


}
