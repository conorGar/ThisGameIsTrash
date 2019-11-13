using UnityEngine;
using System.Collections;

public class HeroAttacker : MonoBehaviour
{
	public int currentHP;
	private EnemyStateController controller;


	// Use this for initialization
	void Start ()
	{
		controller = gameObject.GetComponent<EnemyStateController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TakeDamage(int damage){
		Debug.Log(gameObject.name + "Took" + damage + "damage!");
		currentHP -= damage;
		GlobalVariableManager.Instance.HP_STAT.UpdateCurrent(currentHP);
		if(controller){ //use the prescence of a HeroStateController to determine if this is Jim or not
			controller.SendTrigger(EnemyTrigger.HIT);
		}else{
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.HIT);
		}
        SpawnDamageStars(damage);
		Debug.Log("reached this end of hp hud change" + GlobalVariableManager.Instance.HP_STAT.GetCurrent());

		if(currentHP <= 0){
			
			StartCoroutine("Death");
		}

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

	IEnumerator Death(){
		BattleManager.Instance.RemoveHero(this);

		Time.timeScale = 0.3f;
		yield return new WaitForSeconds(.3f);
		Time.timeScale = 1;
		if(controller){ //use the prescence of a HeroStateController to determine if this is Jim or not
			controller.SendTrigger(EnemyTrigger.DEATH);
		}else{
			PlayerManager.Instance.controller.SendTrigger(JimTrigger.DEATH);
		}
		GameObject deathSmoke = ObjectPool.Instance.GetPooledObject("effect_SmokePuff"); 
		deathSmoke.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		GameObject deathGhost = ObjectPool.Instance.GetPooledObject("effect_DeathGhost",new Vector3((transform.position.x), transform.position.y, transform.position.z));
		deathGhost.GetComponent<Ev_DeathGhost>().OnSpawn();

		this.gameObject.SetActive(false);
	}


}

