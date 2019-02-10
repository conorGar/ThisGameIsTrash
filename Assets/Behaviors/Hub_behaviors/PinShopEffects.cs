using UnityEngine;
using System.Collections;

public class PinShopEffects : MonoBehaviour
{

	public S_Ev_shop shopGUI;
	public AudioClip shopMusic;

	void OnTriggerEnter2D(Collider2D col){
		shopGUI.EnterShop();
		SoundManager.instance.SwitchToOtherBGM(shopMusic);
	}

	void OnTriggerExit2D(Collider2D col){
		Debug.Log("-x-x-x-x-x-   SHOP EXITED      -x-x-x-x-x-x");
		shopGUI.ExitShop();
		SoundManager.instance.ReturnToMainBGM();
	}
}

