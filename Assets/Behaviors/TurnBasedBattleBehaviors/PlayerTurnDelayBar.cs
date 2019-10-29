using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerTurnDelayBar : TurnDelayBar
{

	public Image myBarDisplay;
	public int playerSpeed;

	void Awake(){
		speed = playerSpeed;
	}
	protected override void BarFilledEvent(){
		gameObject.GetComponent<PlayerAttackHandler>().attackPhase = "CHOOSE_ATTACK";
	}

	void Count(){
		base.Count();
		myBarDisplay.fillAmount = turnCounter/100f;
	}
}

