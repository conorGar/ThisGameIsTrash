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
	public ParticleSystem halfWayDonePS;
	public AudioClip halfWayDoneChime;

	float delayBonus;

	float timeToReachTarget = 240f;
	bool canGo;
	int finalCountdownNumber = 10;
	Vector3 startIconPos;
	Vector3 targetPos;
	bool deathSpeedup;
	float t;
	bool halfWayMark;

	void Start () {
		GlobalVariableManager.Instance.TIME_IN_DAY = 0;
		startIconPos = dayIcon.transform.localPosition;
		targetPos = new Vector3(4.13f,.48f,0f);
		dayNumberDisplay.text = "Day: "+ GlobalVariableManager.Instance.DAY_NUMBER;
		Created();

	}//end of Start()

	void Update () {
		if(canGo){
			if(!deathSpeedup)
				t += Time.deltaTime/timeToReachTarget;
			else
				t += (Time.deltaTime/timeToReachTarget)*20;//TODO: this will go past position at 220 if happens at bad time(maybe just find out exact local pos at time = 220 and set it)

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

	public IEnumerator DeathIncrease(){
		/*Time.timeScale = 20;
		yield return new WaitForSeconds(20f);
		Time.timeScale = 1;*/
		Stop();
		deathSpeedup = true;
		canGo = true;
		yield return new WaitForSeconds(1f);
		deathSpeedup = false;
		StartAgain();
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
		if(GlobalVariableManager.Instance.TIME_IN_DAY < 240){
			if(canGo){

					
				GlobalVariableManager.Instance.TIME_IN_DAY++;
				if(GlobalVariableManager.Instance.TIME_IN_DAY >= 220 && !countdownNumber.gameObject.activeInHierarchy){

						countdownNumber.gameObject.SetActive(true);
						InvokeRepeating("FinalCountdown",.1f,2f +(delayBonus*2));
	
					}
			}
		}else{
			

					Debug.Log("*** END DAY FADE ACTIVATE ***");
					fadeHelper.GetComponent<Ev_FadeHelper>().EndOfDayFade();
					this.enabled = false;
				
		}

		if(GlobalVariableManager.Instance.TIME_IN_DAY > (timeToReachTarget/2) && !halfWayMark){
			HalfWay();
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

	void HalfWay(){
		halfWayMark = true;
		halfWayDonePS.Play();
		SoundManager.instance.PlaySingle(halfWayDoneChime);
	}

}
