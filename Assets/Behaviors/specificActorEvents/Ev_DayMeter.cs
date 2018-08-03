using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ev_DayMeter : MonoBehaviour {

	public GameObject dayIcon;
	public GameObject countdownNumber;
	public GameObject fadeHelper;
	public GameObject tutPopup;


	float delayBonus;
	GameObject dayIconInstance;

	GameObject tempNum;
	float myX;
	float myY;
	bool canGo;
	int triggerOnce;

	void Start () {
		myX = -20f;
		myY = 0f;
		dayIconInstance = Instantiate(dayIcon, new Vector3(transform.position.x - 50f, transform.position.y, transform.position.z),Quaternion.identity);
		dayIconInstance.transform.parent = this.gameObject.transform;

		if(GlobalVariableManager.Instance.characterUpgradeArray[1][31].CompareTo('o') == 0){
			//3rd gathering perk - more time in day
			delayBonus = delayBonus + .1f;
		}
		/*if(GlobalVariableManager.Instance.pinsEquipped[36] == 2){
			//Fuel Efficient Pin
			delayBonus += .1f;
		}
		if(GlobalVariableManager.Instance.pinsEquipped[38] != 0){
			//Hardly Working Pin
			if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 3){
				if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0] == 0)
					delayBonus += .1f;
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 4){
				if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[3] == 0)
					delayBonus += .1f;
			}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED.Count == 5){
				if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[4] == 0)
					delayBonus += .1f;
			}
		}*/ 
		Created();

	}//end of Start()

	void Update () {
		
	}

	public void Stop(){
		CancelInvoke();
		canGo = false;
	}
	public void Created(){
		canGo = true;
		InvokeRepeating("Count",0f,(1f +delayBonus));
	}
	public void UpdatePos(){
		transform.position = new Vector2(myX + GlobalVariableManager.Instance.TIME_IN_DAY, myY - Mathf.Floor(GlobalVariableManager.Instance.TIME_IN_DAY)/4);

	}
	public void Slowdown(){
		//For 'Workin' Hard' Pin
		delayBonus += .1f;
	}
	void Count(){
		if(GlobalVariableManager.Instance.TIME_IN_DAY < 120){
			if(canGo && GlobalVariableManager.Instance.ROOM_NUM != 20){
				//myX += .1f;
				//Debug.Log("Count activated. x position = " + myX);
				//Debug.Log("Time in day: "+GlobalVariableManager.Instance.TIME_IN_DAY);
				dayIconInstance.transform.Translate(1f,.1f,0);
				//gameObject.transform.position.x = myX;
					
				GlobalVariableManager.Instance.TIME_IN_DAY++;
					if(GlobalVariableManager.Instance.TIME_IN_DAY >= 100 && GlobalVariableManager.Instance.TIME_IN_DAY%2 ==0){
						if(GlobalVariableManager.Instance.TIME_IN_DAY < 102){
						//initially spawn the countdown
							if((GlobalVariableManager.Instance.TUT_POPUPS_SHOWN & GlobalVariableManager.TUTORIALPOPUPS.DAYNIGHT) != GlobalVariableManager.TUTORIALPOPUPS.DAYNIGHT){
								tutPopup.SetActive(true);
								tutPopup.GetComponent<GUI_TutPopup>().SetData("DayNight");
							}
							tempNum = Instantiate(countdownNumber,new Vector3(300f,74f,0f),Quaternion.identity);
							tempNum.transform.parent = this.gameObject.transform;
						}else{
							tempNum.GetComponent<Ev_CountdownNumber>().StartCoroutine("Reset");
						}
					}
			}
		}else{
			/*if(GlobalVariableManager.Instance.ROOM_NUM != 2 && GlobalVariableManager.Instance.WORLD_NUM !=1){

			took this out. Nt sure why was here??? 6/2/18

			}*/
			if(triggerOnce == 0){
					Debug.Log("*** END DAY FADE ACTIVATE ***");
					fadeHelper.GetComponent<Ev_FadeHelper>().EndOfDayFade();
					triggerOnce = 1;
				}
		}
		//if(GlobalVariableManager.Instance.TIME_IN_DAY > 2 && GlobalVariableManager.Instance.characterUpgradeArray[1][26].CompareTo('o') == 0 && GlobalVariableManager.Instance.CURRENT_HP < int.Parse(GlobalVariableManager.Instance.characterUpgradeArray[3])){
			//Fitness perk 5 - hp regen
			//if(GlobalVariableManager.Instance.TIME_IN_DAY % 15 ==0){
				//GlobalVariableManager.Instance.CURRENT_HP++;
			//}
		//}
		if(GlobalVariableManager.Instance.MASTER_MUSIC_VOL > 0){
			//adjust volumne
		}
	}//end of Count()

}
