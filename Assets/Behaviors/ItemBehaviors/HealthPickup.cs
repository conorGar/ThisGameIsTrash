using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	void OnTriggerEnter2D(Collider2D collider){
		if(collider.gameObject.tag == "Player" && GlobalVariableManager.Instance.HP_STAT.GetCurrent() < GlobalVariableManager.Instance.HP_STAT.GetMax()){
			SoundManager.instance.PlaySingle(SFXBANK.HEAL);
			ObjectPool.Instance.GetPooledObject("effect_heal", gameObject.transform.position);
			collider.gameObject.GetComponent<PlayerTakeDamage>().Heal(1);
			gameObject.SetActive(false);
		}
	}
}

