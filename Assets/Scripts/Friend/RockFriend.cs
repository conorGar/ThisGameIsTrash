using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

/*public enum DESIRED_OBJECT
{
    NONE = 0,
    LIPSTICK,
    WIG,
    EYELINER,
    SUIT
}*/


// Covers the 3 rock-based friends (Rock, Stone, Slab)
public class RockFriend : Friend {

   
    public List<SpecialFriendObject> desiredObject = new List<SpecialFriendObject>();//the needed objects start off as the friend's children
    private List<SpecialFriendObject> deliveredObjects = new List<SpecialFriendObject>();
    List<SpecialFriendObject> pickedUpObjects = new List<SpecialFriendObject>();

    public GUI_RockItemHUD itemHUD;
    public GameObject mainCam;
    public GameObject eyeBreakPS;
    public GameObject eyeCover;
    public GameObject slab;
    public GameObject stone;


    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }
    public void OnEnable(){
    	base.OnEnable();
    	mainCam = GameObject.Find("tk2dCamera");
    }
    public void DeliverObject(SpecialFriendObject obj)
    {
    	for(int i = 0; i < pickedUpObjects.Count;i++){
    		deliveredObjects.Add(pickedUpObjects[i]);
    	}
    	pickedUpObjects.Clear();
        //deliveredObjects.Add(obj);
    }

    public override void StartingEvents(){

    	//spawn the 4(or how ever many left) objects at 4 random garbage spawner locations
		List<GarbageSpawner> myChosenSpawners =GarbageManager.Instance.garbageSpawners;
		myChosenSpawners.Shuffle();
    	for(int i = 0; i< desiredObject.Count;i++){
  			desiredObject[i].transform.position = myChosenSpawners[i].transform.position;
    		desiredObject[i].gameObject.SetActive(true);
    	}

    	//give current needed info to collected HUD
    	for(int i = 0; i< pickedUpObjects.Count;i++){
    		itemHUD.UpdateItemsCollected(pickedUpObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}
		for(int i = 0; i< deliveredObjects.Count; i++){
    		itemHUD.UpdateItemsCollected(deliveredObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}

    }

    public void PickUpObject(SpecialFriendObject go){ //activated by 'SpecialFriendObject'
    	pickedUpObjects.Add(go);
    	desiredObject.Remove(go);
    	if(!itemHUD.gameObject.activeInHierarchy){
    		itemHUD.gameObject.SetActive(true);
    	}
    	itemHUD.UpdateItemsCollected(go.GetComponent<SpriteRenderer>().sprite);
    }

	public override void FinishDialogEvent(){
		
		slab.GetComponent<ActivateDialogWhenClose>().enabled = true;
		base.FinishDialogEvent();
		gameObject.GetComponent<ActivateDialogWhenClose>().enabled = false; // needed to fix glitch where if player spammed continue button dialog would start again
	}

	public void OpeningSequence(){
		StartCoroutine("OpeningSequenceEvent");
	}


	public IEnumerator OpeningSequenceEvent(){
		mainCam.GetComponent<PostProcessingBehaviour>().profile = null;
		//Rock
		mainCam.GetComponent<Ev_MainCamera>().StartCoroutine("ScreenShake",.5f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(gameObject.transform.position,null);
		yield return new WaitForSeconds(1f);
		eyeBreakPS.SetActive(true);
		Destroy(eyeCover);
		yield return new WaitForSeconds(1f);
		//Slab
		mainCam.GetComponent<Ev_MainCamera>().StartCoroutine("ScreenShake",.5f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(slab.transform.position,null);
		yield return new WaitForSeconds(.3f);
		slab.GetComponent<SlabFriend>().BreakEyes();

		yield return new WaitForSeconds(1f);
		//Stone
		mainCam.GetComponent<Ev_MainCamera>().StartCoroutine("ScreenShake",.5f);
		mainCam.GetComponent<Ev_MainCameraEffects>().CameraPan(stone.transform.position,null);
		yield return new WaitForSeconds(.3f);

		stone.GetComponent<StoneFriend>().BreakEyes();
		yield return new WaitForSeconds(1f);
		for(int i = 0; i < dialogManager.dialogIcons.Count; i++){
			dialogManager.dialogIcons[i].GetComponent<Image>().enabled = true;
		}
		dialogManager.ReturnFromAction();

	}

}
