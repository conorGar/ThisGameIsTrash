using UnityEngine;
using System.Collections;

public class DayDebugManager : MonoBehaviour
{

	//Built to quickly test systems that have to do with day changes by only having to
	//change the crrentday var here. 

	public int currentDay;

	//TODO: Eventually if the amount of things to change gets too long, create a parent class for all these managers and just make a list of them to call from
	public IceCreamTownManager iceCreamTownManager;


	void Awake(){
		currentDay = GlobalVariableManager.Instance.DAY_NUMBER;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(GlobalVariableManager.Instance.DAY_NUMBER != currentDay){
			//Go through all managers and run any code that needs to happen at day's start 
			GlobalVariableManager.Instance.DAY_NUMBER = currentDay;
			iceCreamTownManager.DebugDayChange();
		}
	}
}

