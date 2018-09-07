using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using I2.TextAnimation;

public class Ev_DayMeter : MonoBehaviour {

	public GameObject dayIcon;
	public TextMeshProUGUI countdownNumber;
	public GameObject fadeHelper;
	public GameObject tutPopup;
	public AudioClip countdownTick;
	public TextMeshProUGUI dayNumberDisplay;

	float delayBonus;

	float timeToReachTarget =120f;
	bool canGo;
	int finalCountdownNumber = 10;
	Vector3 startIconPos;
	Vector3 targetPos;
	float t;

	void Start () {
		GlobalVariableManager.Instance.TIME_IN_DAY = 0;
		startIconPos = dayIcon.transform.localPosition;
		targetPos = new Vector3(4.13f,.48f,0f);
		dayNumberDisplay.text = "Day: "+ GlobalVariableManager.Instance.DAY_NUMBER;
		Created();

	}//end of Start()

	void Update () {
		if(canGo){
			t += Time.deltaTime/timeToReachTarget;
			dayIcon.transform.localPosition = Vector3.Lerp(startIconPos,targetPos,t);
		}
	}

	public void Stop(){
		CancelInvoke();
		canGo = false;
	}
	public void StartAgain(){
		Created();
	}
	public void Created(){
		canGo = true;
		InvokeRepeating("Count",0f,(1f +delayBonus));
	}

	public void Slowdown(){
		//For 'Workin' Hard' Pin
		delayBonus += .1f;
	}
	void Count(){
		if(GlobalVariableManager.Instance.TIME_IN_DAY < 120){
			if(canGo){

					
				GlobalVariableManager.Instance.TIME_IN_DAY++;
				if(GlobalVariableManager.Instance.TIME_IN_DAY >= 100 && !countdownNumber.gameObject.activeInHierarchy){

						countdownNumber.gameObject.SetActive(true);
						InvokeRepeating("FinalCountdown",.1f,2f +(delayBonus*2));
	
					}
			}
		}else{
			

					Debug.Log("*** END DAY FADE ACTIVATE ***");
					fadeHelper.GetComponent<Ev_FadeHelper>().EndOfDayFade();
					this.enabled = false;
				
		}

		if(GlobalVariableManager.Instance.MASTER_MUSIC_VOL > 0){
			//adjust volumne
		}
	}//end of Count()

	void FinalCountdown(){
		if(finalCountdownNumber > 0){
		SoundManager.instance.RandomizeSfx(countdownTick,.7f,1.3f);
		Debug.Log("Final COuntdown:" + finalCountdownNumber);
		countdownNumber.text = finalCountdownNumber.ToString();
		countdownNumber.color = new Color(152f,152f,152f,.64f);
		countdownNumber.gameObject.GetComponent<TextAnimation>().PlayAnim(0);
		finalCountdownNumber--;
		}
	}

}
