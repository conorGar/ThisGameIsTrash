using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_DeathDisplay : MonoBehaviour
{

	public GUI_TrashCollectedDisplay currentTCD;
	public GUI_TrashCollectedDisplay myTCD;
	public Ev_DayMeter dayMeter;
	public Ev_DayMeter myDayMeter;
	public GameObject healthDisplay;
	public GameObject fader;
	public GameObject meleeHUD;
	public AudioClip dayIncrease;

	public void DayMeterRise(){
		/*Vector2 offsetMin; 
		Vector2 offsetMax;
		offsetMin = dayMeter.GetComponent<RectTransform>().offsetMin;
		offsetMax = dayMeter.GetComponent<RectTransform>().offsetMax;
		dayMeter.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(140f,51f);
		//dayMeter.gameObject.GetComponent<RectTransform>().offsetMax = offsetMax;
		//dayMeter.gameObject.GetComponent<RectTransform>().offsetMin = offsetMin;
		*/
		Debug.Log("DayMeterRise() Activate--------");
		myTCD.gameObject.SetActive(false);
		myDayMeter.gameObject.SetActive(true);
		if(GlobalVariableManager.Instance.TIME_IN_DAY + 20 <220){
			GlobalVariableManager.Instance.TIME_IN_DAY +=20;
		}else{
			GlobalVariableManager.Instance.TIME_IN_DAY = 220;
		}
		myDayMeter.dayIcon.transform.localPosition = dayMeter.dayIcon.transform.localPosition;
		SoundManager.instance.PlaySingle(dayIncrease);
		myDayMeter.StartCoroutine("DeathIncrease");
		dayMeter.StartCoroutine("DeathIncrease");
	}

	public void DepleteTrashCollected(){
		/*Vector2 offsetMin; 
		Vector2 offsetMax;
		offsetMin = currentTCD.GetComponent<RectTransform>().offsetMin;
		offsetMax = currentTCD.GetComponent<RectTransform>().offsetMax;
		currentTCD.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(140f,51f);
		currentTCD.gameObject.GetComponent<RectTransform>().offsetMax = offsetMax;
		currentTCD.gameObject.GetComponent<RectTransform>().offsetMin = offsetMin;
		*/
		//myTCD = currentTCD;
		myTCD.trashDropped = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0];
		myTCD.gameObject.SetActive(true);
		myTCD.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
		myTCD.InvokeRepeating("Deplete",.1f,.1f);
	}

	public void ReturnHUD(){
		fader.GetComponent<Animator>().speed = 1;
		fader.GetComponent<Animator>().Play("Fade");
		meleeHUD.SetActive(true);
		currentTCD.gameObject.SetActive(true);
		dayMeter.gameObject.SetActive(true);
		healthDisplay.SetActive(true);
	}

	public void HideHUD(){
		dayMeter.gameObject.SetActive(false);
		currentTCD.gameObject.SetActive(false);
		healthDisplay.SetActive(false);
		meleeHUD.SetActive(false);
	}

	public void DeathFade(){
		//fader.GetComponent<Animator>().speed = -1;
		fader.SetActive(true);
		fader.GetComponent<Animator>().Play("FadeOut");
		//fader.GetComponent<Animator>().Play("Fade");
		//fader.GetComponent<Animator>().speed = 1;

	}
}

