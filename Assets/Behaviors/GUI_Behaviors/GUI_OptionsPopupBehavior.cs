using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_OptionsPopupBehavior : MonoBehaviour {


	public int howManyOptions = 2;
	public Text option1;
	public Text option2;
	public int closeOptionNumber;

	int arrowPos = 1;
	Color startColor;
	GameObject objectToActivate;



	void Start () {
		startColor = option1.GetComponent<Text>().color;
		//gameObject.transform.parent = GameObject.Find("HUD").transform;
		//gameObject.transform.localPosition = new Vector3(0f,0f,0f);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			if(arrowPos < howManyOptions){
				arrowPos++;
				option1.GetComponent<Text>().color = new Color(startColor.r,startColor.b,startColor.g, .3f);
				option2.GetComponent<Text>().color = new Color(startColor.r,startColor.b,startColor.g, 1f);
			}
		}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			if(1 < arrowPos){
				arrowPos--;
				option1.GetComponent<Text>().color = new Color(startColor.r,startColor.b,startColor.g, 1f);;
				option2.GetComponent<Text>().color = new Color(startColor.r,startColor.b,startColor.g, .3f);
			}
		}else if(Input.GetKeyDown(KeyCode.Space)){
			if(arrowPos == closeOptionNumber){
				Close();
			}else{
				Option1Activation();
			}
		}
	}

	void Close(){
		if(gameObject.name == "EndDayPopUp"){
			GameObject dumpster = GameObject.Find("Dumpster");
			dumpster.GetComponent<SE_GlowWhenClose>().enabled = true;
			dumpster.GetComponent<SE_GlowWhenClose>().SetGlowCheck(0);

		}else if(gameObject.name == "purchasePopup"){
			objectToActivate.GetComponent<Ev_PinBehavior>().SetPopupBack();
		}
		GlobalVariableManager.Instance.PLAYER_CAN_MOVE = true; //at least needed for pin purchase option close
		gameObject.SetActive(false);

	}

	void Option1Activation(){
		if(gameObject.name == "EndDayPopUp"){
			GameObject fadeHelp = GameObject.Find("fadeHelper");
			fadeHelp.GetComponent<Ev_FadeHelper>().EndOfDayFade();
			Destroy(gameObject);
		}else if(gameObject.name == "purchasePopup"){
			objectToActivate.GetComponent<Ev_PinBehavior>().ShopPurchase();
			gameObject.SetActive(false);
		}
	}

	public void setGameObjectToActivate(GameObject go){
		objectToActivate = go;
	}
}
