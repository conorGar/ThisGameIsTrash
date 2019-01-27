using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_Ev_PollutedPeakStairs : MonoBehaviour {

	public GameObject blockade;
	public GameObject blockadeSignText;
	public GameObject normalTextSign;


	// Use this for initialization
	void Awake () {
		if(GlobalVariableManager.Instance.STAR_POINTS_STAT.GetMax() > 2){
			blockade.SetActive(false);
		}else{
			blockadeSignText.SetActive(true);
			normalTextSign.SetActive(false);
			Debug.Log(GlobalVariableManager.Instance.STAR_POINTS_STAT.GetMax());
		}
	}


	void Start(){
		GameStateManager.Instance.PushState(typeof(GameplayState));
	}
	// Update is called once per frame
	void Update () {
		
	}
}
