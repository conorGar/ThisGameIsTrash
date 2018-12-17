using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using I2.TextAnimation;

public class GUI_TrashCollectedDisplay : MonoBehaviour {
	public TextMeshProUGUI trashCollected;
	public TextMeshProUGUI maxTrashDisplay;
	public GameObject newDiscoveryDisplay;
	public int trashDropped;
	public GameObject deathDisplay;
	int phase;
	// Use this for initialization
	void Start () {
		//TODO: adjust for different bag types
		maxTrashDisplay.text = "/" + GlobalVariableManager.Instance.BAG_SIZE_STAT.GetMax();
        GlobalVariableManager.Instance.BAG_SIZE_STAT.ResetCurrent();
        trashCollected.text = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0].ToString();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void UpdateDisplay(int trashNum){
		Debug.Log("UpdateDisplay Happened properly");
		trashCollected.text = trashNum.ToString();
		trashCollected.GetComponent<TextAnimation>().PlayAnimation(0);
	}
	public void NewDiscoveryShow(string trashSprite, string trashname){
		newDiscoveryDisplay.SetActive(true);
		//newDiscoveryDisplay.GetComponent<GUIEffects>().Start();
		newDiscoveryDisplay.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = trashname;
		newDiscoveryDisplay.transform.GetChild(1).GetComponent<tk2dSprite>().SetSprite(trashSprite);
		StopCoroutine("NewDiscoveryBehavior");//makes sure no in middle of another coroutine if just appeared...
		StartCoroutine("NewDiscoveryBehavior");
	}

	IEnumerator NewDiscoveryBehavior(){

		
		yield return new WaitForSeconds(1f);

		newDiscoveryDisplay.GetComponent<SpecialEffectsBehavior>().SmoothMovementToPoint(-133f,-113f,.4f,true);
		yield return new WaitForSeconds(3f);
		newDiscoveryDisplay.SetActive(false);
	}

	public void Deplete(){ //invoked repeatedly by GUI_DeathDisplay

		if(trashDropped > 0){
			Debug.Log("Trash deplete activate ***********");
			trashDropped--;
			trashCollected.text = trashDropped.ToString();

		}else{
			deathDisplay.GetComponent<GUI_DeathDisplay>().CancelInvoke();
		}
	}
}
