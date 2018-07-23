using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_SmallTruck : MonoBehaviour {

	public GameObject player;
	public GameObject resultsDisplay;
	public GameObject backPaper;


	int phase = 0;
	int roomNum;
	float delayTillSpawn;
	// Use this for initialization
	void Awake(){
		if(player == null)//if not already set
			player = GameObject.FindGameObjectWithTag("Player");

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
				EndDay();
			}
		}
	}

	public IEnumerator DropOff(){
		Debug.Log("Truck Drop Off Activate");
		yield return new WaitForSeconds(.7f);
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true;
		gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f,0f);
		Instantiate(player, transform.position, Quaternion.identity);

		if(GlobalVariableManager.Instance.pinsEquipped[37] != 0){
			//Death's Deal
			GlobalVariableManager.Instance.characterUpgradeArray[3] = (Mathf.Round((int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])-1))).ToString();
		}

		if(GlobalVariableManager.Instance.pinsEquipped[10] != 0){
			//Mo' Garbage Mo' Problems
			GlobalVariableManager.Instance.characterUpgradeArray[3] = (Mathf.Round(int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])+ 2)).ToString();
		}

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
			Destroy(player);
			StartCoroutine("StartMovement");
			GameObject tempPaper = Instantiate(backPaper,transform.position,Quaternion.identity);
			//GameObject tempResults = Instantiate(resultsDisplay,transform.position,Quaternion.identity);
			//tempResults.transform.parent = tempPaper.transform.parent;
			//^ for now the prefab had results already attatched as child
		}


	}
	public void DetermineMovie(){

	}

	public void ReturnToDumpster(){

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
}
