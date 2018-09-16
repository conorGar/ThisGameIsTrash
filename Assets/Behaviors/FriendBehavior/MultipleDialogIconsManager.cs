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
		int positionOnScreen = 0;// 0 = left, 1 = center, 2 = right
		for(int i = 0; i < icons.Count; i++){
			if(icons[i].name == newSpeaker){
				Debug.Log("!!!! Names Match up !!!!" + newSpeaker +icons[i].name);
				dialogManager.currentlySpeakingIcon = icons[i];
				newIcon = icons[i];
				positionOnScreen = i;
			}else{
				Debug.Log("ICON FADE SHOULDVE HAPPENED!!!??!" + icons[i].name);
				icons[i].GetComponent<Image>().color = new Color(.55f,.55f,.55f);//new Color(124,124,124);//fadeIconIfNotTalking
			}
		}

		//effects
		newIcon.GetComponent<Image>().color = Color.white;

		iconMoveDir = positionOnScreen;
		movingIcons = true;
		StartCoroutine("MoveIcons");
		SwitchAni(newIcon.GetComponent<MultipleIcon>().myFriend.iconAnimationName);
	}

	public override void SwitchAni(string triggerName){
		int triggerHash = Animator.StringToHash(triggerName);
		dialogManager.currentlySpeakingIcon.GetComponent<Animator>().SetTrigger(triggerHash);
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
}

