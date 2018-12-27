using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_DeathDisplay : MonoBehaviour
{
	public Ev_DayMeter dayMeter;
	public Ev_DayMeter myDayMeter;
	public GameObject healthDisplay;
	public GameObject fader;
	public GameObject meleeHUD;
	public AudioClip dayIncrease;
	public AudioClip truckSfx;

	public void DayMeterRise(){
		/*Vector2 offsetMin; 
		Vector2 offsetMax;
		offsetMin = dayMeter.GetComponent<RectTransform>().offsetMin;
		offsetMax = dayMeter.GetComponent<RectTransform>().offsetMax;
		dayMeter.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(140f,51f);
		//dayMeter.gameObject.GetComponent<RectTransform>().offsetMax = offsetMax;
		//dayMeter.gameObject.GetComponent<RectTransform>().offsetMin = offsetMin;
		*/
        GUIManager.Instance.TrashCollectedDisplayDeath.gameObject.SetActive(false);
        myDayMeter.DayMeterRise(dayMeter.secondsPassed);
		SoundManager.instance.PlaySingle(dayIncrease);
	}

	public void DepleteTrashCollected(){
        GUIManager.Instance.TrashCollectedDisplayDeath.trashDropped = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0];
        GUIManager.Instance.TrashCollectedDisplayDeath.gameObject.SetActive(true);
        GUIManager.Instance.TrashCollectedDisplayDeath.UpdateDisplay(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0]);
        GUIManager.Instance.TrashCollectedDisplayDeath.InvokeRepeating("Deplete",.1f,.1f);
	}

	public void FadeHUD(){
		fader.GetComponent<Animator>().speed = 1;
		fader.GetComponent<Animator>().SetTrigger("FadeOut");
	}

	public void DeathFade(){
		//fader.GetComponent<Animator>().speed = -1;
		fader.SetActive(true);
	
		fader.GetComponent<Animator>().SetTrigger("Fade");
		//fader.GetComponent<Animator>().Play("Fade");
		//fader.GetComponent<Animator>().speed = 1;

	}

	public void PlayTruckSfx(){
		SoundManager.instance.PlaySingle(truckSfx);
	}
}

