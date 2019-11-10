using UnityEngine;
using System.Collections;

public class TurnDelayBar : MonoBehaviour
{

	public int turnCounter = 0; //only public for debugging with inspector visuals
	protected int speed = 2;
	bool barFilled = false; // makes sure 'BarFilledEvent' is only called once

	public void StartCount(){
		turnCounter = 0;
		barFilled = false;
		InvokeRepeating("Count",0,.1f);
	}

	public void ResumeCount(){
		InvokeRepeating("Count",0,.1f); 
	}

	public void Pause(){ //called by BattleManager
		CancelInvoke();
	}


	protected virtual void Count(){
		if(GameStateManager.Instance.GetCurrentState() == typeof(BattleState)){
			if(turnCounter < 100){
				turnCounter += speed; 
			}else{
				if(!barFilled){
					BarFilledEvent();
				}
				CancelInvoke();
			}
		}
	}

	protected virtual void BarFilledEvent(){
		barFilled = true;
		//Nothing for basic TurnDelayBar
	}
}

