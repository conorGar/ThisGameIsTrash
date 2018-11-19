using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_SmallTruck : MonoBehaviour {

	public GameObject player;
	//public GameObject backPaper;
	public GameObject oneTimers;
	//public AudioClip truckStart;
	public AudioClip truckDoor;


	int phase = 0;
	int roomNum;
	float delayTillSpawn;
	// Use this for initialization
	bool endDayTruck;
	void Awake(){
		if(player == null)//if not already set
			player = GameObject.FindGameObjectWithTag("Player");
		oneTimers = GameObject.Find("oneTimers");
	}

	void Start () {

		roomNum = GlobalVariableManager.Instance.ROOM_NUM;
		//Debug.Log(player.name);
		if(roomNum == 112){
			//transform.position = new Vector2(-4.79f,8.61f);//relative to camera parent
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(10f,0f);
			delayTillSpawn = 1.8f;
			StartCoroutine("SpawnPlayer");
		}else if(roomNum == 98){
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(-30f,0f);
			delayTillSpawn = .5f;
			StartCoroutine("StopMovement");
		}else if(roomNum == 198){
			//return to hub screen
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(-40f,0f);
			delayTillSpawn = .7f;
			StartCoroutine("SpawnPlayer");
		}else if(roomNum == 111){
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(40f,0f);
			delayTillSpawn = .3f;
			StartCoroutine("StopMovement");
		}
	}


	// Update is called once per frame
	void Update () {
		if(phase == 1){
			if(this.gameObject.transform.position.x > player.transform.position.x){
				gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
				phase = 2;
				if(endDayTruck){
					EndDay();
				}else{
					Debug.Log("PLAYER SPRITE DISABLED HERE");
					player.GetComponent<tk2dSprite>().enabled = false;
					SoundManager.instance.PlaySingle(truckDoor);
                    StartCoroutine("StartMovement");
                    //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
                }
			}
		}
	}

	public IEnumerator DropOff(){
		Debug.Log("Truck Drop Off Activate");
		yield return new WaitForSeconds(.7f);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		Instantiate(player, transform.position, Quaternion.identity);

	

        // TODO: Review and figure this out
		/*if(GlobalVariableManager.Instance.IsPinEquipped(PIN.MOGARBAGEMOPROBLEMS)){
			//Mo' Garbage Mo' Problems
			GlobalVariableManager.Instance.characterUpgradeArray[3] = (Mathf.Round(int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])+ 2)).ToString();
		}*/

		//play world music again
		
		//GlobalVariableManager.Instance.CURRENT_HP = int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3][0].ToString());

		yield return new WaitForSeconds(.5f);
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(60f,0f);
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
	}

	public void EndDay(){
		Debug.Log("Truck End Day Activate");
	
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = false;
		endDayTruck = true;
		if(phase == 0){
			//player = GameObject.FindGameObjectWithTag("Player");
			if(GlobalVariableManager.Instance.WORLD_NUM == 4){
				// Empty evidence spawn list here
				GlobalVariableManager.Instance.STATUE_LIST.Clear();
			}

			if(GlobalVariableManager.Instance.CARRYING_SOMETHING == true){
				/*GameObject[] largeTrash = GameObject.FindGameObjectsWithTag("LargeTrash");
				for(int i = 0; i < largeTrash.Length; i++){
					largeTrash[i].GetComponent<Ev_LargeTrash>().Kill();
				}*/
				GlobalVariableManager.Instance.CARRYING_SOMETHING = false;

			}

			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
			phase = 1;
		}else if(phase == 2){
			//Destroy(player);
			StartCoroutine("StartMovement");
			if(GlobalVariableManager.Instance.DAY_NUMBER == 2){
				//StartCoroutine("HomelessHarry"); TODO: tookout for debuggin other things to do this later
				GameStateManager.Instance.PushState(typeof(EndDayState)); //gets value from ev_fadeHelper
			}else{
                GameStateManager.Instance.PushState(typeof(EndDayState)); //gets value from ev_fadeHelper
            }

            player.SetActive(false);
        }


	}
	public void DetermineMovie(){

	}

	public void ReturnToDumpster(){


		if(phase == 0){
			
			if(GlobalVariableManager.Instance.CARRYING_SOMETHING == true){
				GlobalVariableManager.Instance.CARRYING_SOMETHING = false;
			}

			gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(50f,0f);
			phase = 1;
		}
	}

	public void RespawnEnd(){
		phase = 0;
		gameObject.SetActive(false);
	}

	IEnumerator SpawnPlayer(){
		Debug.Log("Truck - Time Before Spawn");
		yield return new WaitForSeconds(delayTillSpawn);

		//sfx play- truck door
		gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(0f,0f);

		GameObject tempPlayer = Instantiate(player, transform.position, Quaternion.identity);

		yield return new WaitForSeconds(.5f);
		if(roomNum == 198){
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(-20f,0f);
		}else{
			gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(20f,0f);
		}
		//  play world music based on world...(or in hub)

		yield return new WaitForSeconds(1f);
		if(roomNum == 112){//day display screen
			GameObject manager = GameObject.Find("Manager");
			manager.GetComponent<Ev_FadeHelper>().JumpToScene("1_1");
		}
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		Destroy(gameObject);
	}

	IEnumerator StopMovement(float stopTime){
		yield return new WaitForSeconds(stopTime);
		gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(0f,0f);

	}

	IEnumerator StartMovement(){
		Debug.Log("STart Movement coroutine started ");
		yield return new WaitForSeconds(.6f);
		gameObject.GetComponent<Rigidbody2D>().velocity= new Vector2(50f,0f);

	}

	IEnumerator HomelessHarry(){
		yield return new WaitForSeconds(1f);
		//shake screen
		/*
		//Enable Harry dialog
		oneTimers.GetComponent<ActivateDialogWhenClose>().dialogName = "Harry1";
		oneTimers.GetComponent<ActivateDialogWhenClose>().ActivateDialog();*/
	}
}
