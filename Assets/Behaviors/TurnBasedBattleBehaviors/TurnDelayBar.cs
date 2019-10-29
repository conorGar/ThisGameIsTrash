using UnityEngine;
using System.Collections;

public class TurnDelayBar : MonoBehaviour
{

	public int turnCounter = 0; //only public for debugging with inspector visuals
	protected int speed = 2;

	public void StartCount(){
		turnCounter = 0;
		InvokeRepeating("Count",0,.1f);
	}

	public void ResumeCount(){
		InvokeRepeating("Count",0,.1f); 
	}

	public void Pause(){ //called by BattleManager
		CancelInvoke();
	}


	protected virtual void Count(){
		if(turnCounter < 100){
			turnCounter += speed; 
		}else{
			BarFilledEvent();
			CancelInvoke();
		}
	}

	protected virtual void BarFilledEvent(){
		//Nothing for basic TurnDelayBar
	}
}

