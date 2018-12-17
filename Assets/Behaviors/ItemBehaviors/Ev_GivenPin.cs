using UnityEngine;
using System.Collections;

public class Ev_GivenPin : MonoBehaviour
{	
	/// <summary>
	/// For pins that are given to the player in scripted events.
	/// </summary>


	public PinDefinition pinData;
	public GameObject pinUnlockHud;
	Sprite mySprite;

	void OnEnable(){
			mySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
			pinUnlockHud.SetActive(true);
			pinUnlockHud.GetComponent<GUI_PinUnlockDisplay>().SetValues(pinData.displayName, pinData.description,mySprite);
			GlobalVariableManager.Instance.PINS_DISCOVERED |= pinData.Type;
			GameStateManager.Instance.PushState(typeof(PopupState));
			gameObject.SetActive(false);
	}
}

