using UnityEngine;
using System.Collections;
using TMPro;
using I2.TextAnimation;

public class GUI_SlabTrashGiveHUD : GUI_MenuBase
{
	int maxTrashToGive;
	public TextMeshProUGUI trashToGive;
	public TextMeshProUGUI trashTakenAway;
	public SlabFriend slabFriend;

	// Use this for initialization
	void Start ()
	{
	
	}

	void OnEnable(){
        arrowPos = 0;
        trashToGive.text = arrowPos.ToString();
        trashTakenAway.text = "-" + arrowPos.ToString();
        maxTrashToGive = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVERIGHT) && arrowPos < maxTrashToGive){
			Navigate("right");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.MOVELEFT) && arrowPos >0){
			Navigate("left");
		}else if(ControllerManager.Instance.GetKeyDown(INPUTACTION.INTERACT)){
			slabFriend.AddTrashToFund(arrowPos);
			gameObject.SetActive(false);
		}
	}

	public override void NavigateEffect(){
		trashToGive.text = arrowPos.ToString();
		trashTakenAway.text = "-" + arrowPos.ToString();
		trashToGive.gameObject.GetComponent<TextAnimation>().PlayAnim();
	}
}

