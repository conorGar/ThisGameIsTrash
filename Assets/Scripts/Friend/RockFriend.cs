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

    public GameObject eyeBreakPS;
    public GameObject eyeCover;
    public GameObject slab;
    public GameObject stone;


    public override void GenerateEventData()
    {
        // These guys show up every day.
        day = CalendarManager.Instance.currentDay;
    }
    public new void OnEnable(){
        switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TO_BE_PRETTY":
                // Eyes open.
                BreakEyes();
                break;
            case "END":
                break;
        }
    }

    private void Update()
    {
        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                nextDialog = "Start";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "WANTS_TO_BE_PRETTY":
                break;
            case "END":
                break;
        }
    }

    public override IEnumerator OnFinishDialogEnumerator()
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
                // Now that we've met the rocks they each want something different.
                // Slab wants to be rich with trash.
                slab.GetComponent<SlabFriend>().SetFriendState("WANTS_TRASH");
                slab.GetComponent<ActivateDialogWhenClose>().autoStart = true;

                // Stone wants hands to build the rocket.
                stone.GetComponent<StoneFriend>().SetFriendState("WANTS_HANDS");
                stone.GetComponent<ActivateDialogWhenClose>().autoStart = true;

                // Rock wants to be pretty.
                SetFriendState("WANTS_TO_BE_PRETTY");
                GetComponent<ActivateDialogWhenClose>().autoStart = true;

                break;
            case "WANTS_TO_BE_PRETTY":
                break;
        }
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
    		GUIManager.Instance.rockItemHUD.UpdateItemsCollected(pickedUpObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}
		for(int i = 0; i< deliveredObjects.Count; i++){
            GUIManager.Instance.rockItemHUD.UpdateItemsCollected(deliveredObjects[i].gameObject.GetComponent<SpriteRenderer>().sprite);
    	}

    }

    public void PickUpObject(SpecialFriendObject go){ //activated by 'SpecialFriendObject'
    	pickedUpObjects.Add(go);
    	desiredObject.Remove(go);
    	if(!GUIManager.Instance.rockItemHUD.gameObject.activeInHierarchy){
            GUIManager.Instance.rockItemHUD.gameObject.SetActive(true);
    	}
        GUIManager.Instance.rockItemHUD.UpdateItemsCollected(go.GetComponent<SpriteRenderer>().sprite);
    }

	public void OpeningSequence(){
		StartCoroutine("OpeningSequenceEvent");
	}

    public IEnumerator OpeningSequenceEvent(){
        CamManager.Instance.mainCamPostProcessor.profile = null;
        //Rock
        CamManager.Instance.mainCam.ScreenShake(.5f);
        CamManager.Instance.mainCamEffects.CameraPan(gameObject.transform.position,null);
		yield return new WaitForSeconds(1f);
        BreakEyes();
		yield return new WaitForSeconds(1f);
        //Slab
        CamManager.Instance.mainCam.ScreenShake(.5f);
        CamManager.Instance.mainCamEffects.CameraPan(slab.transform.position,null);
		yield return new WaitForSeconds(.3f);
		slab.GetComponent<SlabFriend>().BreakEyes();

		yield return new WaitForSeconds(1f);
        //Stone
        CamManager.Instance.mainCam.ScreenShake(.5f);
        CamManager.Instance.mainCamEffects.CameraPan(stone.transform.position,null);
		yield return new WaitForSeconds(.3f);

		stone.GetComponent<StoneFriend>().BreakEyes();
		yield return new WaitForSeconds(1f);
		for(int i = 0; i < dialogManager.dialogIcons.Count; i++){
			dialogManager.dialogIcons[i].GetComponent<Image>().enabled = true;
		}
		dialogManager.ReturnFromAction();

	}

    public void BreakEyes()
    {
        Destroy(eyeCover);
        eyeBreakPS.SetActive(true);
    }

    // User Data implementation
    public override string UserDataKey()
    {
        return "Rock";
    }

    public override SimpleJSON.JSONObject Save()
    {
        var json_data = new SimpleJSON.JSONObject();

        json_data["friendState"] = friendState;

        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;
    }
}
