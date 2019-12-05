using UnityEngine;
using System.Collections;

public class IceCreamTownManager : MonoBehaviour
{

	//Keep track of the day and swap with new ice cream folk every 3 days.

	//holds a bunch of possible ice cream folk prefabs that it picks from randomly?
	//^ Try to see if can change ice cream colors progamatically, then just sort through dialog options instead


	//Switch up their dialog every day, certain dialog is only assigned on first or final day.

	public GameObject[] iceCreamFolk;
	public GameObject[] spawnPositions = new GameObject[3];
	public int totalDialogPossibilities;

	// Use this for initialization
	void Start ()
	{
		if(GlobalVariableManager.Instance.ICECREAM_COUNTDOWN_START_DAY == 99){
			GlobalVariableManager.Instance.ICECREAM_COUNTDOWN_START_DAY = GlobalVariableManager.Instance.DAY_NUMBER;
		}else{
			if((GlobalVariableManager.Instance.DAY_NUMBER - GlobalVariableManager.Instance.ICECREAM_COUNTDOWN_START_DAY)%3 == 0){
				SpawnNewIceCream();
			}else{
				SpawnPreviousIceCream();
			}
		}

		HandleDialog();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


	void HandleDialog(){
		//gives the ice cream something new to say each day, certain dialogs also only show up on final days
		if((GlobalVariableManager.Instance.DAY_NUMBER - GlobalVariableManager.Instance.ICECREAM_COUNTDOWN_START_DAY)%3 == 2){ //Final Day Dialogs
			foreach(GameObject iceCreamGuy in iceCreamFolk){
				int randomDialog = Random.Range(1,totalDialogPossibilities);
				iceCreamGuy.GetComponent<NPCFriend>().nextDialog = "ic_townfolk_fd" + randomDialog; //fd = 'final day'
			}
		}else{
			foreach(GameObject iceCreamGuy in iceCreamFolk){
				int randomDialog = Random.Range(1,totalDialogPossibilities);
				iceCreamGuy.GetComponent<NPCFriend>().nextDialog = "ic_townfolk" + randomDialog;
			}
		}
	}



	void SpawnPreviousIceCream(){
		//check an array of integers, saved in GlobalVarManager, that represents the position of which ice cream to spawn within the 
		//iceCreamFolk[] array.
		for(int i = 0; i < spawnPositions.Length; i++){
			Instantiate(iceCreamFolk[GlobalVariableManager.Instance.CURRENT_ICE_CREAM[i]],spawnPositions[i].transform.position, Quaternion.identity);

		}
	}

	void SpawnNewIceCream(){

		for(int i = 0; i < spawnPositions.Length; i++){
			int randomGuy = Random.Range(0,iceCreamFolk.Length);
			Instantiate(iceCreamFolk[randomGuy],spawnPositions[i].transform.position, Quaternion.identity);
			GlobalVariableManager.Instance.CURRENT_ICE_CREAM[i] = randomGuy;
		}


	}

	public void DebugDayChange(){// Activated by DayDebugManager
		Start();
	}
}

