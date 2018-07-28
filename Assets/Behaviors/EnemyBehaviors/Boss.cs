using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

	public int bossNumber;
	public int hp;
	public int attkDmg;
	public GameObject hpDisplay;
	public GameObject objectPool;


	int deathSmokeNumber;
	void Start () {
//jumbo- "How am I suppossed to know if the path I'm on is the right one?!? What if, by the time I realise my mistake...it'll be too late for me to turn back?		

//Activate boss when enter proper room and test Stuart so far

//Stuart Follow and knockback/Take damage properly
//stuart death
//Boss HP display
//------tmw--------
//Questio Slash
//Ex shoot

//Hash protects Stuart
		if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] > hp){
			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = hp;
		}else if(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] < hp && GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] != 0 ){
			GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]++; //regain hp each day
		}

		gameObject.SetActive(false);


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateBoss(){
		gameObject.GetComponent<EnemyTakeDamage>().currentHp = GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber];
		gameObject.GetComponent<EnemyTakeDamage>().bossEnemy = true;
		hpDisplay.SetActive(true);
		hpDisplay.GetComponent<GUI_BossHpDisplay>().maxHp = hp;
		hpDisplay.GetComponent<GUI_BossHpDisplay>().UpdateBossHp(GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber]);
	}

	public IEnumerator BossDeath(){
		Debug.Log("BOSS DEATH ACTIVATE ***********");
		InvokeRepeating("DeathSmoke",.1f,.2f);
		GlobalVariableManager.Instance.BOSS_HP_LIST[bossNumber] = 0;
		yield return new WaitForSeconds(1.5f);
		GameObject deathGhost = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_DeathGhost");
		deathGhost.transform.position = new Vector3((transform.position.x), transform.position.y, transform.position.z);
		this.gameObject.SetActive(false);
	}

	void DeathSmoke(){
		if(deathSmokeNumber < 15){
			GameObject deathSmoke = objectPool.GetComponent<ObjectPool>().GetPooledObject("effect_SmokePuff");
			deathSmoke.transform.position = new Vector3((transform.position.x+ Random.Range(-4,4)), transform.position.y+ Random.Range(-4,4), transform.position.z);
			deathSmokeNumber++;
		}else{
			CancelInvoke();
		}
	}

}
