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
    List<SpecialFriendObject> pickedUpObjects = new List<SpecialFriendObject>();

    public GameObject eyeBreakPS;
    public GameObject eyeCover;
    public GameObject slab;
    public GameObject stone;
    public Sprite beautifulRock;


	public GameObject moon;
	public GameObject moonShadow;
    public GameObject blockade;


    bool moonInProperLocation;
    int departureSequence = 0;
    int endPhase = 0;

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
				gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 5;

                BreakEyes();
                break;
            case "END":
				blockade.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }

    private void Update()
    {
		switch (GetFriendState()) {
            case "START":
                break;
            case "WANTS_TO_BE_PRETTY":
                nextDialog = "RockRequest";
                GetComponent<ActivateDialogWhenClose>().Execute();
                break;
            case "END":
                break;
        }

        OnUpdate();
    }

    public override void OnUpdate()
    {
        switch (GetFriendState()) {
            case "START":
                nextDialog = "Start";
                GetComponent<ActivateDialogWhenClose>().Execute("Nobody");
                break;
            case "WANTS_TO_BE_PRETTY":
                break;
            case "END":
                break;
        }

		if(moon.activeInHierarchy && !moonInProperLocation){
        	moon.transform.position = Vector2.MoveTowards(moon.transform.position, new Vector2(31,50), (3*Time.deltaTime));
			moonShadow.transform.localPosition = Vector2.MoveTowards(moonShadow.transform.localPosition, new Vector2(0,-4.5f), (1*Time.deltaTime));
        	if(Vector2.Distance(moon.transform.position,new Vector2(31,50)) <5){
        		moonInProperLocation = true;
        	}
        }

        if(departureSequence == 1){
			moon.transform.position = Vector2.MoveTowards(moon.transform.position, this.gameObject.transform.position, (5*Time.deltaTime));

        }else if(departureSequence == 2){ //moon and rock leave
			moon.transform.position = Vector2.MoveTowards(moon.transform.position, new Vector2(54,112), (5*Time.deltaTime));

        }
    }

    public override IEnumerator OnFinishDialogEnumerator(bool panToPlayer = true)
    {
        yield return new WaitForSeconds(.3f);

        switch (GetFriendState()) {
            case "START":
                // Now that we've met the rocks they each want something different.
                // Slab wants to be rich with trash.
                slab.GetComponent<SlabFriend>().SetFriendState("WANTS_TRASH");
                //slab.GetComponent<ActivateDialogWhenClose>().autoStart = true;

                // Stone wants hands to build the rocket.
                stone.GetComponent<StoneFriend>().SetFriendState("WANTS_HANDS");
				stone.GetComponent<StoneFriend>().nextDialog = "StoneRequest";
                stone.GetComponent<StoneFriend>().stoneHand.SetActive(true);
                stone.GetComponent<StoneFriend>().secondStoneHand.SetActive(true);
                //stone.GetComponent<ActivateDialogWhenClose>().autoStart = true;

                // Rock wants to be pretty.
                SetFriendState("WANTS_TO_BE_PRETTY");
                gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				gameObject.GetComponent<ActivateDialogWhenClose>().distanceThreshold = 5;

				StartingEvents();
				nextDialog = "RockRequest";
                //GetComponent<ActivateDialogWhenClose>().autoStart = true;

                break;
            case "WANTS_TO_BE_PRETTY":
				gameObject.GetComponent<ActivateDialogWhenClose>().autoStart = false;
				gameObject.GetComponent<ActivateDialogWhenClose>().canTalkTo = true;
				StartCoroutine("TotalProductsDisplay");
                break;
            case "END":
            	CamManager.Instance.mainCamEffects.CameraPan(slab.transform.position,"");
            	yield return new WaitForSeconds(.5f);
            	slab.GetComponent<SlabFriend>().Sleep();
				yield return new WaitForSeconds(.5f);
            	CamManager.Instance.mainCamEffects.CameraPan(stone.transform.position,"");
            	yield return new WaitForSeconds(.5f);
            	stone.GetComponent<StoneFriend>().Sleep();
            	yield return new WaitForSeconds(.5f);
				CamManager.Instance.mainCamEffects.CameraPan(blockade.transform.position,"");
				yield return new WaitForSeconds(.5f);
				blockade.SetActive(false);
				ObjectPool.Instance.GetPooledObject("effect_SmokePuff",blockade.transform.position);
				yield return new WaitForSeconds(.5f);
				yield return base.OnFinishDialogEnumerator();
				moon.SetActive(false);
			break;

        }

        yield return base.OnFinishDialogEnumerator();

    }

    public void GotItemsCheck(){
    	
    	Debug.Log(pickedUpObjects.Count + "<----------- Rock's delivered items count");
    	if(pickedUpObjects.Count >= 4){  
    		dialogManager.JumpToNewNode("RockComplete1");
    	}
    	dialogManager.ReturnFromAction();
    }

  /*  public void DeliverObject(SpecialFriendObject obj)
    {
    	for(int i = 0; i < pickedUpObjects.Count;i++){
    		deliveredObjects.Add(pickedUpObjects[i]);
    	}
    	pickedUpObjects.Clear();
        //deliveredObjects.Add(obj);
    }*/

    public override void StartingEvents() {

        //spawn the 4(or how ever many left) objects at 4 random garbage spawner locations
        var myChosenSpawners = GarbageManager.Instance.garbageSpawners;
        myChosenSpawners.Shuffle();

        // Get a queue of desiredObject indices.
        Queue<int> objectIndices = new Queue<int>();
        for (int i = 0; i < desiredObject.Count; i++) {
            objectIndices.Enqueue(i);
        }

        if (objectIndices.Count > 0) {
            for (int i = 0; i < myChosenSpawners.Count; i++) {
                // Spawn desiredObjects, skipping used spawners.
                if (myChosenSpawners[i].spawnedGarbage == null) {
                    var index = objectIndices.Dequeue();
                    desiredObject[index].transform.position = myChosenSpawners[i].transform.position;
                    desiredObject[index].gameObject.SetActive(true);
                    if (objectIndices.Count <= 0)
                        break;
                }
            }
        }

        // If we couldn't find enough empty spawners, replace some trash with the remaining desired objects.
        if (objectIndices.Count > 0) {
            for (int i = 0; i < myChosenSpawners.Count; i++) {
                if (myChosenSpawners[i].spawnedGarbage != null) {
                    myChosenSpawners[i].spawnedGarbage.gameObject.SetActive(false);

                    var index = objectIndices.Dequeue();
                    desiredObject[index].transform.position = myChosenSpawners[i].transform.position;
                    desiredObject[index].gameObject.SetActive(true);

                    myChosenSpawners[i].spawnedGarbage = null;

                    if (objectIndices.Count > 0)
                        break;
                }
            }
        }
    }

    public void PickUpObject(SpecialFriendObject go){ //activated by 'SpecialFriendObject'
    	pickedUpObjects.Add(go);
    	desiredObject.Remove(go);
		GUIManager.Instance.rockItemHUD.UpdateItemsCollected(go.GetComponent<SpriteRenderer>().sprite);
    	StartCoroutine("TotalProductsDisplay");
      
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

		var multiDialog = (MultipleDialogIconsManager)DialogManager.Instance.currentlySpeakingIcon;
        for (int i = 0; i <multiDialog.icons.Count;i++){
			multiDialog.icons[i].GetComponent<Image>().enabled = true;
		}

		dialogManager.ReturnFromAction();

	}
	IEnumerator TotalProductsDisplay(){
    	GUIManager.Instance.rockItemHUD.gameObject.SetActive(true);

		yield return new WaitForSeconds(2f);
		GUIManager.Instance.rockItemHUD.gameObject.SetActive(false);
    }
    public void BreakEyes()
    {
        eyeCover.SetActive(false);
        eyeBreakPS.SetActive(true);
    }
	public void Sleep(){
    	eyeCover.SetActive(true);
    	SetFriendState("END");
    }
    public void RockDressUp(){
    	gameObject.GetComponent<SpriteRenderer>().sprite = beautifulRock;
    	StartCoroutine(DressUpSequence());
    }
	public void MoonArrive(){
    	StartCoroutine(MoonArriveSequence());
    }

    IEnumerator MoonArriveSequence(){
    	if(endPhase == 0){
			CamManager.Instance.mainCam.ScreenShake(3f);
    		yield return new WaitForSeconds(1f);
    		endPhase = 1;
			dialogManager.ReturnFromAction();
    	}else{
    		CamManager.Instance.mainCamEffects.ZoomInOut(.9f,1);
			CamManager.Instance.mainCamPostProcessor.profile = null;
	    	moon.SetActive(true);
	    	yield return new WaitUntil(() => moonInProperLocation);
	    	yield return new WaitForSeconds(.5f);
	    	dialogManager.ReturnFromAction();
    	}
    }

    IEnumerator DressUpSequence(){
		CamManager.Instance.mainCamPostProcessor.profile = null;
		yield return new WaitForSeconds(2f);
		dialogManager.ReturnFromAction();
    }

	public void RockEnding(){
		if(endPhase == 0){
			StartCoroutine(MoonArriveSequence());
		}else if(endPhase == 1){
			StartCoroutine(DepartureSequence());
		}
    }

    public IEnumerator DepartureSequence(){
    	Debug.Log("Rock departure sequence activated");
    	departureSequence = 1;
		

    	yield return new WaitUntil(() => moon.transform.position.x >= transform.position.x);
    	Debug.Log("Rock departure reached this part");
    	yield return new WaitForSeconds(.4f);
    	departureSequence = 2;
    	transform.parent = moon.transform;
    	yield return new WaitForSeconds(2.5f);
    	departureSequence=3;
		CamManager.Instance.mainCamPostProcessor.profile = null;
		SetFriendState("END");
    	dialogManager.ReturnFromAction();
    }
	
    public override void OnWorldStart(World world){ //populates the world with beauty items
		switch (GetFriendState()) {
            
            case "WANTS_TO_BE_PRETTY":
                // Eyes open.
				StartingEvents();
				for(int i = 0; i < pickedUpObjects.Count; i++){ // update display with previously gathered items
					GUIManager.Instance.rockItemHUD.UpdateItemsCollected(pickedUpObjects[i].GetComponent<SpriteRenderer>().sprite);
				}
				for(int i = 0; i < desiredObject.Count; i++){ // enabled again after 'OnWorldEnd' Disables
					desiredObject[i].gameObject.SetActive(true);
				}
                break;
           	case "END":
	           	blockade.SetActive(false);
	           	break;
        }
    }
    public override void OnWorldEnd(){
		switch (GetFriendState()) {
            
            case "WANTS_TO_BE_PRETTY":
               
				for(int i = 0; i < desiredObject.Count; i++){
					desiredObject[i].gameObject.SetActive(false);
				}
                break;
           	
        }
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

        string savedObjString = "";
        for(int i = 0; i < pickedUpObjects.Count; i++){
        	savedObjString = savedObjString + pickedUpObjects[i].name + ",";
        }
        Debug.Log("***SavedObjString for Rock:" + savedObjString);

        json_data["pickedUpObjects"] = savedObjString;


        return json_data;
    }

    public override void Load(SimpleJSON.JSONObject json_data)
    {
        friendState = json_data["friendState"].AsInt;


        string pickedUpObjs = json_data["pickedUpObjects"];
        Debug.Log("******loaded rock obj string:********" + pickedUpObjs);
        if (pickedUpObjs != null) {
            string[] pickedUpArray = pickedUpObjs.Split(',');
            for (int i = 0; i < pickedUpArray.Length; i++) {
                for (int j = 0; j < desiredObject.Count; j++) {
                    if (desiredObject[j].name == pickedUpArray[i]) {
                        pickedUpObjects.Add(desiredObject[j]);
                        desiredObject.RemoveAt(j);
                        break;
                    }
                }
            }
        }
    }
}
