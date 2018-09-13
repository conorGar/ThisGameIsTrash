using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Ev_WorldSelect : MonoBehaviour {

	public int numberOfStars = 0;
	public float XofStar = 0;
	public int largeTrashListStartIndex = 0;
	public int myMenuSelectStage = 0;
	public int position = 0;
	public int startLargeTrashIndex;
	public GameObject myStars;
	//p = 0 = front; = 1-right; =2-back; = 3 -left
	public int worldNumber;
	public TextMeshProUGUI worldNumberDisplay;
	//public GameObject star;
	public GameObject cloudBurstEffect;
	public GameObject selectEffect;
	public WORLD myWorld;
	public AudioClip selectSFX;
	public SpriteRenderer myUnlockDisplay;
	bool canSelect = false;

	GameObject[] worldIcons;
	int volume = 30;
	//int pulseHieght = 100;
	List<GameObject> starlist = new List<GameObject>();
	bool active = false;
	string layer;
	bool locked = false;

	// Use this for initialization
	void Start () {
		if(!(GlobalVariableManager.Instance.IsWorldUnlocked(myWorld)))
        {
			gameObject.GetComponent<SpriteRenderer>().color = Color.black;
			//if(GlobalVariableManager.Instance.PROGRESS_LV >= myPositionInWorldsUnlocked){
			/*if(myMenuSelectStage == 1){ //just for testing, remove
				StartCoroutine("Unlock");
			}*/
			locked = true;
		}
		//GlobalVariableManager.Instance.MENU_SELECT_STAGE = 0;

		Navigate("right",0);
		StartCoroutine("SelectDelay");
		//Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Navigate(string dir,int arrowPos){
		//Debug.Log(GlobalVariableManager.Instance.MENU_SELECT_STAGE);
		Debug.Log("Navigate Activate");
		//activated by S-Ev_worldSelect. (also sets menuSelectStage before activating this method.

			if(arrowPos == myMenuSelectStage){
				worldNumberDisplay.text = worldNumber.ToString();
				if(locked == false){
					if(myStars != null)
						SpawnStars();
						gameObject.transform.GetChild(0).gameObject.SetActive(true);//activate title

					//-------Activates friend Icons -------------------//
					/*for(int i = 0; i < this.gameObject.transform.childCount; i++){
						gameObject.transform.GetChild(i).gameObject.SetActive(true);
						if(i != 0)//if everything BUT the title child(which has to be 1st child in hieracrchy)
							gameObject.transform.GetChild(i).gameObject.GetComponent<Ev_WorldSelectFriends>().StartCoroutine("ActivateMovement");
					}*/
					//------------------------------------------------//
				}
				//gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,1f,.2f);
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
					//gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.75f,.2f);
				}else{ //if not active
					if(gameObject.transform.localScale.y == .75f){
						//gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.5f,.2f);//if grow doesnt work try coroutine start
					}else{
						//gameObject.GetComponent<SpecialEffectsBehavior>().Grow(.1f,.75f,.2f);
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
		myStars.SetActive(true);
		Debug.Log("Spawn Stars Activate");
		//float tempXStar = XofStar;
		for(int i = startLargeTrashIndex; i < myStars.transform.childCount; i++){
			Debug.Log("StarSpawn");
            LARGEGARBAGE largeGarbageType = LargeGarbage.ByIndex(i);
            Debug.Log(LargeGarbage.ByIndex(i).ToString());
            if ((GlobalVariableManager.Instance.LARGE_GARBAGE_DISCOVERED & largeGarbageType) == largeGarbageType || (GlobalVariableManager.Instance.LARGE_GARBAGE_VIEWED & largeGarbageType) == largeGarbageType) {
            	myStars.transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().color = Color.white;
			}

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
			if(myUnlockDisplay != null){
				myUnlockDisplay.gameObject.SetActive(true);
				myUnlockDisplay.sortingLayerName = layer;
				myUnlockDisplay.sortingOrder = gameObject.GetComponent<SpriteRenderer>().sortingOrder+1;
			}
		
	}

	public void Select(){
		if(locked == false && canSelect){
			/*if(GlobalVariableManager.Instance.WORLD_NUM != myMenuSelectStage +1){
				GlobalVariableManager.Instance.GARBAGE_HAD = 0;
				GlobalVariableManager.Instance.WORLD_NUM = myMenuSelectStage+1;
			}*/
			SoundManager.instance.PlaySingle(selectSFX);
			gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("SelectFlash");
			gameObject.GetComponent<SpecialEffectsBehavior>().SetFadeVariables(.1f,.5f);
			gameObject.GetComponent<SpecialEffectsBehavior>().StartCoroutine("FadeOut");
			Instantiate(selectEffect,transform.position,Quaternion.identity);
			gameObject.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(gameObject.transform.position.x,5.42f,1f);
			GameObject manager = GameObject.Find("manager");
			manager.GetComponent<Ev_FadeHelper>().FadeToScene("1_1");
		}
	}


	IEnumerator ChangeLayer(){
		yield return new WaitForSeconds(.2f);
		gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layer;
	}

	IEnumerator Unlock(){
		GameObject manager = GameObject.Find("manager");
		GameObject camera = GameObject.Find("tk2dCamera");
		manager.GetComponent<S_Ev_WorldSelect>().TriggeredMovement(WorldManager.Instance.world.WorldIndex());
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

	IEnumerator SelectDelay(){
		//added to fix bug where if you select while scene is fading in player gets stuck on screen
		yield return new WaitForSeconds(2f);
		canSelect = true;
	}
}
