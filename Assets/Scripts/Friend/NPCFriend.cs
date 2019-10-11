using UnityEngine;
using System.Collections;

public class NPCFriend : Friend
{

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
}

