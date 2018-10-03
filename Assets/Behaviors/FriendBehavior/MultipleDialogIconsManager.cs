using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultipleDialogIconsManager : DialogIconAnimationManager
{

	public DialogManager dialogManager;

	public List<GameObject> icons = new List<GameObject>();
	bool movingIcons;
	int iconMoveDir;
	//int previouslyHighlightedIcon;// Just needed for 3- character dialogs for the character in the middle
	// Use this for initialization
	void OnEnable(){
		dialogManager.multipleIconsManager = this;
	}

	void Update(){
		if(movingIcons){
			if(iconMoveDir == 0){
				gameObject.transform.Translate(Vector2.right*Time.deltaTime);//just move the parent
			}else if(iconMoveDir == 1){
			//TODO: dtermine if move righ or let for middle icon, or maybe middle icon always stays the same
			}else if(iconMoveDir == 2){
				gameObject.transform.Translate(Vector2.left*Time.deltaTime);//just move the parent
			}
		}
	}

	public void ChangeSpeaker(string newSpeaker){
		GameObject newIcon = null;
		Debug.Log("ChangeSpeaker() activate --x-x-x-x-x-x-x-x-x-x-" );
		for(int i = 0; i < icons.Count; i++){
			if(icons[i].name == newSpeaker){
				Debug.Log("!!!! Names Match up !!!!" + newSpeaker +icons[i].name);
				dialogManager.currentlySpeakingIcon = icons[i];
				newIcon = icons[i];

				//enter new character icon
				if(!icons[i].activeInHierarchy){
					icons[i].SetActive(true);
				}

			}else{
				Debug.Log("ICON FADE SHOULDVE HAPPENED!!!??!" + icons[i].name);
				icons[i].GetComponent<Image>().color = new Color(.55f,.55f,.55f);//new Color(124,124,124);//fadeIconIfNotTalking
				icons[i].GetComponent<MultipleIcon>().ReturnToPosition();
			}
		}

		//effects
		newIcon.GetComponent<Image>().color = Color.white;

		iconMoveDir = newIcon.GetComponent<MultipleIcon>().positionOnScreen;
		movingIcons = true;
		StartCoroutine("MoveIcons");
		SwitchAni(newIcon.GetComponent<MultipleIcon>().myIconAniTrigger);
	}

	public override void SwitchAni(string triggerName){
		int triggerHash = Animator.StringToHash(triggerName);
		if(triggerName == "IconSlide"){
			//disable other icons when iconSlide
			for(int i = 0; i < icons.Count; i++){
				if(icons[i] != dialogManager.currentlySpeakingIcon){
					icons[i].SetActive(false);
				}
			}
		}
		//dialogManager.currentlySpeakingIcon.GetComponent<Animator>().StopPlayback();
		if(dialogManager.currentlySpeakingIcon != null){
			Debug.Log("-x-x-x-x-x-x-x- SWITCHING TO ANI WITH TRIGGER NAME:" + triggerName);
			dialogManager.currentlySpeakingIcon.GetComponent<Animator>().enabled = true;
			Debug.Log(		dialogManager.currentlySpeakingIcon.GetComponent<Animator>().enabled);
			dialogManager.currentlySpeakingIcon.GetComponent<Animator>().SetTrigger(triggerHash);
			dialogManager.currentlySpeakingIcon.GetComponent<Animator>().Play(triggerName);
		}
		if(triggerName == "IconSlideBack"){
			Debug.Log("ICON SLIDE BACK PROPERLY READ-x-x-x-x-x-x-x-x-x-");
			for(int i = 0; i < icons.Count; i++){
				if(!icons[i].activeInHierarchy){
					icons[i].SetActive(true);
				}
			}
		}
//		Debug.Log(		dialogManager.currentlySpeakingIcon.GetComponent<Animator>().enabled);

	}

	IEnumerator MoveIcons(){
		yield return new WaitForSeconds(.3f);
		movingIcons = false;
	}

	public void EnterNewIcon(string iconName, string enterDirection){
		GameObject newIcon = null;
		for(int i = 0; i < icons.Count; i++){
			if(icons[i].name == iconName){
				newIcon = icons[i];
				break;
			}
		}
		if(enterDirection == "left"){
			newIcon.transform.localPosition = new Vector2(-50f,6f);
		}else{
			newIcon.transform.localPosition = new Vector2(40f,6f); //right side
		}

		newIcon.GetComponent<GUIEffects>().Start();
	}

	public void SetStartingIcons(string[] iconNames){
		

		for(int i = 0; i < icons.Count; i++){
			icons[i].SetActive(false); //deactivate all icons
			for(int j = 0; j < iconNames.Length;j++){
				Debug.Log(iconNames[j] + icons[i].name +(iconNames[j] == icons[i].name) );
				if(iconNames[j] == icons[i].name){
					icons[i].SetActive(true);
				}
			}
		}


	}

}

