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
		maxTrashToGive = GlobalVariableManager.Instance.TODAYS_TRASH_AQUIRED[0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.DownArrow) && arrowPos < maxTrashToGive){
			Navigate("down");
		}else if(Input.GetKeyDown(KeyCode.UpArrow)){
			Navigate("up");
		}else if(Input.GetKeyDown(KeyCode.Space)){
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

