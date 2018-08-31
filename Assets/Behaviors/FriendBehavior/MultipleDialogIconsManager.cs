using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultipleDialogIconsManager : MonoBehaviour
{

	public DialogManager dialogManager;

	public List<GameObject> icons = new List<GameObject>();
	bool movingIcons;
	int iconMoveDir;
	//int previouslyHighlightedIcon;// Just needed for 3- character dialogs for the character in the middle
	// Use this for initialization
	void Start ()
	{
		
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
		int positionOnScreen = 0;// 0 = left, 1 = center, 2 = right
		for(int i = 0; i < icons.Count; i++){
			if(icons[i].name == newSpeaker){
				dialogManager.currentlySpeakingIcon = icons[i];
				newIcon = icons[i];
				positionOnScreen = i;
			}else{
				icons[i].GetComponent<Image>().color = new Color(124,124,124);//fadeIconIfNotTalking
			}
		}

		//effects
		newIcon.GetComponent<Image>().color = Color.white;
		iconMoveDir = positionOnScreen;
		movingIcons = true;
		StartCoroutine("MoveIcons");
	}

	IEnumerator MoveIcons(){
		yield return new WaitForSeconds(.3f);
		movingIcons = false;
	}
}

