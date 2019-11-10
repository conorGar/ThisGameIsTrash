using UnityEngine;
using System.Collections;

public class HeroAttacker : MonoBehaviour
{
	public int currentHP;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TakeDamage(int damage){
		Debug.Log(gameObject.name + "Took" + damage + "damage!");
		currentHP -= damage;
		GlobalVariableManager.Instance.HP_STAT.UpdateCurrent(currentHP);

        PlayerManager.Instance.controller.SendTrigger(JimTrigger.HIT);
        SpawnDamageStars(damage);
		Debug.Log("reached this end of hp hud change" + GlobalVariableManager.Instance.HP_STAT.GetCurrent());


		//TODO:Check for death
	}

	void SpawnDamageStars(int damageDealt){
		GameObject damageCounter = ObjectPool.Instance.GetPooledObject("HitStars_player",this.gameObject.transform.position);
		damageCounter.GetComponent<Ev_HitStars>().ShowProperDamage(damageDealt);
		damageCounter.SetActive(true);
		GameObject littleStars = ObjectPool.Instance.GetPooledObject("effect_LittleStars",this.gameObject.transform.position);
		littleStars.SetActive(true);
        CamManager.Instance.mainCam.ScreenShake(.2f);
		damageCounter.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f,10f), ForceMode2D.Impulse);
		
	}
}

