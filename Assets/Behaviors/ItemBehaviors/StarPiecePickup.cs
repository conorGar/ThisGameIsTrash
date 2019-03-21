using UnityEngine;
using System.Collections;

public class StarPiecePickup : MonoBehaviour
{

	public GameObject tempStarPieceInfoDisplay;

	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.tag == "Player"){
			if (GameStateManager.Instance.GetCurrentState() == typeof(GameplayState)) {
				if(GlobalVariableManager.Instance.STAR_BITS_STAT.GetCurrent() == 0){
					tempStarPieceInfoDisplay.SetActive(true);
					GameStateManager.Instance.PushState(typeof(PopupState));

				}


				GlobalVariableManager.Instance.STAR_BITS_STAT.UpdateCurrent(GlobalVariableManager.Instance.STAR_BITS_STAT.GetCurrent() + 1);

				//TODO: effects

				//When collect 3 star bits, gain a star
				if(GlobalVariableManager.Instance.STAR_BITS_STAT.GetCurrent() >= GlobalVariableManager.Instance.STAR_BITS_STAT.GetMax()){
					GlobalVariableManager.Instance.STAR_POINTS_STAT.UpdateCurrent(GlobalVariableManager.Instance.STAR_POINTS_STAT.GetCurrent() + 1);
					GlobalVariableManager.Instance.STAR_BITS_STAT.SetCurrent(0); // reset back to zero star bits
				}
				ObjectPool.Instance.GetPooledObject("effect_heal", gameObject.transform.position);

				gameObject.SetActive(false);
			}
		}
	}
}

