using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ev_Results_BossIcon : MonoBehaviour
{
	public GlobalVariableManager.BOSSES thisBoss;
	public Image myCheckBox;
	public Sprite checkboxMarked;
	// Use this for initialization
	void OnEnable ()
	{
		if((GlobalVariableManager.Instance.BOSSES_KILLED & thisBoss) == thisBoss){
			myCheckBox.sprite = checkboxMarked;
		}
	}
	

}

