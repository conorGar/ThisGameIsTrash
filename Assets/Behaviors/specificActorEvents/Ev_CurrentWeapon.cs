using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ev_CurrentWeapon : MonoBehaviour {

	float myWidth;
	float fillAmount = 0;

	public GameObject  meleeDisplay;
	public GameObject player;
	//public GameObject meleeMeter;
	public Image meleeMeterDraw;
	public Sprite plankSprite;
	public Sprite clawSprite;
	public Sprite poleSprite;
	public Sprite broomSprite;

	void Start () {
		//meleeMeterDraw = gameObject.GetComponent<Image>();
		//GameObject meleeDisplayInstance;
		//meleeDisplayInstance = Instantiate(meleeDisplay,transform.position, Quaternion.identity);
		//GameObject meleeMeterInstance;
		//meleeMeterInstance = Instantiate(meleeMeter,transform.position,Quaternion.identity);
		UpdateMelee();
		//InvokeRepeating("Test",0.5f,0.5f);
	}
	
	void Update () {
	}

	public void UpdateMelee(){
		
		fillAmount = ( 1- (6f - (GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] % 6f))/6f) ;
		meleeMeterDraw.fillAmount = fillAmount;
		//Debug.Log("todays trash: " + GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1]);
		//Debug.Log("Fill Amount" + fillAmount);

		if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 6 && meleeDisplay.GetComponent<Image>().sprite != plankSprite ){
			//Debug.Log("PLANK");
			meleeDisplay.GetComponent<Image>().sprite = plankSprite;
			player.GetComponent<MeleeAttack>().UpdateWeapon();
		}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 6 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] <= 12 && meleeDisplay.GetComponent<Image>().sprite != clawSprite ){
			meleeDisplay.GetComponent<Image>().sprite = clawSprite;
			player.GetComponent<MeleeAttack>().UpdateWeapon();

		}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] > 12 && GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] < 18 && meleeDisplay.GetComponent<Image>().sprite != poleSprite){
			meleeDisplay.GetComponent<Image>().sprite = poleSprite;
			player.GetComponent<MeleeAttack>().UpdateWeapon();

		}else if(GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1] >= 18 && meleeDisplay.GetComponent<Image>().sprite != broomSprite){ //&& GlobalVariableManager.Instance.IsPinEquipped(PIN.WASTEWARRIOR)){
			meleeDisplay.GetComponent<Image>().sprite = broomSprite;
			player.GetComponent<MeleeAttack>().UpdateWeapon();

		}
	}

	public void Test(){
		GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[1]++;
		UpdateMelee();
	}
}
