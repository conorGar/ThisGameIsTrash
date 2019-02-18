using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultipleDialogIconsManager : DialogIconAnimationManager
{

	public DialogManager dialogManager;
	public bool talkingIconsMoveToFront;
	public List<MultipleIcon> icons = new List<MultipleIcon>();
    public int currentSpeakerIndex = 0;
	bool movingIcons;
	int iconMoveDir;
    //int previouslyHighlightedIcon;// Just needed for 3- character dialogs for the character in the middle

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

	public void ChangeSpeaker(string newSpeaker, bool isMoveIcons = true){
		Debug.Log("ChangeSpeaker() activate --x-x-x-x-x-x-x-x-x-x-" );
		for(int i = 0; i < icons.Count; i++){
			if(icons[i].name == newSpeaker){
				Debug.Log("!!!! Names Match up !!!!" + newSpeaker +icons[i].name);
                currentSpeakerIndex = i;

				//enter new character icon
				if(!icons[i].gameObject.activeInHierarchy){
					icons[i].gameObject.SetActive(true);
				}
				if(talkingIconsMoveToFront){
					icons[i].transform.SetSiblingIndex(icons.Count);
				}
			}else{
				Debug.Log("ICON FADE SHOULDVE HAPPENED!!!??!" + icons[i].name);
				icons[i].GetComponent<Image>().color = new Color(.55f,.55f,.55f);//new Color(124,124,124);//fadeIconIfNotTalking
				icons[i].ReturnToPosition();
            }
		}

		//effects
		icons[currentSpeakerIndex].GetComponent<Image>().color = Color.white;

        if (isMoveIcons) {
            iconMoveDir = icons[currentSpeakerIndex].positionOnScreen;
            movingIcons = true;
            StartCoroutine("MoveIcons");
        }
	}

    public override void SetTalking(bool talking)
    {
        // Disable all talking.  Or, disable all talking but the current speaker.
        for (int i = 0; i < icons.Count; i++) {
            if (i == currentSpeakerIndex && talking) {
                icons[i].isTalking = true;
            }
            else {
                icons[i].isTalking = false;
            }
            icons[i].animator.SetBool("IsTalking", icons[i].isTalking);
        }
    }

    public override void Slide()
    {
        //hide other icons.
        for (int i = 0; i < icons.Count; i++) {
            if (icons[i] != icons[currentSpeakerIndex]) {
                icons[i].gameObject.SetActive(false);
            }
        }

        if (dialogManager.currentlySpeakingIcon != null) {
            icons[currentSpeakerIndex].animator.SetTrigger("Slide");
        }
    }

    public override void SlideBack()
    {
        // Show all icons.
        for (int i = 0; i < icons.Count; i++) {
            if (!icons[i].gameObject.activeInHierarchy) {
                icons[i].gameObject.SetActive(true);
            }
        }

        if (dialogManager.currentlySpeakingIcon != null) {
            icons[currentSpeakerIndex].animator.SetTrigger("SlideBack");
        }
    }

    public override void SetAnimTrigger(string action)
    {
        if (dialogManager.currentlySpeakingIcon != null) {
            icons[currentSpeakerIndex].animator.SetTrigger(action);
        }
    }

    public override void SetAnimBool(string key, bool value)
    {
        if (dialogManager.currentlySpeakingIcon != null) {
            icons[currentSpeakerIndex].animator.SetBool(key, value);
        }
    }

    public override void ResetIconPositionsOnScreen()
    {
        // Reset any custom position on screen changes that took place.
        for (int i = 0; i < icons.Count; i++) {
            icons[i].ResetPositionOnScreen();
        }
    }

    IEnumerator MoveIcons(){
		yield return new WaitForSeconds(.3f);
		movingIcons = false;
	}

	public void EnterNewIcon(string iconName, string enterDirection){
		MultipleIcon newIcon = null;
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

	public void SetStartingIcons(string firstIcon = "", string secondIcon = "", string thirdIcon = "")
    {
        // if no icons are defined, show everything.
        if (string.IsNullOrEmpty(firstIcon) &&
            string.IsNullOrEmpty(secondIcon) &&
            string.IsNullOrEmpty(thirdIcon)) {
            for (int i = 0; i < icons.Count; i++) {
                icons[i].gameObject.SetActive(true);
            }
            return;
        }

        for (int i = 0; i < icons.Count; i++) {
            if ((!string.IsNullOrEmpty(firstIcon)  && icons[i].name == firstIcon) ||
                (!string.IsNullOrEmpty(secondIcon) && icons[i].name == secondIcon) ||
                (!string.IsNullOrEmpty(thirdIcon)  && icons[i].name == thirdIcon)) {
                icons[i].gameObject.SetActive(true);
            }
            else {
                icons[i].gameObject.SetActive(false);
            }
        }
	}

}

