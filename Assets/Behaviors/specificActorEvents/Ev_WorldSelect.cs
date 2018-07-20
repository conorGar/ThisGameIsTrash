using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_WorldSelect : MonoBehaviour {

	public int numberOfStars = 0;
	public float XofStar = 0;
	public int largeTrashListStartIndex = 0;
	public int myMenuSelectStage = 0;
	public GlobalVariableManager.WORLDS worldPosition = GlobalVariableManager.WORLDS.ONE;
	public int position = 0;
	//p = 0 = front; = 1-right; =2-back; = 3 -left


	public GameObject star;
	public GameObject cloudBurstEffect;
	public GameObject selectEffect;

	GameObject[] worldIcons;
	int volume = 30;
	//int pulseHieght = 100;
	List<GameObject> starlist = new List<GameObject>();
	bool active = false;
	string layer;
	bool locked = false;

	// Use this for initialization
	void Start () {
		if((GlobalVariableManager.Instance.WORLDS_UNLOCKED & worldPosition) != worldPosition){
			gameObject.GetComponent<SpriteRenderer>().color = Color.black;
			//if(GlobalVariableManager.Instance.PROGRESS_LV >= myPositionInWorldsUnlocked){
			if(myMenuSelectStage == 1){ //just for testing, remove
				StartCoroutine("Unlock");
			}
			locked = true;
		}
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = 0;

		Navigate("right");
		
		Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Navigate(string dir){
		Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
		Debug.Log("Navigate Activate");
		//activated by S-Ev_worldSelect. (also sets menuSelectStage before activating this method.

			if(GlobalVariableManager.Instance.MENU_SELECT_STAGE == myMenuSelectStage){
				if(locked == false){
					SpawnStars();
					//-------Activates friend Icons -------------------//
					for(int i = 0; i < this.gameObject.transform.childCount; i++){
						gameObject.transform.GetChild(i).gameObject.SetActive(true);
						if(i != 0)//if everything BUT the title child(which has to be 1st child in hieracrchy)
							gameObject.transform.GetChild(i).gameObject.GetComponent<Ev_WorldSelectFriends>().StartCoroutine("ActivateMovement");
					}
					//------------------------------------------------//
				}
				gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,1f,.2f);
				active = true;
			}else{
				if(active){
					for(int i = 0; i<starlist.Count; i++){
						Destroy(starlist[i]);
						Debug.Log("Star Destroyed");
					}
					starlist.Clear();
					//-------Deactivates friend Icons -------------------//
					for(int i = 0; i < this.gameObject.transform.childCount; i++){
						gameObject.transform.GetChild(i).gameObject.SetActive(false);
					}
					//------------------------------------------------/
					gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.75f,.2f);
				}else{ //if not active
					if(gameObject.transform.localScale.y == .75f){
						gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.5f,.2f);//if grow doesnt work try coroutine start
					}else{
						gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.75f,.2f);
					}
				}
			}
			if(dir == "right"){
				if(position < 3){
					position++;
				}else{
					position = 0;
				}
			}else if(dir == "left"){
				if(position > 0){
					position--;
				}else{
					position = 3;
				}
			}
			Move();
		

	}//end of Navigate()

	void SpawnStars(){
		Debug.Log("Spawn Stars Activate");
		float tempXStar = XofStar;
		for(int i = 0; i < numberOfStars; i++){
		    GameObject tempStar = Instantiate(star, new Vector2(tempXStar,5f), Quaternion.identity);

            LARGEGARBAGE largeGarbageType = LargeGarbage.ByIndex(i);
            
            // Black out any stars for large trash that hasn't been both discovered and viewed.
            if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & largeGarbageType) != largeGarbageType && (GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED & largeGarbageType) != largeGarbageType) {
				tempStar.GetComponent<SpriteRenderer>().color = Color.black;
			}
				starlist.Add(tempStar);
				tempXStar +=1;
				Debug.Log("Star Spawned");
			}

	}//End of SpawnStars()

	void Move(){
		//middle x = 3.4
		//left side x = - 3.76
		//right side x = 10.35

			if(position == 0){
				gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(3.4f,0f,0.2f);
				layer = "Layer04";
				StartCoroutine("ChangeLayer");
			}else if(position == 1){
				gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(10.35f,0f,0.2f);
				layer = "Layer03";
				StartCoroutine("ChangeLayer");
			}else if(position == 2){
				gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(2.4f,1f,0.2f);
				layer = "Layer02";
				StartCoroutine("ChangeLayer");
			}else{
				gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-4.76f,0f,0.2f);
				layer = "Layer03";
				StartCoroutine("ChangeLayer");
			}
		
	}

	public void Select(){
		if(locked == false){
			if(GlobalVariableManager.Instance.WORLD_NUM != myMenuSelectStage +1){
				GlobalVariableManager.Instance.ROOM_PLAYER_DIED_IN = 99;
				GlobalVariableManager.Instance.GARBAGE_HAD = 0;
				GlobalVariableManager.Instance.WORLD_NUM = myMenuSelectStage+1;
			}
			gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("SelectFlash");
			gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f,.5f);
			gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("FadeOut");
			Instantiate(selectEffect,transform.position,Quaternion.identity);
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(gameObject.transform.position.x,5.42f,1f);
			GameObject manager = GameObject.Find("manager");
			manager.GetComponent<Ev_FadeHelper>().FadeToScene("BagSelectScreen");
		}
	}


	IEnumerator ChangeLayer(){
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layer;
	}

	IEnumerator Unlock(){
		GameObject manager = GameObject.Find("manager");
		GameObject camera = GameObject.Find("tk2dCamera");
		manager.GetComponent<S_Ev_WorldSelect>().TriggeredMovement(GlobalVariableManager.Instance.WorldIndex(worldPosition));
		camera.GetComponent<Ev_MainCamera>().StartCoroutine("ScreenShake",2f);
		yield return new WaitForSeconds(2.5f);
		manager.GetComponent<Ev_FadeHelper>().WhiteFlash();
		Instantiate(cloudBurstEffect,transform.position,Quaternion.identity);
		gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		for(int i = 0; i < gameObject.transform.childCount;i++){
			gameObject.transform.GetChild(i).gameObject.SetActive(true);
		}
		manager.GetComponent<S_Ev_WorldSelect>().SetNavigate(true);
		GlobalVariableManager.Instance.MENU_SELECT_STAGE = myMenuSelectStage;
		Debug.Log("menu select stage:" +GlobalVariableManager.Instance.MENU_SELECT_STAGE);


	}
}
